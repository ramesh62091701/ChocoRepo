using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.CodeAnalysis.MSBuild;
using Microsoft.CodeAnalysis;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ReadCSharpApplication.Models;

namespace ReadCSharpApplication.Service
{
    public static class GetMethodsClass
    {
        public static async Task getAllMethods()
        {
            string solutionFilePath = @"D:\CSOD\AspxTesting\AspxTesting.sln"; // Replace with your solution file path

            MSBuildWorkspace workspace = MSBuildWorkspace.Create();
            Solution solution = await workspace.OpenSolutionAsync(solutionFilePath);

            if (solution == null)
            {
                Console.WriteLine($"Failed to open solution: {solutionFilePath}");
                return;
            }

            Console.WriteLine($"Solution loaded: {solutionFilePath}");

            // Iterate through all projects in the solution
            foreach (Project project in solution.Projects)
            {
                Console.WriteLine($"Project: {project.Name}");

                // Iterate through all documents in the project
                foreach (Microsoft.CodeAnalysis.Document document in project.Documents)
                {
                    Console.WriteLine($"  Document: {document.Name}");

                    // Load the document and parse its syntax tree
                    SyntaxTree syntaxTree = await document.GetSyntaxTreeAsync();

                    // Get semantic model to resolve symbols
                    var semanticModel = await document.GetSemanticModelAsync();

                    // Find method invocations and their relationships
                    var methodDependencies = await GetMethodDependencies(syntaxTree, semanticModel);

                    // Print out method dependencies
                    foreach (var dependency in methodDependencies)
                    {
                        Console.WriteLine($"{dependency.CallerMethod} calls {dependency.CalledMethod}");
                    }
                }
            }
        }

        private static async Task<List<MethodDependency>> GetMethodDependencies(SyntaxTree syntaxTree, SemanticModel semanticModel)
        {
            List<MethodDependency> methodDependencies = new List<MethodDependency>();

            // Parse the syntax tree
            var root = await syntaxTree.GetRootAsync() as CompilationUnitSyntax;

            // Find all method declarations in the syntax tree
            var methods = root.DescendantNodes().OfType<MethodDeclarationSyntax>();

            foreach (var method in methods)
            {
                // Find all method invocations in the method body
                var invocations = method.DescendantNodes().OfType<InvocationExpressionSyntax>();

                foreach (var invocation in invocations)
                {
                    var methodSymbol = semanticModel.GetSymbolInfo(invocation.Expression).Symbol as IMethodSymbol;

                    if (methodSymbol != null)
                    {
                        var callerMethodName = method.Identifier.ValueText;
                        var calledMethodName = methodSymbol.Name;

                        methodDependencies.Add(new MethodDependency
                        {
                            CallerMethod = callerMethodName,
                            CalledMethod = calledMethodName
                        });
                    }
                }
            }

            return methodDependencies;
        }
    }
}

