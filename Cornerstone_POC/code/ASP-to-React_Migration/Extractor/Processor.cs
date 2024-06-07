using Extractor.Model;
using Extractor.Service;
using Extractor.Utils;
using System.Text.Json;
using System.Text.RegularExpressions;

namespace Extractor
{

    public static class Processor
    {

        public async static Task<bool> MigrateToReact(Request request)
        {
            var gptService = new GPTService();

            //Get HTML for Figma
            var htmlResponse = await gptService.GetAiResponseForImage(Constants.ImagePrompt, Constants.SysPrompt, Constants.Model, true, request.ImagePath);
            var reactPrompt = $"<HTML-Code>\n{htmlResponse.Message}\n</HTML-Code>" + "\n\nConvert above HTML Code to react code.\nCreate one single react page.";

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

        public async static Task<bool> MigrateToCSODReact(Request request)
        {
            await DataGridProcessor.Process(request);
            return true;
        }

        public async static Task<bool> MigrateWithFigma(Request request)
        {
            await FigmaHelper.GetContents(request.ImagePath);
            return true;
        }

        public async static Task<bool> Migrate(Request request)
        {
            if (request.IsCSOD)
            {
                await MigrateToCSODReact(request);
                return true;
            }
            if (request.IsFigmaUrl)
            {
                await MigrateWithFigma(request);
                return true;
            }
            return await MigrateToReact(request);
        }
    }
}

