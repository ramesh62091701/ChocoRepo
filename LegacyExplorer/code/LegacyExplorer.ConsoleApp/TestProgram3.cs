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
            var persons = new List<Person>
            {
                new Person
                {
                    Name = "John Doe",
                    Age = 30,
                    Addresses = new List<Address>
                    {
                        new Address { Street = "123 Main St", City = "New York" },
                        new Address { Street = "456 Elm St", City = "Los Angeles" }
                    }
                },
                // Add more persons
            };

            string libPath = "LegacyExplorer.Processors.dll";
            TypeScanner scanner = new TypeScanner();
            var output = scanner.Scan(new ScannerInput { AssemblyPath = libPath });

            // Export the Addresses collection to a separate CSV file



            var asmCsvExporter = new CsvExporter<NetAssembly>();
            asmCsvExporter.ExportToCsv(output.Assemblies, "D:\\rnd\\output");

            var typeCsvExporter = new CsvExporter<NetType>();
            typeCsvExporter.ExportToCsv(output.Types, "D:\\rnd\\output");

            var fldCsvExporter = new CsvExporter<NetField>();
            fldCsvExporter.ExportToCsv(output.Fields, "D:\\rnd\\output");

            var methodCsvExporter = new CsvExporter<NetMethod>();
            methodCsvExporter.ExportToCsv(output.Methods, "D:\\rnd\\output"); 
            
            var refCsvExporter = new CsvExporter<NetReference>();
            refCsvExporter.ExportToCsv(output.References, "D:\\rnd\\output");

        }

        public static void Exporter(Type exportType, ScannerOutput output)

        {
            var csvExporter = new CsvExporter<NetAssembly>();
            csvExporter.ExportToCsv(output.Assemblies, "D:\\rnd\\output");

        }
    }
}
