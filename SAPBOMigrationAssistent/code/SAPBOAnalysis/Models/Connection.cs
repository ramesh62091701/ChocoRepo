using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace SAPBOAnalysis.Models.Connection
{
    public class Connection
    {
        [JsonProperty("@type")]
        public string type { get; set; }
        public string id { get; set; }
        public string cuid { get; set; }
        public string name { get; set; }
        public string folderId { get; set; }
    }

    public class Connections
    {
        public List<Connection> connection { get; set; }
    }

    public class ConnectionRoot
    {
        public Connections connections { get; set; }
    }


    public class ConnectionDetails
    {
        [JsonProperty("@type")]
        public string type { get; set; }
        public string id { get; set; }
        public string cuid { get; set; }
        public string name { get; set; }
        public int folderId { get; set; }
        public string path { get; set; }
        public string database { get; set; }
        public string networkLayer { get; set; }
    }

    public class ConnectionDetailsRoot
    {
        public ConnectionDetails connection { get; set; }
    }

}
