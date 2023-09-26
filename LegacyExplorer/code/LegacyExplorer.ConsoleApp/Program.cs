using LegacyExplorer.Processors;
using LegacyExplorer.Processors.Interfaces;
using System;
using System.Collections.Generic;
using Mono.Cecil;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace LegacyExplorer.ConsoleApp
{
    internal class Program
    {
        static void Main(string[] args)
        {
            TestProgram1(args);

            //TestProgram3.Test(args);
            //TestProgram2(args);

            Console.ReadKey();
        }

        static void TestProgram2(string[] args)
        {
            string libPath = "LegacyExplorer.Processors.dll";
            List<string> lstLibPath = new List<string>();
            lstLibPath.Add(libPath);
            ILineCount<MethodInfo,int> ilineCount = new RefelectionLineCount();
            AssemblyScanner scanner = new AssemblyScanner(ilineCount);
            scanner.Scan(new ScannerInput{ AssemblyPaths = lstLibPath });

        }
        static void TestProgram1(string[] args)
        {
            Processors.FormsScanner scanner = new Processors.FormsScanner();

            string lib = "LegacyExplorer.Processors.dll";
            //string lib = "DCPSupply.Infrastructure.dll";
            string ex = "LegacyExplorer.UIApp.exe";
            string asmName = string.Empty;

            Console.WriteLine("Enter Source assembly type (l -> dll, e -> exe):");
            char asmType = (char)Console.Read();
            Console.WriteLine("\n");
            switch (asmType)
            {
                case 'l':
                    asmName = lib; break;
                case 'e':
                    asmName = ex; break;

                default:
                    Console.WriteLine("Unknown assembly Type");
                    break;
            }

            List<string> lstInfo = scanner.ScanForms(@"D:\Output\" + asmName);

            Console.WriteLine("Output\n");
            Console.WriteLine("======\n");
            foreach (string line in lstInfo)
            {
                Console.WriteLine(line);
            }
            Console.ReadKey();
        }
    }
}
