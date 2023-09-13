using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using SAPBOAnalysis.Models;

namespace SAPBOAnalysis
{
    public class ConfigHelper
    {
        private static Configuration _instance;

        public static Configuration config
        {
            get
            {
                if (_instance == null)
                {
                    _instance = JsonConvert.DeserializeObject<Configuration>(File.ReadAllText("appconfig.json"))!;
                }

                return _instance;
            }
        }

        public static int apiLimit { get { return int.Parse(config.apiPageSize); } }
    }
}
