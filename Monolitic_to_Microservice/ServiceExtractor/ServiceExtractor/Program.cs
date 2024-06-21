// See https://aka.ms/new-console-template for more information
using CommandLine;
using Service.Extractor.Console;
using ServiceExtractor;

internal class Program
{
    private static void Main(string[] args)
    {
        Console.WriteLine("Hello, World!");

        var result = Parser.Default.ParseArguments<Options>(args)
            .WithNotParsed(HandleParseError)
            .WithParsed(o =>
            {
                var fgColor = Console.ForegroundColor;
                Console.ForegroundColor = ConsoleColor.Green;
                Console.WriteLine("Arguments parsed successfully!");
                Console.ForegroundColor = fgColor;
            });

        Extractor extractor = new Extractor(result.Value);
        extractor.Extract();

        void HandleParseError(IEnumerable<Error> enumerable)
        {
            var fgColor = Console.ForegroundColor;
            Console.ForegroundColor = ConsoleColor.Red;

            Console.WriteLine("Error parsing the arguments");
            Console.ForegroundColor = fgColor;
        }
    }
}