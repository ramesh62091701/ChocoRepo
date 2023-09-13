using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace SAPBOAnalysis.Models
{
    public class Dataprovider
    {
        [JsonProperty("@refreshable")]
        public string refreshable { get; set; }
        public string id { get; set; }
        public string name { get; set; }
        public string dataSourceId { get; set; }
        public string dataSourceType { get; set; }
        public DateTime updated { get; set; }
        public string isPartial { get; set; }
        public int rowCount { get; set; }
    }

    public class Dataproviders
    {
        public List<Dataprovider> dataprovider { get; set; }
    }

    public class DataProviderRoot
    {
        public Dataproviders dataproviders { get; set; }
    }
}
