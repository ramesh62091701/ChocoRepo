using Extractor.Service;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Security.AccessControl;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace Extractor.Utils
{

    internal static class Helper
    {

        public const string IGNORE_EMBEDDING = "ignoreEmbedding";
        public static string GetId(string partitionKey, string rowKey)
        {
            return $"{partitionKey}_{rowKey}";
        }

        public static Tuple<string, string> GetKeys(string id)
        {
            var keys = id.Split('_');
            return new Tuple<string, string>(keys[0], keys[1]);
        }

        public static void CreateFile(string destinationPath, string filename, string content)
        {
            string filePath = Path.Combine(destinationPath, filename);
            File.WriteAllText(filePath, content);
            Logger.Log("File Created in path: " + filePath);
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

    }
}
