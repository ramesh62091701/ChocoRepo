using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ReadCSharpApplication.Utils
{
    public static class Helper
    {
        public static async Task CreateFile(string filename, string content)
        {
            string destinationPath = "D:\\Demo\\CSOD\\logs";
            filename = filename + ".txt";
            string filePath = Path.Combine(destinationPath, filename);
            await File.WriteAllTextAsync(filePath, content);
            Console.WriteLine("File created in path: " + filePath);
        }
    }
}
