namespace BackendServiceExtractor.Shared
{
    using Microsoft.CodeAnalysis.MSBuild;
    using System.Collections.Generic;
    public class SolutionFileReader
    {
        public class SolutionModel
        {
            public string SolutionName { get; set; }
            public List<ProjectModel> Projects { get; set; } = new List<ProjectModel>();
        }

        public class ProjectModel
        {
            public string ProjectName { get; set; }
            public List<DocumentModel> Documents { get; set; } = new List<DocumentModel>();
        }

        public class DocumentModel
        {
            public string DocumentName { get; set; }
            public string Code { get; set; }
        }

        public class SolutionReader
        {
            public SolutionModel LoadSolution(string solutionPath)
            {
                MSBuildWorkspace workspace = MSBuildWorkspace.Create();
                var solution = workspace.OpenSolutionAsync(solutionPath).Result;

                SolutionModel solutionModel = new SolutionModel
                {
                    SolutionName = solution.FilePath
                };

                foreach (var project in solution.Projects)
                {
                    ProjectModel projectModel = new ProjectModel
                    {
                        ProjectName = project.Name
                    };

                    foreach (var document in project.Documents)
                    {
                        var syntaxTree = document.GetSyntaxTreeAsync().Result;
                        var code = syntaxTree.GetText().ToString();

                        DocumentModel documentModel = new DocumentModel
                        {
                            DocumentName = document.Name,
                            Code = code
                        };

                        projectModel.Documents.Add(documentModel);
                    }

                    solutionModel.Projects.Add(projectModel);
                }

                return solutionModel;
            }
        }
    }
}
