using Newtonsoft.Json;
using SAPBOAnalysis.Models;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SAPBOAnalysis
{
    public class Generator
    {
        private Configuration config;
        public Generator()
        {
            config = JsonConvert.DeserializeObject<Configuration>(File.ReadAllText("appconfig.json"))!;
        }
        public async Task<string> Generate(AnalysisSettings settings)
        {
            var worksheets = new Dictionary<string, List<object>>();

            if (settings.universes)
            {
                var analyzer = new UniverseAnalyzer();
                var universeWorksheets = await analyzer.Analyze();
                Add(worksheets, universeWorksheets);
            }

            if (settings.documents)
            {
                var reportAnalyzer = new ReportAnalyzer();
                var reportWorkSheets = await reportAnalyzer.Analyze(settings.reports);
                Add(worksheets, reportWorkSheets);
            }

            if (settings.connections)
            {
                var connectionAnalyzer = new ConnectionAnalyzer();
                var connectionWorkSheets = await connectionAnalyzer.Analyze();
                Add(worksheets, connectionWorkSheets);
            }

            Logger.Log("Generating Report...");
            var fileName = $"SAPBOAnalysis_{DateTime.Now:yyyyMMdd_HHmmss}";
            CsvHelper.WriteExcelFile($"{config.csvFilePath}//{fileName}.xlsx", worksheets);
            Logger.Log($"Report available in location {config.csvFilePath}//{fileName}.xlsx");
            return $"{config.csvFilePath}//{fileName}.xlsx";
        }

        private Dictionary<string, List<object>> Add(Dictionary<string, List<object>> dic, Dictionary<string, List<object>> dicToAdd)
        {
            foreach (var key in dicToAdd.Keys)
            {
                dic.Add(key, dicToAdd[key]);
            }
            return dic;
        }

        public List<Item> GetModes()
        {
            var modes = new List<Item>();
            modes.Add(new Item { Id = "Push", Name = "Push" });
            modes.Add(new Item { Id = "Import", Name = "Import" });
            modes.Add(new Item { Id = "Directquery", Name = "DirectQuery" });
            return modes;
        }
    }
}
