using LegacyExplorer.Processors;
using LegacyExplorer.Processors.Export;
using LegacyExplorer.Processors.Models;
using LegacyExplorer.Processors.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Reflection;
using System.Reflection.Emit;
using System.Runtime.CompilerServices;
using System.Configuration;
using System.Collections.Specialized;

namespace LegacyExplorer.ConsoleApp
{
    public class TestProgram3
    {
        public static void Test(string[] args)
        {

            NameValueCollection appSettings = ConfigurationManager.AppSettings;
            string isOutputSingleFile  = appSettings["OutputSingleFile"];

            List<string> listAssemnlyPath = new List<string>();
            listAssemnlyPath.Add("D:\\Downloads\\BlogEngine.NET-master\\BlogEngine.NET-master\\BlogEngine\\BlogEngine.NET\\bin\\BlogEngine.NET.dll");
            listAssemnlyPath.Add("D:\\Downloads\\BlogEngine.NET-master\\BlogEngine.NET-master\\BlogEngine\\BlogEngine.NET\\bin\\BlogEngine.Core.dll");
            listAssemnlyPath.Add("LegacyExplorer.Processors.dll");

            ILineCount<MethodInfo, int> iLineCount = new RefelectionLineCount();

            AssemblyScanner scanner = new AssemblyScanner(iLineCount);

            Console.WriteLine($"Scanning assembly/s {string.Join("\n", listAssemnlyPath)}");
            var output = scanner.Scan(new ScannerInput { AssemblyPaths = listAssemnlyPath });

            // Export the Addresses collection to a separate CSV file
            var csvExporter = new CsvExporter("D:\\rnd\\output");

            Console.WriteLine($"Writing output to location {csvExporter.OutputDirectory}");
            if (isOutputSingleFile == "true")
            {
                List<dynamic> dynamics = new List<dynamic>();
                dynamics.Add(output.Assemblies);
                dynamics.Add(output.References);
                dynamics.Add(output.Types);
                dynamics.Add(output.Fields);
                dynamics.Add(output.Properties);
                dynamics.Add(output.Methods);
                dynamics.Add(output.BaseClasses);


                csvExporter.ExportToCsv(dynamics, true);
            }
            else
            {
                csvExporter.ExportToCsv<NetAssembly>(output.Assemblies);
                csvExporter.ExportToCsv<NetReference>(output.References);
                csvExporter.ExportToCsv<NetType>(output.Types);
                csvExporter.ExportToCsv<NetField>(output.Fields);
                csvExporter.ExportToCsv<NetProperty>(output.Properties);
                csvExporter.ExportToCsv<NetMethod>(output.Methods);
                csvExporter.ExportToCsv<NetBaseClass>(output.BaseClasses);
            }

            Console.WriteLine($"Export Complete...");

        }
    }
}
