using Microsoft.Web.Administration;
using Sonata.Assets.DotnetScanner.IISWebServer.Entities;
using Sonata.Assets.Scanner.Core;
using System.Reflection.Metadata;
using System.Reflection.PortableExecutable;

namespace Sonata.Assets.DotnetScanner.IISWebServer
{
    public class IISWebServerScanner : IScanner<IISWebScannerInput, IISWebScannerOutput>
    {
        IISWebScannerOutput output = new IISWebScannerOutput()
        {
            Result = new List<WebProjProperty>()
        };

        public IISWebScannerOutput Scan(IISWebScannerInput input)
        {
            output.ServerName = input.ServerName;

            try
            {
                ReadFromIIS();
                return output;
            }
            catch (Exception ex)
            {
                output.Error = ex;
                return output;
            }
        }

        private void ReadFromIIS()
        {
            using (ServerManager serverManager = new ServerManager())
            {

                var sites = serverManager.Sites;
                int iisNumber = 0;
                string strVersion = "0";

                foreach (Site site in sites)
                {
                    iisNumber = iisNumber + 1;
                    Console.WriteLine(site.Name);
                    var sitemgr = serverManager.Sites.Where(s => s.Id == iisNumber).Single();
                    var applicationRoot = sitemgr.Applications.Where(a => a.Path == "/").Single();
                    var virtualRoot = applicationRoot.VirtualDirectories.Where(v => v.Path == "/").Single();
                    Console.WriteLine(virtualRoot.PhysicalPath);
                    string strPath = virtualRoot.PhysicalPath;

                    var site1 = serverManager.Sites.Where(s => s.Id == iisNumber).SingleOrDefault();
                    string dllFilename = "";

                    if (site.Name != "Default Web Site")
                    {
                        foreach (string dllFileName in Directory.GetFiles(strPath, "*.dll"))
                        {
                            strVersion = WebsiteDetails(Path.Combine(strPath, dllFileName));
                            dllFilename = Path.GetFileNameWithoutExtension(dllFileName);
                        }

                    }

                    output.Result.Add(new WebProjProperty
                    {
                        SiteName = site.Name,
                        TargetFrameworkVersion = strVersion,
                        ApplicationName = dllFilename,
                        VirtualPath = strPath,

                    });

                }
            }
        }

        private string WebsiteDetails(string strDllpath)
        {
            string frmVersion;
            System.Reflection.Assembly myDllAssembly = System.Reflection.Assembly.LoadFile(strDllpath);

            using var fs = new FileStream(strDllpath, FileMode.Open, FileAccess.Read, FileShare.ReadWrite);
            using var peReader = new PEReader(fs);

            if (peReader.HasMetadata)
            {
                MetadataReader reader = peReader.GetMetadataReader();
                frmVersion = reader.MetadataVersion.ToString();
            }
            else
                frmVersion = "v0.0";

            return frmVersion;

        }

        private void WriteOutput()
        {
            using (StreamWriter sw = File.CreateText(@"c:\temp\IISlist.csv"))
            {
                sw.WriteLine("SiteName , ApplicationName, TargetFrameworkVersion , VirtualPath");
                for (int i = 0; i < output.Result.Count; i++)
                {
                    sw.WriteLine(output.Result[i].SiteName + " , " + output.Result[i].ApplicationName +
                                " , " + output.Result[i].TargetFrameworkVersion +
                                " , " + output.Result[i].VirtualPath);

                }
            }
        }
    }
}