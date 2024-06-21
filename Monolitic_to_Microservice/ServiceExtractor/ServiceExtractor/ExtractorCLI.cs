using CommandLine;

namespace ServiceExtractor
{
    internal class Options
    {
        [Option('c', "command", Required = true, HelpText = "Command to execute")]
        public required string Command { get; set; }

        [Option('r', "controller", Required = true, HelpText = "Name of an Api Controller")]
        public required string Controller { get; set; }

        [Option('j', "json-path", Required = true, HelpText = "Path to JSON to read")]
        public required string JsonPath { get; set; }

        [Option('n', "new-project-path", Required = true, HelpText = "Path to create the new project")]
        public required string PathForNewProject { get; set; }
    }
}
