using Newtonsoft.Json.Linq;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Extractor.Utils;
using Extractor.Model;
using System.Text.RegularExpressions;

namespace Extractor.Service
{
    public static class DataGridProcessor
    {
        public static async Task<bool> Process(Request request)
        {
            var jsonPrompt = @"Read the Figma design image and give me the details of all the controls in json format like example data-grid, textarea, Date-picker etc.\nRules to follow while giving json output:
1.Always generate only json output do not give explanations above or below the json.
2.Table name should be one word without any spaces.
3.Use the below format
[
	{
	  ""type"": ""DataGrid"",
	  ""table-name"": ""UserDetails"",
	  ""total-rows"" :  5,
	  ""column-names"": [ ""Name"", ""ID"", ""Division"", ],
	  ""rows"": [
		{
		  ""Name"": ""John Doe"",
		  ""ID"": ""123"",
		  ""Division"": ""Sales"",
		},
		{
		  ""Name"": ""Jane Smith"",
		  ""ID"": ""456"",
		  ""Division"": ""Marketing"",
		  ""Position"": ""Executive"",
		},
	  ],
	  ""pages"": ""1 of 6""
	},
	{
	  ""type"": ""DateRangePicker"",
	  ""initialValue"": {
		""start"": ""2024-06-04T00:00:00.000Z"",
		""end"": ""2024-06-12T00:00:00.000Z""
	  }
	}
]
";
            var gptService = new GPTService();
            var jsonOutput = await gptService.GetAiResponseForImage(jsonPrompt, string.Empty, Constants.Model, true, request.ImagePath);

            string pattern = @"\[[\s\S]*\]";
            Match match = Regex.Match(jsonOutput.Message, pattern);

            if (!match.Success)
            {
                Logger.Log("No JSON array found in the response.");
                return false;
            }

            string arrayJson = match.Value;

            JArray jsonArray = JArray.Parse(arrayJson);

            foreach (JToken token in jsonArray)
            {
                if (token is JObject)
                {
                    JObject jsonObject = (JObject)token;
                    if (jsonObject["type"].ToString() == "DataGrid")
                    {
                        string tableName = jsonObject["table-name"].ToString();

                        JArray columnNamesArray = (JArray)jsonObject["column-names"];
                        string[] columnNames = columnNamesArray.ToObject<string[]>();

                        JArray rowsArray = (JArray)jsonObject["rows"];
                        var rows = rowsArray.Select(row => row.ToObject<JObject>());

                        string templateFilePath = "./Files/Grid.txt";
                        string template = File.ReadAllText(templateFilePath);

                        string userGridTemplate = GenerateUserGrid(template, tableName, columnNames, rows);

                        Logger.Log(userGridTemplate);

                        Helper.CreateFile(request.OutputPath, tableName + ".jsx", userGridTemplate);
                        
                    }
                }
            }
            return true;
        }

        public static string GenerateUserGrid(string template, string tableName, string[] columnNames, IEnumerable<JObject> rows)
        {
            string columnsString = string.Join(",\n", columnNames.Select(columnName =>
                $"  {{ accessorKey: \"{columnName.ToLower()}\", header: \"{columnName}\" }}"));

            string rowsString = string.Join(",\n", rows.Select(row =>
            {
                var rowProperties = columnNames.Select(columnName =>
                    $"\"{columnName.ToLower()}\": \"{row[columnName]}\"");
                return $"  {{ {string.Join(", ", rowProperties)} }}";
            }));

            template = template.Replace("{{table-name}}", tableName)
                           .Replace("{{columns}}", columnsString)
                           .Replace("{{rows}}", rowsString);

            return template;
        }
    }
}
