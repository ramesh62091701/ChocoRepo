using ServiceExtractor;
using System.Text.Json;

namespace Service.Extractor.Console
{
    internal class Extractor
    {
        private Options _options;

        private Root _root;

        internal Extractor(Options options)
        {
            _options = options;

            using FileStream stream = File.OpenRead(_options.JsonPath);
            _root = JsonSerializer.Deserialize<Root>(stream) ?? throw new NullReferenceException();
        }

        internal void Extract()
        {
            var newFolder = Path.Combine(_options.PathForNewProject, _root.WorkspaceName);
            Directory.CreateDirectory(newFolder);

            var sourceFile = _root.SourceFileResults.Single(x =>
                x.Children.Any(y => string.Equals(y.Type, "namespace") && y.Children.Any(z =>
                    string.Equals(z.Type, "class") && string.Equals(z.Identifier, _options.Controller))));

            // CopyFile the file to the new path
            CopyFile(sourceFile.FileFullPath, newFolder);

            var baseClasses = sourceFile.Children.SingleOrDefault(x => string.Equals(x.Type, "namespace"))
                ?.Children.SingleOrDefault(z => string.Equals(z.Type, "class") && string.Equals(z.Identifier, _options.Controller))?.BaseList ?? new List<string>();
            foreach (var baseType in baseClasses)
            {
                sourceFile = _root.SourceFileResults.SingleOrDefault(x =>
                    x.Children.Any(y => string.Equals(y.Type, "namespace") && y.Children.Any(z =>
                        string.Equals(z.Type, "class") && string.Equals(z.FullIdentifier, baseType))));

                // CopyFile the file to the new path
                if (sourceFile != null)
                    CopyFile(sourceFile.FileFullPath, newFolder);
            }

            // Copy .csproj
            CopyFile(_root.WorkspacePath, newFolder);

            // Copy all Project references
            foreach (var projectReference in _root.ExternalReferences.Project)
            {
                CopyDirectory(projectReference.AssemblyLocation, _options.PathForNewProject);
            }

            // Copy all other files than those doesn't end with "Controller"
            foreach (var file in _root.SourceFiles.Where(x => !x.EndsWith("Controller.cs") && !x.StartsWith("obj")))
            {
                var sourceFileResult = _root.SourceFileResults.Single(x => string.Equals(x.FilePath, file));
                CopyFile(sourceFileResult.FileFullPath, newFolder);
            }

            //Copy content files
            foreach (var file in _root.ContentFiles)
            {
                var filePath = Path.Combine(_root.WorkspaceRootPath, file);
                if (File.Exists(filePath))
                    CopyFile(filePath, newFolder);
            }

            // Construct VSSolution from Solution path
            ModifyAndCopySolution(_options.PathForNewProject);
        }

        private void ModifyAndCopySolution(string path)
        {
            var vsSolution = new VSSolutionReader(_root.SolutionPath).VSSolution;

            // Remove all unwanted project references from vsSolution
            // 1. Get all project reference Guids
            var allProjectReferences = _root.ExternalReferences.Project.Select(x => x.Identifier);
            var projectIds = vsSolution.VSBody.Projects.Where(x => allProjectReferences.Contains(x.Name)).Select(x => x.Identifier).ToList();

            // Also add the self-project 
            projectIds.Add(vsSolution.VSBody.Projects.Single(x => string.Equals(x.Name, _root.WorkspaceName)).Identifier);

            // 2. Remove all ProjectConfigurations
            var projectConfigurationSection = vsSolution.VSBody.Global.GlobalSections.Single(x => x.GetType() == typeof(ProjectConfigurationPlatformsGlobalSection)) as ProjectConfigurationPlatformsGlobalSection;
            projectConfigurationSection.ProjectConfigurations.RemoveAll(x => !projectIds.Contains(x.Identifier));

            // 3. Remove all NestedProjects
            var nestProjectsSection = vsSolution.VSBody.Global.GlobalSections.SingleOrDefault(x => x.GetType() == typeof(NestedProjectsGlobalSection)) as NestedProjectsGlobalSection;
            nestProjectsSection.NestedProjects.RemoveAll(x => !projectIds.Contains(x.NestedProjectIdentifier));

            // 4. Remove from Projects
            vsSolution.VSBody.Projects.RemoveAll(x => !projectIds.Contains(x.Identifier));

            var vsWriter = new VSSolutionWriter(vsSolution);
            vsWriter.Write($"{path}\\{_root.WorkspaceName}.sln");
        }

        private void CopyFile(string fileFullPath, string newFolder)
        {
            var relativePath = fileFullPath.Split($"{_root.WorkspaceName}\\")[1];
            var filePath = Path.Combine(newFolder, relativePath);
            Directory.CreateDirectory(Path.GetDirectoryName(filePath) ?? throw new NotSupportedException());

            if (File.Exists(filePath))
                File.Delete(filePath);

            File.Copy(fileFullPath, filePath);
        }

        private void CopyDirectory(string projectFullPath, string newFolder)
        {
            var sourceDir = Path.GetDirectoryName(projectFullPath);
            var dirName = new DirectoryInfo(sourceDir).Name;
            CopyDirectory(sourceDir, $"{newFolder}\\{dirName}", true);
        }

        private void CopyDirectory(string sourceDir, string destinationDir, bool recursive)
        {
            // Get information about the source directory
            var dir = new DirectoryInfo(sourceDir);

            // Check if the source directory exists
            if (!dir.Exists)
                throw new DirectoryNotFoundException($"Source directory not found: {dir.FullName}");

            // Cache directories before we start copying
            DirectoryInfo[] dirs = dir.GetDirectories();

            if (Directory.Exists(destinationDir))
                Directory.Delete(destinationDir, true);

            // Create the destination directory
            Directory.CreateDirectory(destinationDir);

            // Get the files in the source directory and copy to the destination directory
            foreach (FileInfo file in dir.GetFiles())
            {
                string targetFilePath = Path.Combine(destinationDir, file.Name);
                file.CopyTo(targetFilePath);
            }

            // If recursive and copying subdirectories, recursively call this method
            if (recursive)
            {
                foreach (DirectoryInfo subDir in dirs)
                {
                    string newDestinationDir = Path.Combine(destinationDir, subDir.Name);
                    CopyDirectory(subDir.FullName, newDestinationDir, true);
                }
            }
        }
    }
}
