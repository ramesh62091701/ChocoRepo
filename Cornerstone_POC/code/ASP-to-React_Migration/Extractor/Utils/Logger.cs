using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Extractor.Utils
{
    public static class Logger
    {

        public static event Action<string> LogCreated;
        public static void Log(string message)
        {
            Console.WriteLine(message);
            LogCreated?.Invoke(message);
        }

        public static void LogToFile(string message)
        {
            
            if (Directory.Exists(Configuration.LogFolder))
            {
                var name = $"{Configuration.LogFolder}\\log_{DateTime.Now.Date.ToString("yyyy-MM-dd")}.txt";
                File.AppendAllText(name, message);
                File.AppendAllText(name, Environment.NewLine + new string('-', 50) + Environment.NewLine);
            }
        }
    }
}
