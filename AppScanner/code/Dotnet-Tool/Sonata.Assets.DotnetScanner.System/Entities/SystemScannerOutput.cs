namespace Sonata.Assets.DotnetScanner.System.Entities
{
    public class SystemScannerOutput
    {
        public string? SystemName { get; set; }

        public List<string> Result { get; set; }

        public Exception? Error { get; set; }
    }
}
