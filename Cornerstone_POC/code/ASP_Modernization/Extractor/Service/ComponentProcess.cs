using Newtonsoft.Json;
using System.Text;
using Extractor.Utils;
using Extractor.Model;
using System.Text.RegularExpressions;
using Microsoft.Extensions.DependencyInjection;

namespace Extractor.Service
{
    public static class ComponentProcess
    {
        private static IServiceProvider _serviceProvider;
        static ComponentProcess()
        {
            _serviceProvider = ConfigurationSetup.ConfigureServices();
        }
        public static async Task<List<FigmaComponent>> GetFigmaControls(UIRequest request)
        {
            if (request.IsFigmaUrlOnly)
            {
                return new List<FigmaComponent>();
            }

            var jsonPrompt = @"Read the Figma design image and give me the details of all the controls in json format like example data-grid, textarea, Date-picker etc.
    Rules to follow while giving json output:
    1.Always generate only json output do not give explanations above or below the json.
    2.Table name should be one word without any spaces.
    3.None of the name in type Breadcrumb should contain spaces.
    4.Carefully recognize all the buttons from analyzing the image and add it to below json.
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
                ""label"": ""Comments"",
                ""placeHolder"" : "" Add comment"",
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
            //var gptService = new GPTService();
            var gptService = _serviceProvider.GetService<GPTService>();
            var jsonOutput = await gptService.GetAiResponseForImage(jsonPrompt, string.Empty, Model.Constants.Model, true, request.ImagePath);

            string arrayJson = Helper.SelectJsonArray(jsonOutput.Message);
            List<FigmaComponent> components = JsonConvert.DeserializeObject<List<FigmaComponent>>(arrayJson);
            return components;
        }
        public static async Task<bool> Process(UIRequest request)
        {
            StringBuilder appFileHTMLBuilder = new StringBuilder();
            StringBuilder appFileImportBuilder = new StringBuilder();
            List<FigmaComponent> buttons = new List<FigmaComponent>();
            foreach (var component in request.Components.FigmaComponents)
            {
                switch (component.Type)
                {
                    case "DataGrid":
                        var gridTemplate = GenerateGrid(component, request);
                        appFileHTMLBuilder.AppendLine($"<{gridTemplate.FileName}\nselected={{selected}}\n/>");
                        appFileImportBuilder.AppendLine($"import {{ {gridTemplate.FileName} }} from \"components/{gridTemplate.FileName}\"; ");
                        Helper.CreateFile(request.OutputPath, gridTemplate.FileName + ".tsx", gridTemplate.Content);
                        break;

                    case "TextArea":
                        if (!string.IsNullOrEmpty(component.Label))
                        {
                            var textAreaTemplate = GenerateTextArea(component);
                            appFileHTMLBuilder.AppendLine($"<{textAreaTemplate.FileName}\ninputcomment={{inputComment}}\n/>");
                            appFileImportBuilder.AppendLine($"import {{ {textAreaTemplate.FileName} }} from \"components/{textAreaTemplate.FileName}\"; ");
                            Helper.CreateFile(request.OutputPath, textAreaTemplate.FileName + ".tsx", textAreaTemplate.Content);
                        }
                        break;

                    case "DatePicker":
                        if (!string.IsNullOrEmpty(component.Label))
                        {
                            var datePickerTemplate = GenerateDatePicker(component);
                            appFileHTMLBuilder.AppendLine($"<{datePickerTemplate.FileName}\ndatevalue={{dateValue}}\n/>");
                            appFileImportBuilder.AppendLine($"import {{ {datePickerTemplate.FileName} }} from \"components/{datePickerTemplate.FileName}\"; ");
                            Helper.CreateFile(request.OutputPath, datePickerTemplate.FileName + ".tsx", datePickerTemplate.Content);
                        }
                        break;

                    case "Breadcrumb":
                        var breadcrumbTemplate = GenerateBreadcrumb(component, request);
                        string variablesString = string.Join("", component.Paths.Select(declaredVariable =>
                $"{declaredVariable.Name.ToLower()}={{{declaredVariable.Name}}}\n"));
                        appFileHTMLBuilder.AppendLine($"<{breadcrumbTemplate.FileName}\n");
                        appFileHTMLBuilder.AppendLine($"{variablesString}\n/>");
                        appFileImportBuilder.AppendLine($"import {{ {breadcrumbTemplate.FileName} }} from \"components/{breadcrumbTemplate.FileName}\"; ");
                        Helper.CreateFile(request.OutputPath, breadcrumbTemplate.FileName + ".tsx", breadcrumbTemplate.Content);
                        break;

                    case "Button":
                        buttons.Add(component);
                        break;

                    default:
                        break;
                }
            }
            if (buttons.Any())
            {
                var buttonTemplate = GenerateButtons(buttons, request);
                appFileHTMLBuilder.AppendLine($"<{buttonTemplate.FileName}\ncurrentpage={{currentPage}}\nisdisabled={{isDisabled}}\n/>");
                appFileImportBuilder.AppendLine($"import {{ {buttonTemplate.FileName} }} from \"components/{buttonTemplate.FileName}\"; ");
                Helper.CreateFile(request.OutputPath, buttonTemplate.FileName + ".tsx", buttonTemplate.Content);
            }

            string templateFilePath = "./Templates/App.template";
            string template = File.ReadAllText(templateFilePath);
            template = template.Replace("$$Components$$", appFileHTMLBuilder.ToString())
                .Replace("$$ImportComponents$$", appFileImportBuilder.ToString());
            Helper.CreateFile(request.OutputPath, "App.tsx", template);
            return true;
        }

        private static FileContent GenerateGrid(FigmaComponent dataGrid, UIRequest request)
        {
            string tableName = dataGrid.Name;
            string[] columnNames = dataGrid.ColumnNames.ToArray();
            string templateFilePath = "./Templates/Grid.template";
            string template = File.ReadAllText(templateFilePath);
            string columnsString = string.Join(",\n", columnNames.Select(columnName =>
                $"  {{ accessorKey: \"{columnName.ToLower()}\", header: \"{columnName}\" }}"));

            template = template.Replace("$$Entity$$", tableName)
                           .Replace("$$Columns$$", columnsString);

            var mappedControl = request.MappedControls.FirstOrDefault(x => x.FigmaComponent.Type == "DataGrid" && x.FigmaComponent.Name == tableName);

            if (mappedControl != null && File.Exists(request.AspxPagePath + ".cs"))
            {
                var fetchDetails = Helper.GetMethodDetails(mappedControl.AspComponent.id, request.AspxPagePath + ".cs");
                template = template.Replace("$$FetchDetails$$", $"/*{Environment.NewLine}{fetchDetails}{Environment.NewLine}*/");
            }
            return new FileContent
            {
                FileName = tableName,
                Content = template
            };
        }

        private static FileContent GenerateTextArea(FigmaComponent textArea)
        {
            string propertyName = textArea.Label.Replace(" ","");
            string placeHolder = textArea.PlaceHolder;
            string templateFilePath = "./Templates/Textarea.template";
            string template = File.ReadAllText(templateFilePath);
            template = template.Replace("$$PropertyName$$", propertyName)
                .Replace("$$PlaceHolder$$" , placeHolder);

            return new FileContent
            {
                FileName = propertyName,
                Content = template
            };
        }

        private static FileContent GenerateDatePicker(FigmaComponent datePicker)
        {
            string type = datePicker.Type;
            string templateFilePath = "./Templates/DatePicker.template";
            string template = File.ReadAllText(templateFilePath);
            template = template.Replace("$$PropertyName$$", type);

            return new FileContent
            {
                FileName = type,
                Content = template
            };
        }

        private static FileContent GenerateBreadcrumb(FigmaComponent breadcrumb , UIRequest request)
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

            var mappedControl = request.MappedControls.FirstOrDefault(x => x.FigmaComponent.Type == type);

            if (mappedControl != null && File.Exists(request.AspxPagePath + ".cs"))
            {
                var fetchDetails = Helper.GetMethodDetails(mappedControl.AspComponent.id, request.AspxPagePath + ".cs");
                template = template.Replace("$$FetchDetails$$", $"/*{Environment.NewLine}{fetchDetails}{Environment.NewLine}*/");
            }
            string parametersDeclarationString = string.Join(",\n", paths.Select(path =>
                $" {path.Name.ToLower()}: {path.DataType.ToLower()}"));

            template = template.Replace("$$ComponentName$$", type)
                               .Replace("$$Parameters$$", parametersString)
                               .Replace("$$ParametersAssignments$$", parametersDeclarationString);

            return new FileContent
            {
                FileName = type + "Container",
                Content = template
            };
        }

        private static FileContent GenerateButtons(List<FigmaComponent> buttons , UIRequest request)
        {
            string type = buttons.First().Type;
            var buttonNames = buttons.Select(b => b.Name).ToList();
            string templateFilePath = "./Templates/Button.template";
            string template = File.ReadAllText(templateFilePath);

            List<string> buttonActions = new List<string>();
            List<string> buttonListStrings = new List<string>();

            foreach (var button in buttons)
            {
                string buttonName = button.Name;

                var mappedControl = request.MappedControls.FirstOrDefault(x => x.FigmaComponent.Type == type && x.FigmaComponent.Name == buttonName);

                // Initialize buttonAction for the current button
                string buttonAction = string.Empty;
                if (mappedControl != null && File.Exists(request.AspxPagePath + ".cs"))
                {
                    buttonAction = Helper.GetMethodDetails(mappedControl.AspComponent.id, request.AspxPagePath + ".cs");
                }

                buttonActions.Add($"/*{Environment.NewLine}{buttonAction}{Environment.NewLine}*/");

                string buttonListString = $"\nconst localized{buttonName} = useLocalizationsDefaults(\r\n    `${{object.{buttonName}}}`\r\n  );";
                buttonListStrings.Add(buttonListString);
            }

            string combinedButtonActions = string.Join(Environment.NewLine, buttonActions);
            string combinedButtonListStrings = string.Join(Environment.NewLine, buttonListStrings);

            template = template.Replace("$$ButtonList$$", combinedButtonListStrings)
                .Replace("$$ComponentName$$", type)
                .Replace("$$ButtonAction$$", combinedButtonActions);

            return new FileContent
            {
                FileName = type + "Container",
                Content = template
            };
        }
    }
}
