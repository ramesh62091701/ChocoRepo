namespace Sonata.Assets.ArchitectureAnalyzer.DotNetAnalyzer.Models
{
    public class DotNetAnalyzerOutput
    {
        public string? ProjectBinariesPath { get; set; }

        public List<string>? Result { get; set; }

        public Exception? Error { get; set; }

    }

}
