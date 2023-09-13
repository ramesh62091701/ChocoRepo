namespace Sonata.Assets.DotnetScanner.System.Entities
{
    public class AzureScannerOutput
    {
        public string? EnvironmentName { get; set; }

        public List<string> Result { get; set; }

        public Exception? Error { get; set; }
    }
}
