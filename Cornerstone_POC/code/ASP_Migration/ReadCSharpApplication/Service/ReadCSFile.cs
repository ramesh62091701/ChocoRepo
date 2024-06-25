using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.CodeAnalysis.MSBuild;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ReadCSharpApplication.Models;

namespace ReadCSharpApplication.Service
{
    public static class ReadCSFile
    {
        public static async Task GetAllMethodsInClass(string targetClassName)
        {
            string solutionFilePath = @"C:\Users\m.abhishek.SONATA\Downloads\LMS\LMS\LMS.sln"; // Replace with your solution file path

            MSBuildWorkspace workspace = MSBuildWorkspace.Create();
            Solution solution = await workspace.OpenSolutionAsync(solutionFilePath);

            if (solution == null)
            {
                Console.WriteLine($"Failed to open solution: {solutionFilePath}");
                return;
            }

            Console.WriteLine($"Solution loaded: {solutionFilePath}");

            // Ensure projects are loaded correctly
            if (!solution.Projects.Any())
            {
                Console.WriteLine("No projects found in the solution.");
                return;
            }

            // Iterate through all projects in the solution
            foreach (Project project in solution.Projects)
            {
                // Process only the specified project
                if (project.Name != "LMS.Web")
                {
                    continue;
                }

                Console.WriteLine($"Project: {project.Name}");

                // Iterate through all documents in the project
                foreach (Document document in project.Documents)
                {
                    Console.WriteLine($"  Document: {document.Name}");

                    // Load the document and parse its syntax tree
                    SyntaxTree syntaxTree = await document.GetSyntaxTreeAsync();

                    if (syntaxTree == null)
                    {
                        Console.WriteLine($"Failed to get syntax tree for document: {document.Name}");
                        continue;
                    }

                    // Get semantic model to resolve symbols
                    var semanticModel = await document.GetSemanticModelAsync();

                    if (semanticModel == null)
                    {
                        Console.WriteLine($"Failed to get semantic model for document: {document.Name}");
                        continue;
                    }

                    // Get all class declarations in the document
                    var root = await syntaxTree.GetRootAsync() as CompilationUnitSyntax;
                    var classDeclarations = root.DescendantNodes().OfType<ClassDeclarationSyntax>();

                    foreach (var classDeclaration in classDeclarations)
                    {
                        // Check if the class name matches the target class name
                        if (classDeclaration.Identifier.Text == targetClassName)
                        {
                            // Find target class and its methods and constructors
                            var methodDependencies = await GetClassMethodAndConstructorDependencies(syntaxTree, semanticModel, classDeclaration.Identifier.Text);

                            // Print out method dependencies
                            foreach (var dependency in methodDependencies)
                            {
                                Console.WriteLine($"{dependency.CallerMethod} calls {dependency.CalledMethod}");
                            }
                        }
                    }
                }
            }
        }

        private static async Task<List<MethodDependency>> GetClassMethodAndConstructorDependencies(SyntaxTree syntaxTree, SemanticModel semanticModel, string targetClassName)
        {
            List<MethodDependency> methodDependencies = new List<MethodDependency>();

            // Parse the syntax tree
            var root = await syntaxTree.GetRootAsync() as CompilationUnitSyntax;

            if (root == null)
            {
                Console.WriteLine("Failed to get the root of the syntax tree.");
                return methodDependencies;
            }

            // Find the target class declaration
            var targetClass = root.DescendantNodes()
                                  .OfType<ClassDeclarationSyntax>()
                                  .FirstOrDefault(c => c.Identifier.Text == targetClassName);

            if (targetClass == null)
            {
                Console.WriteLine($"Class {targetClassName} not found.");
                return methodDependencies;
            }

            // Find all method and constructor declarations in the target class
            var methodsAndConstructors = targetClass.DescendantNodes()
                .Where(node => node is MethodDeclarationSyntax || node is ConstructorDeclarationSyntax);

            foreach (var member in methodsAndConstructors)
            {
                // Find all method invocations in the method or constructor body
                var invocations = member.DescendantNodes().OfType<InvocationExpressionSyntax>();

                foreach (var invocation in invocations)
                {
                    var methodSymbol = semanticModel.GetSymbolInfo(invocation.Expression).Symbol as IMethodSymbol;

                    if (methodSymbol != null)
                    {
                        string callerMethodName;
                        if (member is MethodDeclarationSyntax method)
                        {
                            callerMethodName = method.Identifier.ValueText;
                        }
                        else if (member is ConstructorDeclarationSyntax constructor)
                        {
                            callerMethodName = constructor.Identifier.ValueText;
                        }
                        else
                        {
                            continue;
                        }

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