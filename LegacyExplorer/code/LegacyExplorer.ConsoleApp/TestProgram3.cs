using LegacyExplorer.Processors;
using LegacyExplorer.Processors.Export;
using LegacyExplorer.Processors.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LegacyExplorer.ConsoleApp
{
    public class TestProgram3
    {
        public static void Test(string[] args)
        {
            List<string> listAssemnlyPath = new List<string>();
            listAssemnlyPath.Add("D:\\Downloads\\BlogEngine.NET-master\\BlogEngine.NET-master\\BlogEngine\\BlogEngine.NET\\bin\\BlogEngine.NET.dll");
            listAssemnlyPath.Add("D:\\Downloads\\BlogEngine.NET-master\\BlogEngine.NET-master\\BlogEngine\\BlogEngine.NET\\bin\\BlogEngine.Core.dll");

            AssemblyScanner scanner = new AssemblyScanner();

            Console.WriteLine($"Scanning assembly/s {string.Join("\n", listAssemnlyPath)}");
            var output = scanner.Scan(new ScannerInput { AssemblyPaths = listAssemnlyPath });

            // Export the Addresses collection to a separate CSV file
            var csvExporter = new CsvExporter("D:\\rnd\\output");

            Console.WriteLine($"Writing output to location {csvExporter.OutputDirectory}");

            csvExporter.ExportToCsv<NetAssembly>(output.Assemblies);
            csvExporter.ExportToCsv<NetReference>(output.References);
            csvExporter.ExportToCsv<NetType>(output.Types);
            csvExporter.ExportToCsv<NetField>(output.Fields);
            csvExporter.ExportToCsv<NetProperty>(output.Properties);
            csvExporter.ExportToCsv<NetMethod>(output.Methods);
            csvExporter.ExportToCsv<NetBaseClass>(output.BaseClasses);


            Console.WriteLine($"Export Complete...");

        }
    }
}
