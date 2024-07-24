using Extractor.Model;
using Extractor.Utils;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.CodeAnalysis.MSBuild;
using Microsoft.Extensions.DependencyInjection;
using Newtonsoft.Json;
using System.Diagnostics;
using System.Net;

namespace Extractor.Service
{
    public static class BackendProcess
    {
        private static IServiceProvider _serviceProvider;
        static BackendProcess()
        {
            _serviceProvider = ConfigurationSetup.ConfigureServices();
        }
        public static async Task GetAllMethodsInClass(BERequest request)
        {
            string solutionFilePath = request.SolutionPath;


            MSBuildWorkspace workspace = MSBuildWorkspace.Create();
            Solution solution = await workspace.OpenSolutionAsync(solutionFilePath);

            if (solution == null)
            {
                Logger.Log($"Failed to open solution: {solutionFilePath}");
                return;
            }

            Logger.Log($"Solution loaded: {solutionFilePath}");

            if (!solution.Projects.Any())
            {
                Logger.Log("No projects found in the solution.");
                return;
            }

            var methodCodes = new Dictionary<string, string>();

            foreach (Project project in solution.Projects)
            {
                Logger.Log($"Project: {project.Name}");

                foreach (Document document in project.Documents)
                {
                    //Logger.Log($"  Document: {document.Name}");
                    SyntaxTree syntaxTree = await document.GetSyntaxTreeAsync();

                    if (syntaxTree == null)
                    {
                        Logger.Log($"Failed to get syntax tree for document: {document.Name}");
                        continue;
                    }

                    var semanticModel = await document.GetSemanticModelAsync();

                    if (semanticModel == null)
                    {
                        Logger.Log($"Failed to get semantic model for document: {document.Name}");
                        continue;
                    }

                    var methodDependencies = await GetClassMethodAndConstructorDependencies(solution, project, document, syntaxTree, semanticModel, request.ClassName, methodCodes , request);
                    
                    //Uncomment below code to print all methods in the solution
                    
                    //foreach (var dependency in methodDependencies)
                    //{
                    //    Logger.Log($"{dependency.CallerMethod} ---> {dependency.CalledMethod}");
                    //}
                }
            }
        }

        private static async Task<List<MethodDependency>> GetClassMethodAndConstructorDependencies(Solution solution, Project project, Document document, SyntaxTree syntaxTree, SemanticModel semanticModel, string targetClassName, Dictionary<string, string> methodCodes , BERequest request)
        {
            var methodDependencies = new List<MethodDependency>();

            var root = await syntaxTree.GetRootAsync() as CompilationUnitSyntax;

            if (root == null)
            {
                Logger.Log("Failed to get the root of the syntax tree.");
                return methodDependencies;
            }

            var targetClass = root.DescendantNodes()
                                  .OfType<ClassDeclarationSyntax>()
                                  .FirstOrDefault(c => c.Identifier.Text == targetClassName);

            if (targetClass == null)
            {
                return methodDependencies;
            }
            Logger.Log($"  Document: {document.Name}");
            var methodsAndConstructors = targetClass.DescendantNodes()
                .Where(node => node is MethodDeclarationSyntax || node is ConstructorDeclarationSyntax);

            var gptService = _serviceProvider.GetService<GPTService>();
            var codeResponses = new List<string>();
            string addComments = string.Empty;
            string fileName = string.Empty; 
            foreach (var member in methodsAndConstructors)
            {
                var methodSymbol = semanticModel.GetDeclaredSymbol(member) as IMethodSymbol;
                if (methodSymbol != null)
                {
                    var methodCode = member.ToFullString();
                    var methodSignature = methodSymbol.ToDisplayString();

                    if (!methodCodes.ContainsKey(methodSignature))
                    {
                        methodCodes.Add(methodSignature, methodCode);
                    }

                    await FindMethodDependenciesRecursive(solution, methodSymbol, methodDependencies, methodCodes);
                }
                string content = string.Join(Environment.NewLine, methodCodes.Values);
                fileName = targetClassName + "." + methodSymbol.MetadataName.ToString();
                Logger.Log($"Reading method {fileName}");
                //Uncomment this code to save complete method chain to a file.
                //await Helper.CreateFile(fileName, content);
                methodCodes.Clear();

                
                if (request.AddComments)
                {
                    addComments = Constants.AddComments; 
                }

                //Call AI here with the above content.   
                var multiProjectCodePrompt = $"<Methods-Chain>\n{content}\n</Methods-Chain> \n {Constants.AspxBackendPrompt}\n{addComments}";
                var multiProjectCodeResponse = await gptService.GetAiResponse(multiProjectCodePrompt, Constants.BackendSysPrompt, Constants.Model, true);
                codeResponses.Add(multiProjectCodeResponse.Message);
                
            }
            string allResponses = string.Join(Environment.NewLine, codeResponses);
            if (!string.IsNullOrEmpty(allResponses))
            {
                Logger.Log("Creating Project");
                var codePrompt = $"<Multiple-JSONS>\n{allResponses}\n</Multiple-JSONS> \n {Constants.AspxSingleProjectBackendPrompt}\n{addComments}";
                var codeResponse = await gptService.GetAiResponse(codePrompt, string.Empty, Constants.Model, true);

                if (request.MultipleProject)
                {
                    CreateMultipleProject(codeResponse.Message , fileName, request);
                    string outputPath = Path.Join(request.OutputPath, request.ClassName + "MultiProject");
                    CreateSolutionFile(request , outputPath);
                    return methodDependencies;
                }
                CreateSingleProject(codeResponse.Message , "APIService" , request);
                
            }
            return methodDependencies;
        }


        private static string CreateSingleProject(string response , string projectName, BERequest request)
        {
            string jsonArray = Helper.SelectJsonArray(response);
            List<FileTypeAndContent> rootObjects = JsonConvert.DeserializeObject<List<FileTypeAndContent>>(jsonArray)!;
            string bffServiceCode = null;
            string controllerCode = null;
            string dataServiceCode = null;
            string dataRepositoryCode = null;
            string dbContextCode = null;
            string dbSetCode = null;
            string ibffServiceCode = null;


            foreach (var fileContent in rootObjects)
            {
                switch (fileContent.FileName)
                {
                    case "BFFService":
                        bffServiceCode = fileContent.Content;
                        break;
                    case "Controller":
                        controllerCode = fileContent.Content;
                        break;
                    case "DataService":
                        dataServiceCode = fileContent.Content;
                        break;
                    case "DataRepository":
                        dataRepositoryCode = fileContent.Content;
                        break;
                    case "DbContext":
                        dbContextCode = fileContent.Content;
                        break;
                    case "DbSet":
                        dbSetCode = fileContent.Content;
                        break;
                    default:
                        Console.WriteLine($"Unexpected filename: {fileContent.FileName}");
                        break;
                }
            }
            string swagger =string.Empty;
            if (request.Swagger)
            {
                swagger = "--EnableSwaggerSupport \"true\"";
            }

            string commandFilePath = "./Backend-Templates/Scripts-Templates/create_api_ps.template";
            string template = File.ReadAllText(commandFilePath);
            template = template.Replace("$$BFFSERVICECODE$$", bffServiceCode)
                               .Replace("$$CONTROLLERCODE$$", controllerCode)
                               .Replace("$$DATASERVICECODE$$", dataServiceCode)
                               .Replace("$$DATAREPOSITORYCODE$$", dataRepositoryCode)
                               .Replace("$$DBCONTEXTCODE$$", dbContextCode)
                               .Replace("$$DBSETCODE$$", dbSetCode)
                               //.Replace("$$IBFFSERVICECODE$$", ibffServiceCode)
                               .Replace("$$FILENAME$$" , projectName)
                               .Replace("$$FRAMEWORK$$" , request.Framework)
                               .Replace("$$OUTPUTPATH$$" , request.OutputPath+"/"+request.ClassName)
                               .Replace("**ENABLESWAGGER**" , swagger);

            string tempScriptFilePath = Path.Combine(Path.GetTempPath(), "temp_script.ps1");
            File.WriteAllText(tempScriptFilePath, template);
            ExecuteScript(tempScriptFilePath, projectName, request);
            foreach (var fileContent in rootObjects)
            {
                if (fileContent.Type == "Interface")
                {
                    var interfaceCode = fileContent.Content;
                    var interfaceOutputPath = Path.Combine(request.OutputPath, request.ClassName, "Interfaces", $"{fileContent.FileName}.cs");
                    CreateInterface(request, interfaceCode, fileContent.FileName , interfaceOutputPath);
                }
            }
            string outputPath = Path.Join(request.OutputPath, request.ClassName);
            CreateSolutionFile(request, outputPath);
            return string.Empty;
        }


        
        private static string CreateInterface(BERequest request , string interfaceCode, string fileName , string outputPath)
        {
            string templatePath = "./Backend-Templates/Interface.template";
            string templateContent = File.ReadAllText(templatePath);
            string finalContent = templateContent.Replace("$$InterfaceCode$$", interfaceCode);
            File.WriteAllText(outputPath, finalContent);
            return string.Empty ;
        }

        private static Boolean CreateSolutionFile(BERequest request, string outputPath)
        {
            string commandFilePath = "./Backend-Templates/Scripts-Templates/create_solution_ps.template";
            string template = File.ReadAllText(commandFilePath);
            string tempScriptFilePath2 = Path.Combine(Path.GetTempPath(), "temp_script.ps1");
            template = template.Replace("$$OUTPUTPATH$$", outputPath)
                                .Replace("$$SOLUTIONNAME$$", request.ClassName);
            File.WriteAllText(tempScriptFilePath2, template);
            ExecuteScript(tempScriptFilePath2, "SLN_File", request);
            return true;
        }

        private static string CreateMultipleProject(string response, string fileName, BERequest request)
        {
            string jsonArray = Helper.SelectJsonArray(response);
            List<FileTypeAndContent> rootObjects = JsonConvert.DeserializeObject<List<FileTypeAndContent>>(jsonArray)!;
            foreach (var fileContent in rootObjects)
            {
                switch (fileContent.FileName)
                {
                    case "BFFService":
                        string bffServiceCode = fileContent.Content;
                        CreateAndExecuteScript(request, bffServiceCode, fileContent);
                        break;
                    case "Controller":
                        string controllerCode = fileContent.Content;
                        CreateAndExecuteScript(request, controllerCode, fileContent);
                        break;
                    case "DataService":
                        string dataServiceCode = fileContent.Content;
                        CreateAndExecuteScript(request, dataServiceCode, fileContent);
                        break;
                    case "DataRepository":
                        string dataRepositoryCode = fileContent.Content;
                        CreateAndExecuteScript(request, dataRepositoryCode, fileContent);
                        break;
                    case "DbContext":
                    case "DbSet":
                        string dbContextCode = fileContent.Content;
                        CreateDataRepoClasses(request, dbContextCode, fileContent);
                        break;
                    default:
                        Console.WriteLine($"Unexpected filename: {fileContent.FileName}");
                        break;
                }
            }

            string commandFilePath = "./Backend-Templates/Scripts-Templates/create_interface_ps.template";
            string template = File.ReadAllText(commandFilePath);
            string outputPath = Path.Join(request.OutputPath, request.ClassName+"MultiProject");
            template = template.Replace("$$FRAMEWORK$$", request.Framework)
                                .Replace("$$OUTPUTPATH$$" , outputPath);

            string tempScriptFilePath = Path.Combine(Path.GetTempPath(), "temp_script.ps1");
            File.WriteAllText(tempScriptFilePath, template);
            ExecuteScript(tempScriptFilePath, fileName, request);

            foreach (var fileContent in rootObjects)
            {
                if (fileContent.Type == "Interface")
                {
                    var interfaceCode = fileContent.Content;
                    var interfaceOutputPath = Path.Combine(request.OutputPath, request.ClassName+"MultiProject", "Interfaces", $"{fileContent.FileName}.cs");
                    CreateInterface(request, interfaceCode, fileContent.FileName, interfaceOutputPath);
                }
            }

            return string.Empty;
        }

        private static string CreateDataRepoClasses(BERequest request, string dataRepoCode, FileTypeAndContent fileContent)
        {
            string templatePath = "./Backend-Templates/DataRepoClasses.template";
            string outputPath = Path.Combine(request.OutputPath, request.ClassName+"MultiProject", "DataRepository", "DataRepository.Application", $"{fileContent.FileName}.cs");
            string templateContent = File.ReadAllText(templatePath);
            string finalContent = templateContent.Replace("$$DataRepoClassesCode$$", dataRepoCode);
            File.WriteAllText(outputPath, finalContent);
            return string.Empty;
        }

        public static string CreateAndExecuteScript(BERequest request, string serviceCode , FileTypeAndContent fileContent)
        {
            string fileName = fileContent.FileName;
            string swagger = string.Empty;
            string projectTemplate = "csodcustomtemplate";
            string excludeProject = $"--Exclude{fileName} \"true\"";
            if (fileContent.FileName == "Controller")
            {
                projectTemplate = "apicustomtemplate";
                excludeProject = string.Empty;  
                if (request.Swagger)
                {
                    swagger = "--EnableSwaggerSupport \"true\"";
                }
            }

            string commandFilePath = "./Backend-Templates/Scripts-Templates/create_classlibrary_ps.template";
            string template = File.ReadAllText(commandFilePath);
            template = template.Replace("$$SERVICECODE$$", serviceCode)
                               .Replace("$$FILENAME$$", fileName)
                               .Replace("$$FRAMEWORK$$", request.Framework)
                               .Replace("$$OUTPUTPATH$$", request.OutputPath + "/" + request.ClassName+"MultiProject")
                               .Replace("$$PROJECTTEMPLATE$$" , projectTemplate)
                               .Replace("**ENABLESWAGGER**", swagger)
                               .Replace("**EXCLUDEPROJECT**", excludeProject);

            string tempScriptFilePath = Path.Combine(Path.GetTempPath(), "temp_script.ps1");
            File.WriteAllText(tempScriptFilePath, template);
            ExecuteScript(tempScriptFilePath, fileName, request);

            return string.Empty;
        }

        private static string ExecuteScript(string scriptPath, string fileName, BERequest request)
        {
            string cmdArguments = $"/c powershell.exe -File \"{scriptPath}\"";
            ProcessStartInfo psi = new ProcessStartInfo("cmd.exe", cmdArguments)
            {
                RedirectStandardOutput = true,
                RedirectStandardError = true,
                UseShellExecute = false,
                CreateNoWindow = true,
            };

            using (Process process = Process.Start(psi))
            {
                if (process != null)
                {
                    process.WaitForExit();
                }
            }

            // Delete the temporary script file after execution
            File.Delete(scriptPath);
            Logger.Log($"Project with name {fileName} is created in path {request.OutputPath}");
            return string.Empty;
        }

        private static async Task FindMethodDependenciesRecursive(Solution solution, IMethodSymbol methodSymbol, List<MethodDependency> methodDependencies, Dictionary<string, string> methodCodes)
        {
            foreach (var project in solution.Projects)
            {
                foreach (var document in project.Documents)
                {
                    var syntaxTree = await document.GetSyntaxTreeAsync();
                    var semanticModel = await document.GetSemanticModelAsync();

                    var root = await syntaxTree.GetRootAsync() as CompilationUnitSyntax;

                    if (root == null)
                    {
                        continue;
                    }

                    var methodDeclaration = root.DescendantNodes()
                        .OfType<MethodDeclarationSyntax>()
                        .FirstOrDefault(m => semanticModel.GetDeclaredSymbol(m)?.Equals(methodSymbol) == true);

                    if (methodDeclaration != null)
                    {
                        var methodCode = methodDeclaration.ToFullString();
                        var methodSignature = methodSymbol.ToDisplayString();

                        if (!methodCodes.ContainsKey(methodSignature))
                        {
                            methodCodes.Add(methodSignature, methodCode);
                        }

                        var invocations = methodDeclaration.DescendantNodes().OfType<InvocationExpressionSyntax>();

                        foreach (var invocation in invocations)
                        {
                            var invokedMethodSymbol = semanticModel.GetSymbolInfo(invocation.Expression).Symbol as IMethodSymbol;

                            if (invokedMethodSymbol != null && IsCustomMethod(invokedMethodSymbol, solution))
                            {
                                var calledMethodName = invokedMethodSymbol.ToDisplayString();

                                methodDependencies.Add(new MethodDependency
                                {
                                    CallerMethod = methodSignature,
                                    CalledMethod = calledMethodName
                                });

                                if (!methodCodes.ContainsKey(calledMethodName))
                                {
                                    await FindMethodDependenciesRecursive(solution, invokedMethodSymbol, methodDependencies, methodCodes);
                                }
                            }
                        }
                        return;
                    }
                }
            }
        }

        private static bool IsCustomMethod(IMethodSymbol methodSymbol, Solution solution)
        {
            var containingAssembly = methodSymbol.ContainingAssembly;
            return solution.Projects.Any(p => p.AssemblyName == containingAssembly.Name);
        }

    }
}