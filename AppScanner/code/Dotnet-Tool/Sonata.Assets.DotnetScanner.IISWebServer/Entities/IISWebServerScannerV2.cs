using Microsoft.Web.Administration;
using Sonata.Assets.Scanner.Core;

namespace Sonata.Assets.DotnetScanner.IISWebServer.Entities
{
    /// <summary>
    /// 
    /// </summary>
    public class IISWebServerScannerV2 : IScanner<IISWebScannerInput, IISWebScannerOutputV2>
    {
        IISMetaInfo info = new IISMetaInfo()
        {
            ApplicationPools = new List<ApplicationPool>(),
            VirtualDirectories = new List<VirtualDirectory>(),
            Applications = new List<Application>()

        };

        IISWebScannerOutputV2 output = new IISWebScannerOutputV2();


        public IISWebScannerOutputV2 Scan(IISWebScannerInput input)
        {
            output.ServerName = input.ServerName;
            output.Result = info;

            try
            {
                ReadIISMetadata();
                return output;
            }
            catch (Exception ex)
            {
                output.Error = ex;
                return output;
            }
        }

        private void ReadIISMetadata()
        {
            using (ServerManager serverManager = new ServerManager())
            {

                // Application pool information
                var pools = serverManager.ApplicationPools;
                foreach (var pool in pools)
                {
                    ApplicationPool poolInfo = new ApplicationPool();
                    poolInfo.Name = pool.Name;
                    poolInfo.NetVersion = pool.ManagedRuntimeVersion;
                    info.ApplicationPools.Add(poolInfo);
                }

                // Websites

                var sites = serverManager.Sites;
                foreach (var site in sites)
                {
                    foreach (var app in site.Applications)
                    {
                        Application appinfo = new Application();
                        appinfo.WebSite = site.Name;
                        appinfo.ApplicationPool = app.ApplicationPoolName;
                        appinfo.Path = app.Path;
                        info.Applications.Add(appinfo);

                        foreach (var vdir in app.VirtualDirectories)
                        {
                            VirtualDirectory virtualDir = new VirtualDirectory();
                            virtualDir.AppName = app.Path;
                            virtualDir.Path = vdir.Path;
                            virtualDir.PhysicalPath = vdir.PhysicalPath;
                            info.VirtualDirectories.Add(virtualDir);
                        }
                    }
                }
            }
        }

    }
}