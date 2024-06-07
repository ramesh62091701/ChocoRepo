using Newtonsoft.Json.Linq;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ASP_to_React_Migration.Utils;

namespace ASP_to_React_Migration
{
    public static class CreateDataGrid
    {
        public static void createGrid(string filePath)
        {
            string jsonFilePath = "data.json";
            string jsonData = File.ReadAllText(jsonFilePath);

            JObject jsonObject = JsonConvert.DeserializeObject<JObject>(jsonData);

            string tableName = jsonObject["table-name"].ToString();

            JArray columnNamesArray = (JArray)jsonObject["column-names"];
            string[] columnNames = columnNamesArray.ToObject<string[]>();

            JArray rowsArray = (JArray)jsonObject["rows"];
            var rows = rowsArray.Select(row => row.ToObject<JObject>());

            string templateFilePath = "./Files/Grid.txt";
            string template = File.ReadAllText(templateFilePath);

            string userGridTemplate = GenerateUserGrid(template, tableName, columnNames, rows);

            Helper.CreateFile(filePath, tableName + ".jsx", userGridTemplate);

            Console.WriteLine(userGridTemplate);
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
