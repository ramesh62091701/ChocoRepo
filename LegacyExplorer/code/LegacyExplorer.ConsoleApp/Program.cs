using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LegacyExplorer.ConsoleApp
{
    internal class Program
    {
        static void Main(string[] args)
        {
            Processors.FormsScanner scanner = new Processors.FormsScanner();
            scanner.ScanForms(@"D:\test.dll");

        }
    }
}
