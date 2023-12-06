using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FrontEndCodeGenerator
{
    internal class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Please enter Designer.cs file path");
            Console.WriteLine(@"For example : D:\Test\Test.Designer.cs");
            Console.Write("Enter :");
            string FilePath = "D:\\test\\student.designer.cs";   // Console.ReadLine();
            if (FilePath.ToLower().Contains("designer.cs"))
            {                
                ParserGenerator parser = new ParserGenerator();
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
}