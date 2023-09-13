using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SAPBOAnalysis.Models
{
    public class Login
    {
        public string password { get; set; }
        public string clientType { get; set; }
        public string auth { get; set; }
        public string userName { get; set; }
    }

    public class LoginResponse
    {
        public string logonToken { get; set; }
    }

    public class Configuration
    {
        public SapConnection sapConnection { get; set; }
        public GenAIConfig genai { get; set; }
        public string csvFilePath { get; set; }
        public PowerBIConfig powerbi { get; set; }
        public string apiPageSize { get; set; }

        public string token { get; set; }
    }

    public class GenAIConfig
    {
        public string completionApi { get; set; }
    }

    public class SapConnection
    {
        public string server { get; set; }
        public string userName { get; set; }
        public string password { get; set; }
        public string auth { get; set; }
    }

    public class PowerBIConfig
    {
        public string clientId { get; set; }
        public string clientSecret { get; set; }
        public string tenantId { get; set; }
        public Guid workspaceId { get; set; }
        public string workspace { get; set; }
        public string authorityUrl { get; set; }
        public string resourceUrl { get; set; }
        public string apiUrl { get; set; }
    }


}
