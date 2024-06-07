// See https://aka.ms/new-console-template for more information
using ASP_to_React_Migration;
using ASP_to_React_Migration.Model;
using ASP_to_React_Migration.Service;
using CommandLine;
using Microsoft.Extensions.Configuration;

internal class Program
{
    private static async Task Main(string[] args)
    {

        var result = Parser.Default.ParseArguments<Options>(args)
        .WithParsed<Options>(o =>
        {
            var fgColor = Console.ForegroundColor;
            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine("Arguments parsed successfully!");
            Console.ForegroundColor = fgColor;
        });

        //var jsonPrompt = "Read the Figma design image and give me the details of all the controls in json format like example data-grid, textarea, Date-picker etc.\nRules to follow while giving json output:\n1.Always generate only json output do not give explanations above or below the json.\n2.Use the below format\n[\r\n\t{\r\n\t  \"type\": \"DataGrid\",\r\n\t  \"table-name\": \"UserDetails\",\r\n\t  \"total-rows\" :  5,\r\n\t  \"column-names\": [ \"Name\", \"ID\", \"Division\", ],\r\n\t  \"rows\": [\r\n\t\t{\r\n\t\t  \"Name\": \"John Doe\",\r\n\t\t  \"ID\": \"123\",\r\n\t\t  \"Division\": \"Sales\",\r\n\t\t},\r\n\t\t{\r\n\t\t  \"Name\": \"Jane Smith\",\r\n\t\t  \"ID\": \"456\",\r\n\t\t  \"Division\": \"Marketing\",\r\n\t\t  \"Position\": \"Executive\",\r\n\t\t},\r\n\t  ],\r\n\t  \"pages\": \"1 of 6\"\r\n\t},\r\n\t{\r\n\t  \"type\": \"DateRangePicker\",\r\n\t  \"initialValue\": {\r\n\t\t\"start\": \"2024-06-04T00:00:00.000Z\",\r\n\t\t\"end\": \"2024-06-12T00:00:00.000Z\"\r\n\t  }\r\n\t}\r\n]";
        //var gptService = new GPTService();
        //var jsonOutput = await gptService.GetAiResponseForImage(jsonPrompt, string.Empty, Constants.Model, true, "./Files/image2.png");

        CreateDataGrid.createGrid("D:\\Generated-React-Files");


        var response = await Extractor.MigrateToReact(result.Value.ImagePath , result.Value.PathForNewProject);









        //var fileOperation = new FileOperations(result.Value);
        //var gptService = new GPTService();
        //var finalResponse = (string.Empty , string.Empty);
        //if (result.Value.FilePath != null)
        //{
        //    var fileContent = await fileOperation.ReadFileContentAsync();
        //    Console.WriteLine(fileContent.ToString());

        //    var finalPrompt = $"<ASPX-File>\n{fileContent}\n</ASPX-File>" + Constants.AspxPrompt;
        //    finalResponse = await gptService.GetAiResponse(finalPrompt, Constants.SysPrompt, Constants.Model, true);
        //}

        //if (result.Value.ImagePath != null)
        //{
        //    var imageResponse = await gptService.GetAiResponseForImage(Constants.ImagePrompt, Constants.SysPrompt, Constants.Model, true , result.Value.ImagePath);

        //    var finalImgPrompt = $"HTML Controllers = {imageResponse.Message}" + "For all the above controls generate a complete HTML file. ";
        //    finalResponse = await gptService.GetAiResponseForImage(finalImgPrompt, Constants.SysPrompt, Constants.Model, true, result.Value.ImagePath);

        //}



    }


}