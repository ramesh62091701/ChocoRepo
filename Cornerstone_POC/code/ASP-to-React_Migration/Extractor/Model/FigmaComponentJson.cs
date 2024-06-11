using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Extractor.Model
{
    public class FigmaComponentJson
    {
        [JsonProperty("type")]
        public string Type { get; set; }

        [JsonProperty("declared-variable")]
        public List<string> DeclaredVariable { get; set; }
    }
}
