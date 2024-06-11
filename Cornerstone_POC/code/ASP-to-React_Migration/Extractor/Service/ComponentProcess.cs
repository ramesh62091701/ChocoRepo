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
using System.Text.Json.Nodes;

namespace Extractor.Service
{
    public static class ComponentProcess
    {
        public static async Task<bool> Process(Request request)
        {
            var jsonPrompt = @"Read the Figma design image and give me the details of all the controls in json format like example data-grid, textarea, Date-picker etc.\nRules to follow while giving json output:
1.Always generate only json output do not give explanations above or below the json.
2.Table name should be one word without any spaces.
3.None of the name in type Breadcrumb should contain spaces.
4.Get all the buttons from analyzing the image and add it to below json.
5.Use the below json format as reference
[
    {
	  ""type"": ""Breadcrumb"",
	  ""paths"": [
                    { ""name"": ""Home"", ""data-type"": ""string"" },
                    { ""name"": ""AssignTraining"", ""data-type"": ""string"" },
                ]
	},
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
	  ""type"": ""DatePicker"",
	  ""initialValue"": {
		""start"": ""2024-06-04T00:00:00.000Z"",
		""end"": ""2024-06-12T00:00:00.000Z""
	},
    {
	  ""type"": ""TextArea"",
	  ""property-name"": ""Comments""
	},
    {
	  ""type"": ""Button"",
	  ""button-names"": [ ""Submit"", ""Search""],
	},

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
                    string type = jsonObject["type"].ToString();

                    switch (type)
                    {
                        case "DataGrid":
                            var gridTemplate = GenerateGrid(jsonObject);
                            //Logger.Log(gridTemplate.Content);
                            Helper.CreateFile(request.OutputPath, gridTemplate.FileName, gridTemplate.Content);
                            break;

                        case "TextArea":
                            var textAreaTemplate = GenerateTextArea(jsonObject);
                            //Logger.Log(textAreaTemplate.Content);
                            Helper.CreateFile(request.OutputPath, textAreaTemplate.FileName, textAreaTemplate.Content);
                            break;

                        case "DatePicker":
                            var datePickerTemplate = GenerateDatePicker(jsonObject);
                            //Logger.Log(datePickerTemplate.Content);
                            Helper.CreateFile(request.OutputPath, datePickerTemplate.FileName, datePickerTemplate.Content);
                            break;
                        case "Breadcrumb":
                            var breadcrumbTemplate = GenerateBreadcrumb(jsonObject);
                            //Logger.Log(breadcrumbTemplate.Content);
                            Helper.CreateFile(request.OutputPath, breadcrumbTemplate.FileName, breadcrumbTemplate.Content);
                            break;
                        case "Button":
                            var buttonTemplate = GenerateButtons(jsonObject);
                            //Logger.Log(buttonTemplate.Content);
                            Helper.CreateFile(request.OutputPath, buttonTemplate.FileName, buttonTemplate.Content);
                            break;

                        default:
                            Logger.Log($"Json object of type ={type} not found");  
                            break;
                    }
                }
            }
            return true;
        }

        private static (string Content, string FileName) GenerateGrid(JObject jsonObject)
        {
            string tableName = jsonObject["table-name"].ToString();

            JArray columnNamesArray = (JArray)jsonObject["column-names"];
            string[] columnNames = columnNamesArray.ToObject<string[]>();

            JArray rowsArray = (JArray)jsonObject["rows"];
            var rows = rowsArray.Select(row => row.ToObject<JObject>());

            string templateFilePath = "./Templates/Grid.template";
            string template = File.ReadAllText(templateFilePath);

            string columnsString = string.Join(",\n", columnNames.Select(columnName =>
                $"  {{ accessorKey: \"{columnName.ToLower()}\", header: \"{columnName}\" }}"));

            template = template.Replace("$$Entity$$", tableName)
                           .Replace("$$Columns$$", columnsString);

            return (Content: template , FileName: tableName + ".tsx");
        }

        private static (string Content, string FileName) GenerateTextArea(JObject jsonObject)
        {
            string propertyName = jsonObject["property-name"].ToString();

            string templateFilePath = "./Templates/Textarea.template";
            string template = File.ReadAllText(templateFilePath);

            template = template.Replace("$$PropertyName$$", propertyName);

            return (Content :template , FileName : propertyName+".tsx");
        }

        private static (string Content, string FileName) GenerateDatePicker(JObject jsonObject)
        {
            string type = jsonObject["type"].ToString();
            string templateFilePath = "./Templates/DatePicker.template";
            string template = File.ReadAllText(templateFilePath);

            return (Content: template, FileName: type + ".tsx");
        }

        private static (string Content, string FileName) GenerateBreadcrumb(JObject jsonObject)
        {
            string type = jsonObject["type"].ToString();

            JArray pathsArray = (JArray)jsonObject["paths"];

            var paths = pathsArray.Select(pathObject => new
            {
                Name = (string)pathObject["name"],
                DataType = (string)pathObject["data-type"]
            }).ToArray();

            string[] pathNames = paths.Select(path => path.Name).ToArray();

            string templateFilePath = "./Templates/Breadcrumb.template";
            string template = File.ReadAllText(templateFilePath);

            string parametersString = string.Join("\n", pathNames.Select(pathName =>
                $" {pathName.ToLower()},"));

            string parametersDeclarationString = string.Join(",\n", paths.Select(path =>
                $" {path.Name.ToLower()}: {path.DataType.ToLower()}"));


            template = template.Replace("$$ComponentName$$", type)
                               .Replace("$$Parameters$$", parametersString)
                               .Replace("$$ParametersAssignments$$", parametersDeclarationString);

            return (Content: template, FileName: type + "Container.tsx");
        }

        private static (string Content, string FileName) GenerateButtons(JObject jsonObject)
        {
            string type = jsonObject["type"].ToString();

            JArray buttonsArray = (JArray)jsonObject["button-names"];
            string[] buttonNames = buttonsArray.ToObject<string[]>();

            string templateFilePath = "./Templates/Button.template";
            string template = File.ReadAllText(templateFilePath);

            string buttonListString = string.Join(",\n", buttonNames.Select(buttonName =>
                $"//replace object with your localizations\n const localized{buttonName} = useLocalizationsDefaults(\r\n    `${{object.{buttonName}}}`\r\n  );"));

            template = template.Replace("$$ButtonList$$", buttonListString)
                .Replace("$$ComponentName$$", type);

            return (Content: template, FileName: type + "Container.tsx");
        }
    }
}
