using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Parser
{
    internal class Parser
    {
        Console.WriteLine("Please enter Designer.cs file path");
            Console.WriteLine(@"For example : C:\Bistrack.Net\Main\Common\Controls\SelectButton.Designer.cs");
            Console.Write("Enter :");
            string FilePath = "d:\\test\\student.designer.cs";   // Console.ReadLine();
            if (FilePath.ToLower().Contains("designer.cs"))
            {
                //GetFormDetails(@"C:\Bistrack.Net\Main\Common\Controls\SelectButton.Designer.cs");
                BTCCParser parser = new BTCCParser();
        parser.Parse(FilePath);
                Console.WriteLine("Files created successfully!");
                Console.Read();
            }
            else
            {
                Console.WriteLine("Invalid file path");
                Console.Read();
            }
    }
}
