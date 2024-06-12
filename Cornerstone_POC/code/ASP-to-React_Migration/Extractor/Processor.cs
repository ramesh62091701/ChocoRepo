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
                Logger.Log("Processing Figma Image.");
                var gptService = new GPTService();
                var htmlResponse = await gptService.GetAiResponseForImage(Constants.FigmaImageToHTMLPrompt, string.Empty, Constants.Model, true, request.ImagePath);
                Logger.Log("Processing Completed.");
                return htmlResponse.Message;
            }
            Logger.Log("Started executing AI request.");
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
            var jsonContent = Helper.SelectJsonArray(jsonResponse.Message);
            var controls = JsonConvert.DeserializeObject<List<AspComponent>>(jsonContent);
            return controls;
        }

        private async static Task<bool> MigrateToReactInternal(Request request)
        {
            var gptService = new GPTService();

            //Get HTML for Figma
            var htmlResponse = await GetHTMLFromFigma(request);
            var reactPrompt = @$"<HTML-Code>
{htmlResponse}
</HTML-Code>
Convert above HTML Code to react code.
Create one single react page.";

            //Get React 
            Logger.Log("Generating React components.");
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

            string jsonArray = Helper.SelectJsonArray(reactSeparateResponse.Message);
            List<FileContent> rootObjects = JsonConvert.DeserializeObject<List<FileContent>>(jsonArray)!;
            if (rootObjects == null)
            {
                Logger.Log("Deserialization failed or the JSON response is empty.");
                return false;
            }
            Logger.Log("React Components created.");

            Logger.Log("Saving React Component files.");
            foreach (var rootObject in rootObjects)
            {
                string filePath = Path.Combine(request.OutputPath, rootObject.FileName);
                string directoryPath = Path.GetDirectoryName(filePath);
                if (!Directory.Exists(directoryPath))
                    Directory.CreateDirectory(directoryPath);
                await File.WriteAllTextAsync(filePath, rootObject.Content);
                Logger.Log($"File created: {filePath}");
            }

            return true;
        }


        public async static Task<bool> MigrateToHtml(Request request)
        {
            Logger.Log("Started processing...");
            var htmlContent = await GetHTMLFromFigma(request);
            htmlContent = Helper.RemoveMarkupCode(htmlContent, "html");
            Logger.Log("AI request executed.");
            Helper.CreateFile(request.OutputPath, "index.html", htmlContent);
            Logger.Log("Completed.");
            return true;
        }


        public async static Task<Components> GetControls(Request request)
        {
            Logger.Log("Started processing...");
            var response = new Components();
            if (request.IsCustom)
            {
                Logger.Log("Getting Aspx Controls");
                response.AspComponents = await GetControlsFromAspx(request);
                Logger.Log("Getting Figma Controls");
                response.FigmaComponents = await ComponentProcess.GetFigmaControls(request);
            }
            Logger.Log("Processing Completed.");
            return response;
        }

        public async static Task<bool> MigrateToReact(Request request)
        {
            if (request.IsFigmaUrl)
            {
                await FigmaHelper.GetContents(request.FigmaUrl);
            }
            if (request.IsCustom)
            {
                Logger.Log("Generating Files");
                await ComponentProcess.Process(request);
            }
            else
            {
                // First convert to html then convert html to react.
                Logger.Log("Started executing AI request.");
                await MigrateToReactInternal(request);
                Logger.Log("AI request executed.");
            }
            Logger.Log("Completed.");
            return true;
        }
    }
}

