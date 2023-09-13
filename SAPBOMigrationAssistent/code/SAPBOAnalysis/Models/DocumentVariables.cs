using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SAPBOAnalysis.Models.DocumentVariables
{
    public class DocumentVariablesRoot
    {
        public Variables variables { get; set; }
    }

    public class Variable
    {
        [JsonProperty("@dataType")]
        public string dataType { get; set; }

        [JsonProperty("@qualification")]
        public string qualification { get; set; }

        [JsonProperty("@highPrecision")]
        public string highPrecision { get; set; }

        [JsonProperty("@customSort")]
        public string customSort { get; set; }

        [JsonProperty("@stripped")]
        public string stripped { get; set; }

        [JsonProperty("@dataSourceEnriched")]
        public string dataSourceEnriched { get; set; }

        [JsonProperty("@constant")]
        public string constant { get; set; }
        public string id { get; set; }
        public string name { get; set; }
    }

    public class Variables
    {
        public List<Variable> variable { get; set; }
    }


}
