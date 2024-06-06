using ASP_to_React_Migration.Model;
using ASP_to_React_Migration.Service;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace ASP_to_React_Migration
{
    public static class Extractor
    {

        public async static Task<bool> MigrateToReact(string imagePath, string pathForNewProject)
        {
            var gptService = new GPTService();
            //Get HTML from image
            var imageResponse = await gptService.GetAiResponseForImage(Constants.ImagePrompt, Constants.SysPrompt, Constants.Model, true, imagePath);

            var imgPrompt = $"<HTML-Code>\n{imageResponse.Message}\n</HTML-Code>" + "\n\nConvert above HTML Code to react code.\nCreate one single react page.";
            var reactResponse = await gptService.GetAiResponse(imgPrompt, Constants.ReactSysPrompt, Constants.Model, true);

            var reactPrompt = $"<React-Code>\n{reactResponse.Message}\n</React-Code>" + "\n\nFrom above React-Code separate all the components like Grid,Breadcrumb etc and call those components from index file and use .jsx file to create a components";
            var reactSeperateResponse = await gptService.GetAiResponse(reactPrompt, Constants.ReactSysPrompt, Constants.Model, true);

            //Get Control definition from html.

            var finalPrompt = $"<React-Code> {reactSeperateResponse.Message} </React-Code>\n\nConvert the React code in Json in below format.\nRules to follow:\n1.Strightly Generate only json data do not give explanation before or after json. \n[\r\n\t{{\r\n\t\t\"file\" : \"app.jsx\",\r\n\t\t\"content\" : \"its code\"\r\n\t}},\r\n\t{{\r\n\t\t\"file\" : \"src/components/grid.jsx\",\r\n\t\t\"content\" : \"its code\"\r\n\t}},\r\n]";
            var finalJsonResponse = await gptService.GetAiResponse(finalPrompt, Constants.ReactSysPrompt, Constants.Model, true);

            string pattern = @"\[[\s\S]*\]";
            Match match = Regex.Match(finalJsonResponse.Message, pattern);

            if (match.Success)
            {
                string jsonArray = match.Value;

                List<FileContent> rootObjects = JsonSerializer.Deserialize<List<FileContent>>(jsonArray);

                if (rootObjects != null)
                {
                    foreach (var rootObject in rootObjects)
                    {
                        string filePath = Path.Combine(pathForNewProject, rootObject.file);
                        string directoryPath = Path.GetDirectoryName(filePath);
                        if (!Directory.Exists(directoryPath))
                        {
                            Directory.CreateDirectory(directoryPath);
                        }
                        await File.WriteAllTextAsync(filePath, rootObject.content);

                        Console.WriteLine($"File created: {filePath}");
                    }
                }
                else
                {
                    Console.WriteLine("Deserialization failed or the JSON response is empty.");
                }
            }
            else
            {
                Console.WriteLine("No JSON array found in the response.");
            }

            /*
             * [
            {"type": "Label",
            "props": {
            "text": ""
            }
            },
            {
            "type": "DataPicker",
            "props": {
            "text": ""
            }
            },
            {
            "type": "Grid",
            "props": {
            "header": ""
            },
            "cols": []
            }
            ]*/

            // Loop through each of the controls, create tsx file
            // Create each file for a control
            // Create main file


            return true;
        }
    }
}
