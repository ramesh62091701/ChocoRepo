using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace SAPBOAnalysis.Models
{
    public class ReportsRoot
    {
        public Reports reports { get; set; }
    }

    public class Reports
    {
        public List<Report> report { get; set; }
    }
    public class Report
    {
        [JsonProperty("@hasDatafilter")]
        public string hasDatafilter { get; set; }

        [JsonProperty("@hasDriller")]
        public string hasDriller { get; set; }
        public int id { get; set; }
        public string name { get; set; }
        public string reference { get; set; }
        public string showDataChanges { get; set; }
        public List<object> section { get; set; }
    }
}
