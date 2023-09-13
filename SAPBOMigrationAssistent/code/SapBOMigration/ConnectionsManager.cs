using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using SAPBOAnalysis.Models;

namespace SapBOMigration
{
    public static class ConnectionsManager
    {
        private static List<Connection> _connections;

        public static List<Connection> GetConnections()
        {
            if (_connections == null)
            {
                _connections = JsonConvert.DeserializeObject<List<Connection>>(File.ReadAllText("data\\connections.json"))!;
            }

            return _connections;
        }
    }

    public class Connection : ConnectionAnalysis
    {
        public string ConnectionString { get; set; } = string.Empty;
    }
}
