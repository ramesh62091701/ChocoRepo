using Sonata.Assets.DotnetScanner.IISWebServer.Entities;
using Sonata.Assets.ScannerConsole;

internal class Program
{
    static void Main(string[] args)
    {
        //TestIISScanner.InvokeIISScannerV2();

        TestAzureScanner.InvokeAzureScanner();

        Console.ReadLine();
    }

}
