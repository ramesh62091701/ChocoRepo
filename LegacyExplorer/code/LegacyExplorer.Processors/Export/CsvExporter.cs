using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Reflection;
using CsvHelper;
using CsvHelper.Configuration;
using CsvHelper.TypeConversion;

namespace LegacyExplorer.Processors.Export
{
    public class CsvExporter
    {
        public string ExportId = $"{DateTime.Now.ToString("yyyy-MM-dd-HHmmss")}";

        public string OutputDirectory { get; set; } 

        public CsvExporter(string outputDirectory)
        {
            ExportId = $"{DateTime.Now:yyyyMMddHHmmss}";
            OutputDirectory = outputDirectory;
        }

        public CsvExporter(string exportId, string outputDirectory)
        {
            ExportId = exportId;
            OutputDirectory = outputDirectory;
        }

        public void ExportToCsv<TCollection>(IEnumerable<TCollection> collection)
        {
            var directoryPath = Path.Combine(OutputDirectory, ExportId);
            Directory.CreateDirectory(directoryPath);

            var objectType = typeof(TCollection);


            var fileName = $"{objectType.Name}-{ExportId}.csv";

            using (var writer = new StreamWriter(Path.Combine(directoryPath, fileName)))
            using (var csv = new CsvWriter(writer, new CsvConfiguration(new CultureInfo("en-US"))))
            {
                csv.WriteRecords(collection);
            }

        }
    }
}
