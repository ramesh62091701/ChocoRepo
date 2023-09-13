using Sonata.Assets.DotnetScanner.IISWebServer.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sonata.Assets.ScannerConsole
{
    public class TestIISScanner
    {

        public static void InvokeIISScannerV2()
        {
            IISWebServerScannerV2 scanner = new IISWebServerScannerV2();
            var output = scanner.Scan(new IISWebScannerInput { ServerName = "LocalSystem" });

            // print output
            PrintOutput(output);

            Console.ReadLine();
        }

        public static void PrintOutput(IISWebScannerOutputV2 outputV2)
        {
            Console.WriteLine("Server Name: {0}", outputV2.ServerName);
            foreach (var app in outputV2.Result?.ApplicationPools)
            {
                Console.WriteLine("Pool Name: {0}, Net Version: {1},", app.Name, app.NetVersion);
            }

            foreach (var app in outputV2.Result?.Applications)
            {
                Console.WriteLine("AppName: {0}, AppPath: {1}, Applcation Pool: {2}", app.Appname, app.Path, app.ApplicationPool);
            }

            foreach (var app in outputV2.Result?.VirtualDirectories)
            {
                Console.WriteLine("Virtual Directory: {0}, AppPath: {1}, Physical Path: {2}", app.AppName, app.Path, app.PhysicalPath);
            }
        }
    }
}
