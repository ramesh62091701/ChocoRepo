namespace Sonata.Assets.DotnetScanner.IISWebServer.Entities
{
    public class IISWebScannerOutput
    {
        public string? ServerName { get; set; }

        public List<WebProjProperty> Result { get; set; }
        public Exception? Error { get; set; }
    }
}