using Sonata.Assets.DotnetScanner.Projects.Entities;
using Sonata.Assets.Scanner.Core;
using System.Xml.Linq;

namespace Sonata.Assets.DotnetScanner.Projects
{
    public class ProjectsScanner : IScanner<ProjectsScannerInput, ProjectScannerOutput>
    {
        ProjectScannerOutput output = new ProjectScannerOutput();

        public ProjectScannerOutput Scan(ProjectsScannerInput input)
        {
            if (!Directory.Exists(input.ProjectPath))
            {
                string message = "Directory does not exists";
                output.Error = new Exception(message);
                return output;
            }

            try
            {
                foreach (string csProjFileNmae in Directory.GetFiles(input.ProjectPath, "*.csProj", SearchOption.AllDirectories))
                {
                    //Get details from PropertyGroup
                    getDetailsFromPropertyGroup(Path.Combine(input.ProjectPath, csProjFileNmae));
                    //get details from ItemGroup, //Get project references
                    readReferencesFromItemGroup(csProjFileNmae);
                }
            }
            catch (Exception ex)
            {
                output.Error = ex;
            }
            return output;
        }

        private void getDetailsFromPropertyGroup(string fileNameAndPath)
        {
            XNamespace msbuild = "http://schemas.microsoft.com/developer/msbuild/2003";

            XDocument projDefinition = XDocument.Load(fileNameAndPath);

            //Project_ToolsVersion

            IEnumerable<string> projToolVersions = projDefinition
                .Element(msbuild + "Project")
                .Attributes("ToolsVersion")
                .Select(refElem => refElem.Value);
            foreach (string projToolVersion in projToolVersions)
            {
                output.CSprojProperies.Add(new ProjProperty
                {
                    ProjectName = fileNameAndPath,
                    Project_ToolsVersion = projToolVersion
                });
            }
            //End Project_ToolsVersion

            IEnumerable<string> projGUIDs = projDefinition
                .Element(msbuild + "Project")
                .Elements(msbuild + "PropertyGroup")
                .Elements(msbuild + "ProjectGuid")
                .Select(refElem => refElem.Value);

            foreach (string projGUID in projGUIDs)
            {
                output.CSprojProperies.Add(new ProjProperty
                {
                    ProjectName = fileNameAndPath,
                    ProjectGuid = projGUID
                });
            }
            //OutputType
            IEnumerable<string> OutputTypes = projDefinition
            .Element(msbuild + "Project")
            .Elements(msbuild + "PropertyGroup")
            .Elements(msbuild + "OutputType")
            .Select(refElem => refElem.Value);

            foreach (string outputType in OutputTypes)
            {
                output.CSprojProperies.Add(new ProjProperty
                {
                    ProjectName = fileNameAndPath,
                    OutputType = outputType
                });
                Console.WriteLine(outputType);
            }
            //TargetFrameworkVersion
            IEnumerable<string> TargetFrameworkVersion = projDefinition
            .Element(msbuild + "Project")
            .Elements(msbuild + "PropertyGroup")
            .Elements(msbuild + "TargetFrameworkVersion")
            .Select(refElem => refElem.Value);

            foreach (string targetFrameworkVersion in TargetFrameworkVersion)
            {
                output.CSprojProperies.Add(new ProjProperty
                {
                    ProjectName = fileNameAndPath,
                    TargetFrameworkVersion = targetFrameworkVersion
                });
                Console.WriteLine(targetFrameworkVersion);
            }
        }
        private void readReferencesFromItemGroup(string fileNameAndPath)
        {
            //Concole Application
            //Get reference
            XNamespace msbuild = "http://schemas.microsoft.com/developer/msbuild/2003";
            XDocument projDefinition = XDocument.Load(fileNameAndPath);
            IEnumerable<string> references = projDefinition
                .Element(msbuild + "Project")
                .Elements(msbuild + "ItemGroup")
                .Elements(msbuild + "Reference")
                .Attributes("Include")    // This is where the reference is mentioned
                .Select(refElem => refElem.Value);
            foreach (string reference in references)
            {
                output.CSprojReferences.Add(new ProjReferences
                {
                    ProjectName = fileNameAndPath,
                    Reference = reference
                });
                Console.WriteLine(reference);
            }

            //Projectreference
            IEnumerable<string> projectReferences = projDefinition
                .Element(msbuild + "Project")
                .Elements(msbuild + "ItemGroup")
                .Elements(msbuild + "ProjectReference")
                .Attributes("Include")    // This is where the reference is mentioned
                .Select(refElem => refElem.Value);
            foreach (string projReference in projectReferences)
            {
                output.CSprojReferences.Add(new ProjReferences
                {
                    ProjectName = fileNameAndPath,
                    ProjectReference = projReference
                });
                Console.WriteLine(projReference);
            }

            //Content include
            IEnumerable<string> projectContents = projDefinition
                .Element(msbuild + "Project")
                .Elements(msbuild + "ItemGroup")
                .Elements(msbuild + "Content")
                .Attributes("Include")    // This is where the reference is mentioned
                .Select(refElem => refElem.Value);
            foreach (string projectContent in projectContents)
            {
                output.CSprojReferences.Add(new ProjReferences
                {
                    ProjectName = fileNameAndPath,
                    ProjectContent = projectContent
                });
                Console.WriteLine(projectContent);
            }

            //Compile include
            IEnumerable<string> projectCompiles = projDefinition
                .Element(msbuild + "Project")
                .Elements(msbuild + "ItemGroup")
                .Elements(msbuild + "Compile")
                .Attributes("Include")    // This is where the reference is mentioned
                .Select(refElem => refElem.Value);
            foreach (string projCompile in projectCompiles)
            {
                output.CSprojReferences.Add(new ProjReferences
                {
                    ProjectName = fileNameAndPath,
                    ProjectCompile = projCompile
                });
                Console.WriteLine(projCompile);
            }
        }
    }
}