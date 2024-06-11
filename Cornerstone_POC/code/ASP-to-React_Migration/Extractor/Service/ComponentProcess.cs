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
using System.Net.NetworkInformation;
using Microsoft.VisualBasic;

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
            { ""name"": ""AssignTraining"", ""data-type"": ""string"" }
        ]
    },
    {
        ""type"": ""DataGrid"",
        ""table-name"": ""UserDetails"",
        ""total-rows"": 5,
        ""column-names"": [""Name"", ""ID"", ""Division""],
        ""rows"": [
            {
                ""Name"": ""John Doe"",
                ""ID"": ""123"",
                ""Division"": ""Sales""
            },
            {
                ""Name"": ""Jane Smith"",
                ""ID"": ""456"",
                ""Division"": ""Marketing"",
                ""Position"": ""Executive""
            }
        ],
        ""pages"": ""1 of 6""
    },
    {
        ""type"": ""DatePicker"",
        ""initialValue"": {
            ""start"": ""2024-06-04T00:00:00.000Z"",
            ""end"": ""2024-06-12T00:00:00.000Z""
        }
    },
    {
        ""type"": ""TextArea"",
        ""property-name"": ""Comments""
    },
    {
        ""type"": ""Button"",
        ""button-names"": [""Submit"", ""Search""]
    }
]

";
            var gptService = new GPTService();
            var jsonOutput = await gptService.GetAiResponseForImage(jsonPrompt, string.Empty, Model.Constants.Model, true, request.ImagePath);

            string pattern = @"\[[\s\S]*\]";
            Match match = Regex.Match(jsonOutput.Message, pattern);

            if (!match.Success)
            {
                Logger.Log("No JSON array found in the response.");
                return false;
            }

            string arrayJson = match.Value;

            //Send the componenets to the popup screen

            List<ComponentJson> jsonArray = JsonConvert.DeserializeObject<List<ComponentJson>>(arrayJson);

            foreach (var component in jsonArray)
            {
                switch (component.Type)
                {
                    case "DataGrid":
                        if (component.Rows != null)
                        {
                            var gridTemplate = GenerateGrid(new DataGrid
                            {
                                Type = component.Type,
                                TableName = component.TableName,
                                TotalRows = component.TotalRows ?? 0,
                                ColumnNames = component.ColumnNames,
                                Rows = component.Rows,
                                Pages = component.Pages
                            });
                            Helper.CreateFile(request.OutputPath, gridTemplate.FileName, gridTemplate.Content);
                        }
                        break;

                    case "TextArea":
                        if (component.PropertyName != null)
                        {
                            var textAreaTemplate = GenerateTextArea(new TextArea
                            {
                                Type = component.Type,
                                PropertyName = component.PropertyName
                            });
                            Helper.CreateFile(request.OutputPath, textAreaTemplate.FileName, textAreaTemplate.Content);
                        }
                        break;

                    case "DatePicker":
                        if (component.InitialValue != null)
                        {
                            var datePickerTemplate = GenerateDatePicker(new DatePicker
                            {
                                Type = component.Type,
                                InitialValue = component.InitialValue
                            });
                            Helper.CreateFile(request.OutputPath, datePickerTemplate.FileName, datePickerTemplate.Content);
                        }
                        break;

                    case "Breadcrumb":
                        if (component.Paths != null)
                        {
                            var breadcrumbTemplate = GenerateBreadcrumb(new Breadcrumb
                            {
                                Type = component.Type,
                                Paths = component.Paths
                            });
                            Helper.CreateFile(request.OutputPath, breadcrumbTemplate.FileName, breadcrumbTemplate.Content);
                        }
                        break;

                    case "Button":
                        if (component.ButtonNames != null)
                        {
                            var buttonTemplate = GenerateButtons(new Button
                            {
                                Type = component.Type,
                                ButtonNames = component.ButtonNames
                            });
                            Helper.CreateFile(request.OutputPath, buttonTemplate.FileName, buttonTemplate.Content);
                        }
                        break;

                    default:
                        Logger.Log($"Json object of type ={component.Type} not found");
                        break;
                }
            }        
            return true;
        }

        private static (string Content, string FileName) GenerateGrid(DataGrid dataGrid)
        {
            string tableName = dataGrid.TableName;

            string[] columnNames = dataGrid.ColumnNames.ToArray();

            string templateFilePath = "./Templates/Grid.template";
            string template = File.ReadAllText(templateFilePath);

            string columnsString = string.Join(",\n", columnNames.Select(columnName =>
                $"  {{ accessorKey: \"{columnName.ToLower()}\", header: \"{columnName}\" }}"));

            template = template.Replace("$$Entity$$", tableName)
                           .Replace("$$Columns$$", columnsString);

            return (Content: template , FileName: tableName + ".tsx");
        }

        private static (string Content, string FileName) GenerateTextArea(TextArea textArea)
        {
            string propertyName = textArea.PropertyName;

            string templateFilePath = "./Templates/Textarea.template";
            string template = File.ReadAllText(templateFilePath);

            template = template.Replace("$$PropertyName$$", propertyName);

            return (Content :template , FileName : propertyName+".tsx");
        }

        private static (string Content, string FileName) GenerateDatePicker(DatePicker datePicker)
        {
            string type = datePicker.Type;
            string templateFilePath = "./Templates/DatePicker.template";
            string template = File.ReadAllText(templateFilePath);

            return (Content: template, FileName: type + ".tsx");
        }

        private static (string Content, string FileName) GenerateBreadcrumb(Breadcrumb breadcrumb)
        {
            string type = breadcrumb.Type;

            var pathsArray = breadcrumb.Paths.ToArray(); 

            var paths = pathsArray.Select(pathObject => new
            {
                Name = pathObject.Name,
                DataType = pathObject.DataType,
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

        private static (string Content, string FileName) GenerateButtons(Button button)
        {
            string type = button.Type;

            var buttonsArray = button.ButtonNames.ToArray();

            string templateFilePath = "./Templates/Button.template";
            string template = File.ReadAllText(templateFilePath);

            string buttonListString = string.Join(",\n", buttonsArray.Select(buttonName =>
                $"//replace object with your localizations\n const localized{buttonName} = useLocalizationsDefaults(\r\n    `${{object.{buttonName}}}`\r\n  );"));

            template = template.Replace("$$ButtonList$$", buttonListString)
                .Replace("$$ComponentName$$", type);

            return (Content: template, FileName: type + "Container.tsx");
        }

        public static async Task<string> GenerateMainFile(Request request)
        {
            var allComponentContent = ReadFiles(request.OutputPath);
            var gptService = new GPTService();
            var prompt = $@"{allComponentContent}
Above are the component files in the React. Generate a json response in below format for the code of React component
1.Always generate only json output do not give explanations above or below the json.
2.type should always be a single word.
3.Use the below json format as reference:
[
  {{
    ""type"": ""BreadcrumbContainer"",
    ""declared-variable"": [""home"", ""assigntraining""]
  }},
  {{
    ""type"": ""ButtonContainer"",
    ""declared-variable"": [""currentPage"", ""isDisabled""]
  }},
]";
            var response = await gptService.GetAiResponse(prompt, String.Empty, Model.Constants.Model, true);

            string pattern = @"\[[\s\S]*\]";
            Match match = Regex.Match(response.Message, pattern);

            if (!match.Success)
            {
                Logger.Log("No JSON array found in the response.");
            }

            string arrayJson = match.Value;
            List<FigmaComponentJson> jsonArray = JsonConvert.DeserializeObject<List<FigmaComponentJson>>(arrayJson);

            StringBuilder componentBuilder = new StringBuilder();
            foreach (var obj in jsonArray)
            {
                string type = obj.Type;
                var declaredVariables = obj.DeclaredVariable.ToArray();

                componentBuilder.AppendLine($"<{type}");

                string variablesString = string.Join("\n", declaredVariables.Select(declaredVariable =>
                    $"{declaredVariable.ToLower()}={{{declaredVariable}}}"));

                componentBuilder.AppendLine(variablesString);
                componentBuilder.AppendLine("/>");
            }
            string templateFilePath = "./Templates/App.template";
            string template = File.ReadAllText(templateFilePath);
            string componentString = componentBuilder.ToString();
            template = template.Replace("$$Components$$", componentString);

            return template;
        }

        
        
        
        
        private static string ReadFiles(string directoryPath)
        {
            string allFileContent = string.Empty;
            try
            {
                foreach (string filePath in Directory.GetFiles(directoryPath))
                {
                    string fileContent = File.ReadAllText(filePath);
                    string fileName = Path.GetFileName(filePath);
                    allFileContent += $"<{fileName}>\n";
                    allFileContent += fileContent + "\n";
                    allFileContent += $"</{fileName}>\n\n";
                }

                foreach (string subDir in Directory.GetDirectories(directoryPath))
                {
                    allFileContent += ReadFiles(subDir);
                }
            }
            catch (Exception ex)
            {
                Logger.Log($"An error occurred: {ex.Message}");
            }
            return allFileContent;
        }
    }
}
