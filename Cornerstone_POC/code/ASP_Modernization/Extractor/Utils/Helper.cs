using Extractor.Model;
using Extractor.Service;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using System.Text.RegularExpressions;


namespace Extractor.Utils
{

    internal static class Helper
    {
        public static void CreateFile(string destinationPath, string filename, string content)
        {
            if (!Directory.Exists(destinationPath))
            {
                Directory.CreateDirectory(destinationPath);
            }
            string filePath = Path.Combine(destinationPath, filename);
            File.WriteAllText(filePath, content);
            Logger.Log("File Created in path: " + filePath);
        }

        public static async Task CreateFile(string filename, string content)
        {
            string destinationPath = "D:\\Demo\\CSOD\\logs";
            filename = filename + ".txt";
            string filePath = Path.Combine(destinationPath, filename);
            await File.WriteAllTextAsync(filePath, content);
            Logger.Log("File created in path: " + filePath);
        }

        public static string RemoveMarkupCode(string content, string codeToRemove)
        {
            var stringToRemove = "```" + codeToRemove;
            if (content.StartsWith(stringToRemove))
            {
                content = content.Substring(stringToRemove.Length);
            }
            if (content.EndsWith("```"))
            {
                content = content.Substring(0, content.Length - 3);
            }
            return content;
        }


        public static string GetMethodDetails(string methodName, string filePath)
        {
            string code = File.ReadAllText(filePath);

            // Parse the code into a SyntaxTree
            SyntaxTree tree = CSharpSyntaxTree.ParseText(code);

            // Get the root node of the syntax tree
            CompilationUnitSyntax root = tree.GetCompilationUnitRoot();

            var method = FindMethod(root, methodName);

            if (method != null)
            {
                return method.ToFullString();
            }
            return string.Empty;
        }

        private static MethodDeclarationSyntax? FindMethod(SyntaxNode node, string methodName)
        {
            foreach (var descendant in node.DescendantNodes())
            {
                if (descendant is MethodDeclarationSyntax methodDeclaration &&
                    methodDeclaration.Identifier.ValueText.IndexOf(methodName, StringComparison.InvariantCultureIgnoreCase) > -1)
                {
                    return methodDeclaration;
                }
            }
            return null;
        }

        public static string SelectJsonArray(string jsonOutput)
        {
            string pattern = @"\[[\s\S]*\]";
            Match match = Regex.Match(jsonOutput, pattern);

            if (!match.Success)
            {
                Logger.Log("No JSON array found in the response.");
                return string.Empty;
            }
            return match.Value;
        }

    }
}
