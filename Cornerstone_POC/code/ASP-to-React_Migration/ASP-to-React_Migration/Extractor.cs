using ASP_to_React_Migration.Model;
using ASP_to_React_Migration.Service;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ASP_to_React_Migration
{
    public static class Extractor
    {

        public async static Task<bool> MigrateToReact(string imagePath)
        {
            var gptService = new GPTService();
            //Get HTML from image
            var imageResponse = await gptService.GetAiResponseForImage(Constants.ImagePrompt, Constants.SysPrompt, Constants.Model, true, imagePath);

            var imgPrompt = $"HTML Controllers = {imageResponse.Message}" + "For all the above controls generate a complete HTML file. ";
            var htmlResponse = await gptService.GetAiResponseForImage(imgPrompt, Constants.SysPrompt, Constants.Model, true, imagePath);

            //Get Control definition from html.

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
