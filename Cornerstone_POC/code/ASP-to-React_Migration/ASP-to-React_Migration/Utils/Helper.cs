using ASP_to_React_Migration.Service;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.AccessControl;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace ASP_to_React_Migration.Utils
{

    internal static class Helper
    {

        public const string IGNORE_EMBEDDING = "ignoreEmbedding";
        public static string GetId(string partitionKey, string rowKey)
        {
            return $"{partitionKey}_{rowKey}";
        }

        public static Tuple<string, string> GetKeys(string id)
        {
            var keys = id.Split('_');
            return new Tuple<string, string>(keys[0], keys[1]);
        }

        public static string GetConfig(string key)
        {
            var builder = new ConfigurationBuilder().AddJsonFile("..\\local.setting.json", false, false);
            var config = builder.Build();
            return config["key"];
        }

        
    }
}
