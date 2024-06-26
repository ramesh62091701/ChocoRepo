using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.CodeAnalysis.MSBuild;
using ReadCSharpApplication.Models;
using ReadCSharpApplication.Utils;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace ReadCSharpApplication.Service
{
    public static class ReadCSFile2
    {
        public static async Task GetAllMethodsInClass(string targetClassName)
        {
            string solutionFilePath = @"C:\Users\m.abhishek.SONATA\source\repos\Aspx_Demo\Aspx_Demo.sln";

            MSBuildWorkspace workspace = MSBuildWorkspace.Create();
            Solution solution = await workspace.OpenSolutionAsync(solutionFilePath);

            if (solution == null)
            {
                Console.WriteLine($"Failed to open solution: {solutionFilePath}");
                return;
            }

            Console.WriteLine($"Solution loaded: {solutionFilePath}");

            if (!solution.Projects.Any())
            {
                Console.WriteLine("No projects found in the solution.");
                return;
            }

            var methodCodes = new Dictionary<string, string>();

            foreach (Project project in solution.Projects)
            {
                Console.WriteLine($"Project: {project.Name}");

                foreach (Document document in project.Documents)
                {
                    Console.WriteLine($"  Document: {document.Name}");

                    SyntaxTree syntaxTree = await document.GetSyntaxTreeAsync();

                    if (syntaxTree == null)
                    {
                        Console.WriteLine($"Failed to get syntax tree for document: {document.Name}");
                        continue;
                    }

                    var semanticModel = await document.GetSemanticModelAsync();

                    if (semanticModel == null)
                    {
                        Console.WriteLine($"Failed to get semantic model for document: {document.Name}");
                        continue;
                    }

                    var methodDependencies = await GetClassMethodAndConstructorDependencies(solution, project, document, syntaxTree, semanticModel, targetClassName, methodCodes);

                    foreach (var dependency in methodDependencies)
                    {
                        Console.WriteLine($"{dependency.CallerMethod} calls {dependency.CalledMethod}");
                    }
                }
            }
        }

        private static async Task<List<MethodDependency>> GetClassMethodAndConstructorDependencies(Solution solution, Project project, Document document, SyntaxTree syntaxTree, SemanticModel semanticModel, string targetClassName, Dictionary<string, string> methodCodes)
        {
            var methodDependencies = new List<MethodDependency>();

            var root = await syntaxTree.GetRootAsync() as CompilationUnitSyntax;

            if (root == null)
            {
                Console.WriteLine("Failed to get the root of the syntax tree.");
                return methodDependencies;
            }

            var targetClass = root.DescendantNodes()
                                  .OfType<ClassDeclarationSyntax>()
                                  .FirstOrDefault(c => c.Identifier.Text == targetClassName);

            if (targetClass == null)
            {
                return methodDependencies;
            }

            var methodsAndConstructors = targetClass.DescendantNodes()
                .Where(node => node is MethodDeclarationSyntax || node is ConstructorDeclarationSyntax);

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
                string fileName = targetClassName + "." + methodSymbol.MetadataName.ToString();
                await Helper.CreateFile(fileName, content);
                methodCodes.Clear();




            }

            return methodDependencies;
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













































//using Microsoft.CodeAnalysis;
//using Microsoft.CodeAnalysis.CSharp.Syntax;
//using Microsoft.CodeAnalysis.MSBuild;
//using ReadCSharpApplication.Models;
//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Threading.Tasks;

//namespace ReadCSharpApplication.Service
//{
//    public static class ReadCSFile2
//    {
//        public static async Task GetAllMethodsInClass(string targetClassName)
//        {
//            string solutionFilePath = @"C:\Users\m.abhishek.SONATA\source\repos\Aspx_Demo\Aspx_Demo.sln";

//            MSBuildWorkspace workspace = MSBuildWorkspace.Create();
//            Solution solution = await workspace.OpenSolutionAsync(solutionFilePath);

//            if (solution == null)
//            {
//                Console.WriteLine($"Failed to open solution: {solutionFilePath}");
//                return;
//            }

//            Console.WriteLine($"Solution loaded: {solutionFilePath}");

//            if (!solution.Projects.Any())
//            {
//                Console.WriteLine("No projects found in the solution.");
//                return;
//            }

//            var methodCodes = new Dictionary<string, string>();

//            // Iterate through all projects in the solution
//            foreach (Project project in solution.Projects)
//            {
//                Console.WriteLine($"Project: {project.Name}");

//                // Iterate through all documents in the project
//                foreach (Document document in project.Documents)
//                {
//                    Console.WriteLine($"  Document: {document.Name}");

//                    // Load the document and parse its syntax tree
//                    SyntaxTree syntaxTree = await document.GetSyntaxTreeAsync();

//                    if (syntaxTree == null)
//                    {
//                        Console.WriteLine($"Failed to get syntax tree for document: {document.Name}");
//                        continue;
//                    }

//                    // Get semantic model to resolve symbols
//                    var semanticModel = await document.GetSemanticModelAsync();

//                    if (semanticModel == null)
//                    {
//                        Console.WriteLine($"Failed to get semantic model for document: {document.Name}");
//                        continue;
//                    }

//                    // Find target class and its methods
//                    var methodDependencies = await GetClassMethodAndConstructorDependencies(solution, project, document, syntaxTree, semanticModel, targetClassName, methodCodes);

//                    // Print out method dependencies
//                    foreach (var dependency in methodDependencies)
//                    {
//                        Console.WriteLine($"{dependency.CallerMethod} calls {dependency.CalledMethod}");
//                    }
//                }
//            }
//            await File.WriteAllLinesAsync(@"D:\Demo\CSOD\logs\method.txt", methodCodes.Values);
//        }

//        private static async Task<List<MethodDependency>> GetClassMethodAndConstructorDependencies(Solution solution, Project project, Document document, SyntaxTree syntaxTree, SemanticModel semanticModel, string targetClassName, Dictionary<string, string> methodCodes)
//        {
//            List<MethodDependency> methodDependencies = new List<MethodDependency>();

//            // Parse the syntax tree
//            var root = await syntaxTree.GetRootAsync() as CompilationUnitSyntax;

//            if (root == null)
//            {
//                Console.WriteLine("Failed to get the root of the syntax tree.");
//                return methodDependencies;
//            }

//            // Find the target class declaration
//            var targetClass = root.DescendantNodes()
//                                  .OfType<ClassDeclarationSyntax>()
//                                  .FirstOrDefault(c => c.Identifier.Text == targetClassName);

//            if (targetClass == null)
//            {
//                return methodDependencies;
//            }

//            // Find all method and constructor declarations in the target class
//            var methodsAndConstructors = targetClass.DescendantNodes()
//                .Where(node => node is MethodDeclarationSyntax || node is ConstructorDeclarationSyntax);

//            foreach (var member in methodsAndConstructors)
//            {
//                var methodCode = member.ToFullString();
//                methodCodes.Add(methodCode);
//                await FindMethodDependenciesRecursive(solution, project, document, member, semanticModel, methodDependencies, methodCodes);
//            }

//            return methodDependencies;
//        }

//        private static async Task FindMethodDependenciesRecursive(Solution solution, Project project, Document document, SyntaxNode member, SemanticModel semanticModel, List<MethodDependency> methodDependencies, Dictionary<string, string> methodCodes)
//        {
//            // Find all method invocations in the method or constructor body
//            var invocations = member.DescendantNodes().OfType<InvocationExpressionSyntax>();

//            foreach (var invocation in invocations)
//            {
//                var methodSymbol = semanticModel.GetSymbolInfo(invocation.Expression).Symbol as IMethodSymbol;

//                if (methodSymbol != null)
//                {
//                    string callerMethodName;
//                    if (member is MethodDeclarationSyntax method)
//                    {
//                        callerMethodName = method.Identifier.ValueText;
//                    }
//                    else if (member is ConstructorDeclarationSyntax constructor)
//                    {
//                        callerMethodName = constructor.Identifier.ValueText;
//                    }
//                    else
//                    {
//                        continue;
//                    }

//                    var calledMethodName = methodSymbol.ToDisplayString();

//                    methodDependencies.Add(new MethodDependency
//                    {
//                        CallerMethod = callerMethodName,
//                        CalledMethod = calledMethodName
//                    });

//                    // Recursively find dependencies for the called method
//                    await FindCalledMethodDependencies(solution, methodSymbol, methodDependencies, methodCodes);
//                }
//            }
//        }

//        private static async Task FindCalledMethodDependencies(Solution solution, IMethodSymbol calledMethodSymbol, List<MethodDependency> methodDependencies, Dictionary<string, string> methodCodes)
//        {
//            foreach (var project in solution.Projects)
//            {
//                foreach (var document in project.Documents)
//                {
//                    var syntaxTree = await document.GetSyntaxTreeAsync();
//                    var semanticModel = await document.GetSemanticModelAsync();

//                    var root = await syntaxTree.GetRootAsync() as CompilationUnitSyntax;

//                    if (root == null)
//                    {
//                        continue;
//                    }

//                    var calledMethodDeclaration = root.DescendantNodes()
//                        .OfType<MethodDeclarationSyntax>()
//                        .FirstOrDefault(m => semanticModel.GetDeclaredSymbol(m)?.Equals(calledMethodSymbol) == true);

//                    if (calledMethodDeclaration != null)
//                    {
//                        await FindMethodDependenciesRecursive(solution, project, document, calledMethodDeclaration, semanticModel, methodDependencies, methodCodes);
//                        return;
//                    }
//                }
//            }
//        }
//    }
//}
