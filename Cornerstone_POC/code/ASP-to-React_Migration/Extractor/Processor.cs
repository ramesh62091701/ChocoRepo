using Extractor.Model;
using Extractor.Service;
using Extractor.Utils;
using Microsoft.Extensions.DependencyInjection;
using Newtonsoft.Json;

namespace Extractor
{

    public static class Processor
    {
        private static IServiceProvider _serviceProvider;

        static Processor()
        {
            _serviceProvider = ConfigurationSetup.ConfigureServices();
        }

        private async static Task<string> GetHTMLFromFigma(UIRequest request)
        {
            if (!request.IsFigmaUrlOnly)
            {
                Logger.Log("Processing Figma Image.");
                //var gptService = new GPTService();
                var gptService = _serviceProvider.GetService<GPTService>();
                var htmlResponse = await gptService.GetAiResponseForImage(Constants.FigmaImageToHTMLPrompt, string.Empty, Constants.Model, true, request.ImagePath);
                Logger.Log("Processing Completed.");
                return htmlResponse.Message;
            }
            Logger.Log("Started executing AI request.");
            //Will not work if nodes are more.

            var figmaHelper = _serviceProvider.GetService<FigmaHelper>();

            return await figmaHelper.GetContents(request.FigmaUrl);
        }

        private async static Task<List<AspComponent>> GetControlsFromAspx(UIRequest request)
        {
            try
            {
                if (string.IsNullOrEmpty(request.AspxPagePath))
                {
                    return new List<AspComponent>();
                }
                var aspxContent = File.ReadAllText(request.AspxPagePath);
                //var gptService = new GPTService();
                var gptService = _serviceProvider.GetService<GPTService>();
                var jsonResponse = await gptService.GetAiResponseForImage($"<aspx-code>{aspxContent}</aspx-code>/n${Constants.AspxCodeToJson}", string.Empty, Constants.Model, true, request.ImagePath);
                var jsonContent = Helper.SelectJsonArray(jsonResponse.Message);
                var controls = JsonConvert.DeserializeObject<List<AspComponent>>(jsonContent);
                return controls;
            }catch (Exception ex)
            {
                Logger.LogToFile(ex.ToString());
                return new List<AspComponent>();
            }
        }

        private async static Task<bool> MigrateToReactInternal(UIRequest request)
        {
            //var gptService = new GPTService();
            var gptService = _serviceProvider.GetService<GPTService>();
            //Get HTML for Figma Image
            var htmlResponse = await GetHTMLFromFigma(request);
            var reactPrompt = @$"<HTML-Code>
{htmlResponse}
</HTML-Code>
Follow this rules:
1.Convert above HTML Code to react code.
2.Create one single react page with App.jsx as file name.
3.Generate only code, do not give explanation below or above the code.
4.Generate the code in markdown format with ```jsx.";

            //Get React 
            Logger.Log("Generating React components.");
            var reactResponse = await gptService.GetAiResponse(reactPrompt, Constants.ReactSysPrompt, Constants.Model, true);
            var reactCode = Helper.RemoveMarkupCode(reactResponse.Message, "jsx");
            Helper.CreateFile(request.OutputPath, "App.jsx", reactCode);
            

            //Get Separate controls  
            var separatePrompt = @$"
<React-Code>
{reactResponse.Message}
</React-Code>
From above React-Code Separate the components (like Grid, Breadcrumb, etc.) from the provided React code and convert them into JSON data following these rules:
1.Generate only JSON data without any explanation.
2.Import all components in the App.jsx file which is generated. Do not add any other components which are not in React-Code.
3.Use 'src/component/filename' for components in the JSON response filename.
4.Replace 'xxxx' with name of the component.
5.Use the specified JSON format for the response.
[
	{{
		""filename"" : ""src/App.jsx"",
		""content"" : ""its code""
	}},
	{{
		""filename"" : ""src/components/xxxx.jsx"",
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


        public async static Task<bool> MigrateToHtml(UIRequest request)
        {
            Logger.Log("Started processing...");
            var htmlContent = await GetHTMLFromFigma(request);
            htmlContent = Helper.RemoveMarkupCode(htmlContent, "html");
            Logger.Log("AI request executed.");
            Helper.CreateFile(request.OutputPath, "index.html", htmlContent);
            Logger.Log("Completed.");
            return true;
        }


        public async static Task<Components> GetControls(UIRequest request)
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

        public async static Task<bool> MigrateToReact(UIRequest request)
        {
            if ((request.IsFigmaUrlOnly || request.IsBothFigmaUrlAndImage) && request.IsCustom)
            {
                var figmaHelper = _serviceProvider.GetService<FigmaHelper>();
                var content = await figmaHelper.GetFigmaJsonFromUrl(request.FigmaUrl);
                // Enrich controls fetched from Figma url json.
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

