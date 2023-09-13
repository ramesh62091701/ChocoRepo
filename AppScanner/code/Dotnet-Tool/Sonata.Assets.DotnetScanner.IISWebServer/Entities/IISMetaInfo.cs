namespace Sonata.Assets.DotnetScanner.IISWebServer.Entities
{
    public class IISMetaInfo
    {
        public string ServerName { get; set; }

        public List<ApplicationPool> ApplicationPools { get; set; }

        public List<VirtualDirectory> VirtualDirectories { get; set; }

        public List<Application> Applications { get; set; }
    }
}