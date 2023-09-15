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
    public class CsvExporter<T>
    {
        public void ExportToCsv(IEnumerable<T> data, string directoryPath)
        { 
                var objectType = typeof(T);
                var fileName = objectType.Name + ".csv"; // Use the name of the object as the filename

                using (var writer = new StreamWriter(Path.Combine(directoryPath, fileName)))
                using (var csv = new CsvWriter(writer, new CsvConfiguration(new CultureInfo("en-US"))))
                {
                    csv.WriteRecords(data);
                }

        }

        public void ExportCollectionToCsv<TCollection>(IEnumerable<TCollection> collection, string outputPath)
        {
            using (var writer = new StreamWriter(outputPath))
            using (var csv = new CsvWriter(writer, new CsvConfiguration(new CultureInfo("en-US"))))
            {
                csv.WriteRecords(collection);
            }
        }
    }
}
