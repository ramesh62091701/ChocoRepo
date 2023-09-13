namespace Sonata.Assets.DotnetScanner.IISWebServer.Entities
{
    public class IISWebScannerOutputV2
    {
        public string? ServerName { get; set; }

        public IISMetaInfo? Result { get; set; }
        public Exception? Error { get; set; }
    }
}