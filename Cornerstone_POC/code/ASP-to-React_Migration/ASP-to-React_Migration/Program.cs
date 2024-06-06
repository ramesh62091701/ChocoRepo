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

        var extractor = new FileOperations(result.Value);
        var gptService = new GPTService();
        var finalResponse = (string.Empty , string.Empty);
        if (result.Value.FilePath != null)
        {
            var fileContent = await extractor.ReadFileContentAsync();
            Console.WriteLine(fileContent.ToString());

            var finalPrompt = $"<ASPX-File>\n{fileContent}\n</ASPX-File>" + Constants.AspxPrompt;
            finalResponse = await gptService.GetAiResponse(finalPrompt, Constants.SysPrompt, Constants.Model, true);
        }

        if (result.Value.ImagePath != null)
        {
            var imageResponse = await gptService.GetAiResponseForImage(Constants.ImagePrompt, Constants.SysPrompt, Constants.Model, true , result.Value.ImagePath);

            var finalImgPrompt = $"HTML Controllers = {imageResponse.Message}" + "For all the above controls generate a complete HTML file. ";
            finalResponse = await gptService.GetAiResponseForImage(finalImgPrompt, Constants.SysPrompt, Constants.Model, true, result.Value.ImagePath);

        }



    }


}