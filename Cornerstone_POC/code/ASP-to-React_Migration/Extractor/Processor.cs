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
            var separatePrompt = $"<React-Code>\n{reactResponse.Message}\n</React-Code>" + "\n\nFrom above React-Code separate all the components like Grid,Breadcrumb etc and call those components from index file and use .jsx file to create a components";
            var reactSeparateResponse = await gptService.GetAiResponse(separatePrompt, Constants.ReactSysPrompt, Constants.Model, true);

            var separate2Prompt = $"<React-Code> {reactSeparateResponse.Message} </React-Code>\n\nConvert the React code in Json in below format.\nRules to follow:\n1.Strictly generate only json data do not give explanation before or after json. \n[\r\n\t{{\r\n\t\t\"file\" : \"app.jsx\",\r\n\t\t\"content\" : \"its code\"\r\n\t}},\r\n\t{{\r\n\t\t\"file\" : \"src/components/grid.jsx\",\r\n\t\t\"content\" : \"its code\"\r\n\t}},\r\n]";
            var separate2Response = await gptService.GetAiResponse(separate2Prompt, Constants.ReactSysPrompt, Constants.Model, true);

            string pattern = @"\[[\s\S]*\]";
            Match match = Regex.Match(separate2Response.Message, pattern);

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
                    string filePath = Path.Combine(request.OutputPath, rootObject.file);
                    string directoryPath = Path.GetDirectoryName(filePath);
                    if (!Directory.Exists(directoryPath))
                        Directory.CreateDirectory(directoryPath);
                    await File.WriteAllTextAsync(filePath, rootObject.content);

                    Logger.Log($"File created: {filePath}");
                }
            }

            return true;
        }
    }
}

