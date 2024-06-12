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
using System.Globalization;

namespace Extractor.Service
{
    public static class ComponentProcess
    {

        public static async Task<List<FigmaComponent>> GetFigmaControls(Request request)
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
            ""name"": ""UserDetails"",
            ""columnNames"": [""Name"", ""ID"", ""Division""],
        },
        {
            ""type"": ""DataGrid"",
            ""name"": ""ClientDetails"",
            ""columnNames"": [""Name"", ""ID"", ""Division""],
        },
        {
            ""type"": ""DatePicker"",
            ""label"": ""Date"",
        },
        {
            ""type"": ""TextArea"",
            ""label"": ""Comments""
        },
        {
            ""type"": ""Button"",
            ""name"": ""Submit"",
        },
        {
            ""type"": ""Button"",
            ""name"": ""Cancel"",
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
                return new List<FigmaComponent>();
            }
            string arrayJson = match.Value;
            List<FigmaComponent> components = JsonConvert.DeserializeObject<List<FigmaComponent>>(arrayJson);
            return components;
        }
        public static async Task<bool> Process(Request request)
        {
            List<FigmaComponent> buttons = new List<FigmaComponent>();
            foreach (var component in request.ControlResponse.FigmaComponents)
            {
                switch (component.Type)
                {
                    case "DataGrid":
                        if (component.Type != null)
                        {
                            var gridTemplate = GenerateGrid(new FigmaComponent
                            {
                                Type = component.Type,
                                Name = component.Name,
                                ColumnNames = component.ColumnNames,

                            }, request);
                            Helper.CreateFile(request.OutputPath, gridTemplate.FileName, gridTemplate.Content);
                        }
                        break;

                    case "TextArea":
                        if (!string.IsNullOrEmpty(component.Label))
                        {
                            var textAreaTemplate = GenerateTextArea(new FigmaComponent
                            {
                                Type = component.Type,
                                Name = component.Label
                            });
                            Helper.CreateFile(request.OutputPath, textAreaTemplate.FileName, textAreaTemplate.Content);
                        }
                        break;

                    case "DatePicker":
                        if (!string.IsNullOrEmpty(component.Label))
                        {
                            var datePickerTemplate = GenerateDatePicker(new FigmaComponent
                            {
                                Type = component.Type,
                                Label = component.Label
                            });
                            Helper.CreateFile(request.OutputPath, datePickerTemplate.FileName, datePickerTemplate.Content);
                        }
                        break;

                    case "Breadcrumb":
                        if (component.Paths != null)
                        {
                            var breadcrumbTemplate = GenerateBreadcrumb(new FigmaComponent
                            {
                                Type = component.Type,
                                Paths = component.Paths
                            });
                            Helper.CreateFile(request.OutputPath, breadcrumbTemplate.FileName, breadcrumbTemplate.Content);
                        }
                        break;

                    case "Button":
                        buttons.Add(component);
                        break;

                    default:
                        Logger.Log($"Json object of type ={component.Type} not found");
                        break;
                }
            }
            if (buttons.Any())
            {
                var buttonTemplate = GenerateButtons(buttons);
                Helper.CreateFile(request.OutputPath, buttonTemplate.FileName, buttonTemplate.Content);
            }
            return true;
        }

        private static (string Content, string FileName) GenerateGrid(FigmaComponent dataGrid, Request request)
        {
            string tableName = dataGrid.Name;

            string[] columnNames = dataGrid.ColumnNames.ToArray();

            string templateFilePath = "./Templates/Grid.template";
            string template = File.ReadAllText(templateFilePath);

            string columnsString = string.Join(",\n", columnNames.Select(columnName =>
                $"  {{ accessorKey: \"{columnName.ToLower()}\", header: \"{columnName}\" }}"));

            template = template.Replace("$$Entity$$", tableName)
                           .Replace("$$Columns$$", columnsString);

            var mappedControl = request.Mapping.FirstOrDefault(x => x.FigmaComponent.Type == "DataGrid" && x.FigmaComponent.Name == tableName);

            if (mappedControl != null && File.Exists(request.AspxPagePath + ".cs"))
            {
                var fetchDetails = Helper.GetMethodDetails(mappedControl.AspComponent.id, request.AspxPagePath + ".cs");
                template = template.Replace("$$FetchDetails$$", $"/*{Environment.NewLine}{fetchDetails}{Environment.NewLine}*/");
            }

            return (Content: template , FileName: tableName + ".tsx");
        }

        private static (string Content, string FileName) GenerateTextArea(FigmaComponent textArea)
        {
            string propertyName = textArea.Name;

            string templateFilePath = "./Templates/Textarea.template";
            string template = File.ReadAllText(templateFilePath);

            template = template.Replace("$$PropertyName$$", propertyName);

            return (Content :template , FileName : propertyName+".tsx");
        }

        private static (string Content, string FileName) GenerateDatePicker(FigmaComponent datePicker)
        {
            string type = datePicker.Type;
            string templateFilePath = "./Templates/DatePicker.template";
            string template = File.ReadAllText(templateFilePath);

            template = template.Replace("$$PropertyName$$", type);
            return (Content: template, FileName: type + ".tsx");
        }

        private static (string Content, string FileName) GenerateBreadcrumb(FigmaComponent breadcrumb)
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

        private static (string Content, string FileName) GenerateButtons(List<FigmaComponent> buttons)
        {
            string type = buttons.First().Type;

            var buttonNames = buttons.Select(b => b.Name).ToList();

            string templateFilePath = "./Templates/Button.template";
            string template = File.ReadAllText(templateFilePath);

            string buttonListString = string.Join(",\n", buttonNames.Select(buttonName =>
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
            StringBuilder importComponent = new StringBuilder();
            foreach (var obj in jsonArray)
            {
                string type = obj.Type;
                var declaredVariables = obj.DeclaredVariable.ToArray();

                componentBuilder.AppendLine($"<{type}");

                string variablesString = string.Join("\n", declaredVariables.Select(declaredVariable =>
                    $"{declaredVariable.ToLower()}={{{declaredVariable}}}"));

                componentBuilder.AppendLine(variablesString);
                componentBuilder.AppendLine("/>");
                importComponent.AppendLine($"import {{ {type} }} from \"components/{type}\"; ");
            }
            string templateFilePath = "./Templates/App.template";
            string template = File.ReadAllText(templateFilePath);
            template = template.Replace("$$Components$$", componentBuilder.ToString())
                .Replace("$$ImportComponents$$", importComponent.ToString());

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
