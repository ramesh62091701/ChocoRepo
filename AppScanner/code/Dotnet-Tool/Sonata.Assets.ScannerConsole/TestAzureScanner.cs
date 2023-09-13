using Sonata.Assets.DotnetScanner.AzureEnvironment;
using Sonata.Assets.DotnetScanner.IISWebServer.Entities;
using Sonata.Assets.DotnetScanner.System.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sonata.Assets.ScannerConsole
{
    public class TestAzureScanner
    {

        public static void InvokeAzureScanner()
        {
            AzureEnvironmentScanner scanner = new AzureEnvironmentScanner();
            var output = scanner.Scan(new AzureScannerInput { EnvironmentName = "KPSubscription" });

            // print output
            PrintOutput(output);

            Console.ReadLine();
        }

        public static void PrintOutput(AzureScannerOutput output)
        {
            Console.WriteLine("Server Name: {0}", output.EnvironmentName);
        }
    }
}
