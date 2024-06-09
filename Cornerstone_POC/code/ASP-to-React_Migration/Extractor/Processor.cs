using Extractor.Model;
using Extractor.Service;
using Extractor.Utils;
using System.Net.Http;
using System.Text.Json;
using System.Text.RegularExpressions;

namespace Extractor
{

    public static class Processor
    {
        private async static Task<string> GetHTMLFromFigma(Request request)
        {
            if (!request.IsFigmaUrl)
            {
                var gptService = new GPTService();
                var htmlResponse = await gptService.GetAiResponseForImage(Constants.FigmaImageToHTMLPrompt, string.Empty, Constants.Model, true, request.ImagePath);
                return htmlResponse.Message;
            }
            return await FigmaHelper.GetContents(request.FigmaUrl);
        }

        private async static Task<AspControls> GetControlsFromAspx(Request request)
        {
            if (string.IsNullOrEmpty(request.AspxPagePath))
            {
                return new AspControls();
            }
            var aspxContent = File.ReadAllText(request.AspxPagePath);
            var gptService = new GPTService();
            var jsonResponse = await gptService.GetAiResponseForImage($"<aspx-code>{aspxContent}</aspx-code>/n${Constants.AspxCodeToJson}", string.Empty, Constants.Model, true, request.ImagePath);
            var jsonContent = Helper.RemoveMarkupCode(jsonResponse.Message, "json");
            var controls = JsonSerializer.Deserialize<AspControls>(jsonContent);
            return controls;
        }

        private async static Task<bool> MigrateToReactInternal(Request request)
        {
            var gptService = new GPTService();

            //Get HTML for Figma
            var htmlResponse = await GetHTMLFromFigma(request);
            var reactPrompt = @$"<HTML-Code>\n{htmlResponse}\n</HTML-Code>
Convert above HTML Code to react code.
Create one single react page.";

            //Get React 
            var reactResponse = await gptService.GetAiResponse(reactPrompt, Constants.ReactSysPrompt, Constants.Model, true);

            //Get Separate controls  
            var separatePrompt = @$"
<React-Code>
{reactResponse.Message}
</React-Code>
From above React-Code Separate the components (like Grid, Breadcrumb, etc.) from the provided React code and convert them into JSON data following these rules:
1.Generate only JSON data without any explanation.
2.Call all components in the App.js file.
3.Remember to write all CSS styles for the react component in a 'App.css' file.
4.Use 'src/component/filename' for components in the JSON response filename.
5.Use the specified JSON format for the response.
[
	{{
		""filename"" : ""src/App.js"",
		""content"" : ""its code""
	}},
	{{
		""filename"" : ""src/components/Grid.jsx"",
		""content"" : ""its code""
	}},
	{{
		""filename"" : ""src/components/component.css"",
		""content"" : ""css style code""
	}},
]";
            var reactSeparateResponse = await gptService.GetAiResponse(separatePrompt, Constants.ReactSysPrompt, Constants.Model, true);
            string pattern = @"\[[\s\S]*\]";
            Match match = Regex.Match(reactSeparateResponse.Message, pattern);

            if (!match.Success)
            {
                Logger.Log("No JSON array found in the response.");
                return false;
            }

            string jsonArray = match.Value;
            List<FileContent> rootObjects = JsonSerializer.Deserialize<List<FileContent>>(jsonArray)!;
            if (rootObjects == null)
            {
                Logger.Log("Deserialization failed or the JSON response is empty.");
                return false;
            }
            if (rootObjects != null)
            {
                foreach (var rootObject in rootObjects)
                {
                    string filePath = Path.Combine(request.OutputPath, rootObject.filename);
                    string directoryPath = Path.GetDirectoryName(filePath);
                    if (!Directory.Exists(directoryPath))
                        Directory.CreateDirectory(directoryPath);
                    await File.WriteAllTextAsync(filePath, rootObject.content);
                    Logger.Log($"File created: {filePath}");
                }
            }
            return true;
        }

        private async static Task<bool> MigrateToCSODReact(Request request)
        {
            await DataGridProcessor.Process(request);
            return true;
        }

        public async static Task<bool> MigrateToHtml(Request request)
        {
            Logger.Log("Started processing...");
            var htmlContent = await GetHTMLFromFigma(request);
            htmlContent = Helper.RemoveMarkupCode(htmlContent, "html");
            Helper.CreateFile(request.OutputPath, "index.html", htmlContent);
            Logger.Log("Completed.");
            return true;
        }

        public async static Task<bool> MigrateToReact(Request request)
        {
            Logger.Log("Started processing...");
            if (request.IsCustom)
            {
                var aspxControls = await GetControlsFromAspx(request);
                await MigrateToCSODReact(request);
            }
            else
            {
                // First convert to html then convert html to react.
                await MigrateToReactInternal(request);
            }
            Logger.Log("Completed.");
            return true;
        }
    }
}

