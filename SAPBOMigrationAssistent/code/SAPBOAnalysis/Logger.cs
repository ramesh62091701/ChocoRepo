using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SAPBOAnalysis
{
    public static class Logger
    {
        public static event Action<string> LogCreated;
        public static void Log(string message)
        {
            Console.WriteLine(message);
            LogCreated?.Invoke(message);
        }
    }
}
