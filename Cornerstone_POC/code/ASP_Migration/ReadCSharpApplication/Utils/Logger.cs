
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using ReadCSharpApplication.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ReadCSharpApplication.Utils
{
    public static class Logger
    {
        private static readonly string logFolder;
        static Logger()
        {
            var appSettings = ConfigurationSetup.ConfigureServices().GetService<IOptions<AppSettings>>().Value;
            logFolder = appSettings.LogFolder;
        }

        public static event Action<string> LogCreated;
        public static void Log(string message)
        {
            Console.WriteLine(message);
            LogCreated?.Invoke(message);
        }

        public static void LogToFile(string message)
        {
            
            if (Directory.Exists(logFolder))
            {
                var time = DateTime.Now;    
                var name = $"{logFolder}\\log_{DateTime.Now.Date.ToString("yyyy-MM-dd")}.txt";
                File.AppendAllText(name, time.ToString());
                File.AppendAllText(name, message);
                File.AppendAllText(name, Environment.NewLine + new string('-', 50) + Environment.NewLine);
            }
        }
    }
}
