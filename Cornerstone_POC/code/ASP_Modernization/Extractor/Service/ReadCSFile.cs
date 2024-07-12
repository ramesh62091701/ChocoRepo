using Extractor.Model;
using Extractor.Utils;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.CodeAnalysis.MSBuild;
using Microsoft.Extensions.DependencyInjection;
using Newtonsoft.Json;
using System.Diagnostics;

namespace Extractor.Service
{
    public static class ReadCSFile
    {
        private static IServiceProvider _serviceProvider;
        static ReadCSFile()
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
                var codePrompt = $"<Multiple-JSONS>\n{allResponses}\n</Multiple-JSONS> \n {Constants.AspxSingleProjectBackendPrompt}\n{addComments}";
                var codeResponse = await gptService.GetAiResponse(codePrompt, string.Empty, Constants.Model, true);

                if (request.MultipleProject)
                {
                    CreateMultipleProject(codeResponse.Message , fileName, request);
                    string commandFilePath = "./createSolution.ps1";
                    string template = File.ReadAllText(commandFilePath);
                    string tempScriptFilePath = Path.Combine(Path.GetTempPath(), "temp_script.ps1");
                    template = template.Replace("$$OUTPUTPATH$$", request.OutputPath+"/"+request.ClassName)
                                        .Replace("$$SOLUTIONNAME$$" , request.ClassName);
                    File.WriteAllText(tempScriptFilePath, template);
                    ExecuteScript(tempScriptFilePath, "SLN_File", request);
                    return methodDependencies;
                }
                CreateSingleProject(codeResponse.Message , "APIService" , request);
                
            }
            return methodDependencies;
        }


        private static string CreateSingleProject(string response , string fileName, BERequest request)
        {
            string jsonArray = Helper.SelectJsonArray(response);
            List<FileContent> rootObjects = JsonConvert.DeserializeObject<List<FileContent>>(jsonArray)!;
            string bffServiceCode = null;
            string controllerCode = null;
            string dataServiceCode = null;
            string dataRepositoryCode = null;

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

            string commandFilePath = "./api-command.ps1";
            string template = File.ReadAllText(commandFilePath);
            template = template.Replace("$$BFFSERVICECODE$$", bffServiceCode)
                               .Replace("$$CONTROLLERCODE$$", controllerCode)
                               .Replace("$$DATASERVICECODE$$", dataServiceCode)
                               .Replace("$$DATAREPOSITORYCODE$$", dataRepositoryCode)
                               .Replace("$$FILENAME$$" , fileName)
                               .Replace("$$FRAMEWORK$$" , request.Framework)
                               .Replace("$$OUTPUTPATH$$" , request.OutputPath+"/"+request.ClassName)
                               .Replace("**ENABLESWAGGER**" , swagger);

            string tempScriptFilePath = Path.Combine(Path.GetTempPath(), "temp_script.ps1");
            File.WriteAllText(tempScriptFilePath, template);
            ExecuteScript(tempScriptFilePath, fileName, request);
            return string.Empty;
        }


        


        private static string CreateMultipleProject(string response, string fileName, BERequest request)
        {
            string jsonArray = Helper.SelectJsonArray(response);
            List<FileContent> rootObjects = JsonConvert.DeserializeObject<List<FileContent>>(jsonArray)!;
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
                    default:
                        Console.WriteLine($"Unexpected filename: {fileContent.FileName}");
                        break;
                }
            }
            
            return string.Empty;
        }

        public static string CreateAndExecuteScript(BERequest request, string serviceCode , FileContent fileContent)
        {
            string swagger = string.Empty;
            string projectTemplate = "csodcustomtemplate";
            if (fileContent.FileName == "Controller")
            {
                projectTemplate = "apicustomtemplate";

                if (request.Swagger)
                {
                    swagger = "--EnableSwaggerSupport \"true\"";
                }
            }

            string fileName = fileContent.FileName;
            string commandFilePath = "./command.ps1";
            string template = File.ReadAllText(commandFilePath);
            template = template.Replace("$$SERVICECODE$$", serviceCode)
                               .Replace("$$FILENAME$$", fileName)
                               .Replace("$$FRAMEWORK$$", request.Framework)
                               .Replace("$$OUTPUTPATH$$", request.OutputPath + "/" + request.ClassName)
                               .Replace("$$PROJECTTEMPLATE$$" , projectTemplate)
                               .Replace("**ENABLESWAGGER**", swagger);

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
                    string output = process.StandardOutput.ReadToEnd();
                    string error = process.StandardError.ReadToEnd();

                    Console.WriteLine($"Output: {output}");
                    Console.WriteLine($"Error: {error}");

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