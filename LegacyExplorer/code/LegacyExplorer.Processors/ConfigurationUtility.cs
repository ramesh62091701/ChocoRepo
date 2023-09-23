using LegacyExplorer.Processors.Interfaces;
using NLog;
using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LegacyExplorer.Processors
{
    public class ConfigurationUtility : IConfiguration
    {
        #region variable declartion
        private string currentDirect = AppDomain.CurrentDomain.BaseDirectory;
        private int indexOfBin = -1;
        private string _fileName;
        #endregion

        public ConfigurationUtility() { }
        public ConfigurationUtility(string fileName) { 

            _fileName = fileName;

        }
        public void LoadNLogConfiguration() {

            //NLog.LogManager.Setup().LoadConfiguration(builder => {
            //    builder.ForLogger().FilterMinLevel(LogLevel.Info).WriteToConsole();
            //    builder.ForLogger().FilterMinLevel(LogLevel.Debug).WriteToFile(fileName: "App_${shortdate}.txt");
            //});

            indexOfBin = currentDirect.LastIndexOf("bin", StringComparison.OrdinalIgnoreCase);

            if (indexOfBin != -1)
            {
                string xmlFilePath = currentDirect.Substring(0, indexOfBin - 1);

                var fileXmlContent = File.ReadAllText(@xmlFilePath + @"\" + _fileName);
                NLog.LogManager.Setup().LoadConfigurationFromXml(fileXmlContent);
            }
        }
    }
}
