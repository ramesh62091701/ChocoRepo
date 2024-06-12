using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Extractor.Model
{

    public class BreadcrumbPath
    {
        [JsonProperty("name")]
        public string Name { get; set; }

        [JsonProperty("data-type")]
        public string DataType { get; set; }
    }

    public class FigmaComponent
    {
        public string Id
        {
            get
            {
                string val = string.Empty;
                if (!string.IsNullOrEmpty(Type))
                    val = Type;
                var name = Name;
                if (string.IsNullOrEmpty(name))
                    name = Label;
                if (!string.IsNullOrEmpty(name))
                    val = $"{val}-{name}";
                return val;
            }
        }
        [JsonProperty("type")]
        public string Type { get; set; }

        [JsonProperty("paths")]
        public List<BreadcrumbPath> Paths { get; set; }

        [JsonProperty("name")]
        public string Name { get; set; }

        [JsonProperty("columnNames")]
        public List<string> ColumnNames { get; set; }

        [JsonProperty("label")]
        public string Label { get; set; }

    }


}
