namespace Sonata.Assets.ArchitectureAnalyzer.DotNetAnalyzer.Models
{
    public class ArchitectureRule
    {
        public string Namespace { get; set; }
        public string Assembly { get; set; }
        public string Type { get; set; }
        public string Value { get; set; }
    }
}
