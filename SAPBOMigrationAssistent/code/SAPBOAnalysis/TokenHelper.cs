using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using SAPBOAnalysis.Models;

namespace SAPBOAnalysis
{
    public class TokenHelper
    {
        private string _token = string.Empty;

        HttpHelper httpHelper = new HttpHelper();
        private Configuration config;
        public TokenHelper()
        {
            config = JsonConvert.DeserializeObject<Configuration>(File.ReadAllText("appconfig.json"))!;
        }

        public async Task<string> GetToken()
        {
            if (!string.IsNullOrEmpty(_token))
            {
                return _token;
            }

            string url = $"{config.sapConnection.server}/logon/long";
            var request = new Login();
            request.userName = config.sapConnection.userName;
            request.password = config.sapConnection.password;
            request.auth = config.sapConnection.auth;
            request.clientType = string.Empty;
            var response = await httpHelper.Execute<Login, LoginResponse>(request, HttpMethod.Post, "application/json", string.Empty, url);

            _token = response.logonToken;
            return _token;
        }
    }
}
