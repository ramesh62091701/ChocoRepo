using Extractor.Model;
using Extractor.Service;
using Extractor.Utils;
using Newtonsoft.Json;
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

        private async static Task<List<AspComponent>> GetControlsFromAspx(Request request)
        {
            if (string.IsNullOrEmpty(request.AspxPagePath))
            {
                return new List<AspComponent>();
            }
            var aspxContent = File.ReadAllText(request.AspxPagePath);
            var gptService = new GPTService();
            var jsonResponse = await gptService.GetAiResponseForImage($"<aspx-code>{aspxContent}</aspx-code>/n${Constants.AspxCodeToJson}", string.Empty, Constants.Model, true, request.ImagePath);
            var jsonContent = Helper.RemoveMarkupCode(jsonResponse.Message, "json");
            var controls = System.Text.Json.JsonSerializer.Deserialize<List<AspComponent>>(jsonContent);
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
3.Use 'src/component/filename' for components in the JSON response filename.
4.Use the specified JSON format for the response.
[
	{{
		""filename"" : ""src/App.js"",
		""content"" : ""its code""
	}},
	{{
		""filename"" : ""src/components/Grid.jsx"",
		""content"" : ""its code""
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
            List<FileContent> rootObjects = System.Text.Json.JsonSerializer.Deserialize<List<FileContent>>(jsonArray)!;
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


        public async static Task<bool> MigrateToHtml(Request request)
        {
            Logger.Log("Started processing...");
            var htmlContent = await GetHTMLFromFigma(request);
            htmlContent = Helper.RemoveMarkupCode(htmlContent, "html");
            Helper.CreateFile(request.OutputPath, "index.html", htmlContent);
            Logger.Log("Completed.");
            return true;
        }


        public async static Task<ControlResponse> GetControls(Request request)
        {
            Logger.Log("Started processing...");
            var response = new ControlResponse();
            if (request.IsCustom)
            {
                response.AspxComponents = await GetControlsFromAspx(request);
                response.FigmaComponents = await ComponentProcess.GetFigmaControls(request);
            }
            return response;
        }

        public async static Task<bool> MigrateToReact(Request request)
        {
            if (request.IsCustom)
            {
                await ComponentProcess.Process(request);
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

