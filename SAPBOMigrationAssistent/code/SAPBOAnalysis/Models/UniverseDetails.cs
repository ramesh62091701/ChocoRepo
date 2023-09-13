using Newtonsoft.Json;
using SAPBOAnalysis.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SAPBOAnalysis.Models.UniverseDetails
{
    public class CustomProperty
    {
        [JsonProperty("@name")]
        public string name { get; set; }

    }

    public class Folder
    {
        public string id { get; set; }
        public string name { get; set; }
        public string description { get; set; }
        public List<object> customProperty { get; set; }
        public List<Item> item { get; set; }
        public List<Folder> folder { get; set; }
    }

    public class Item
    {
        [JsonProperty("@type")]
        public string type { get; set; }

        [JsonProperty("@dataType")]
        public string dataType { get; set; }

        [JsonProperty("@hasLov")]
        public string hasLov { get; set; }
        public string id { get; set; }
        public string name { get; set; }
        public string description { get; set; }
        public List<CustomProperty> customProperty { get; set; }
        public List<Item> item { get; set; }
        public string path { get; set; }
        public string aggregationFunction { get; set; }
    }

    public class Outline
    {
        [JsonProperty("@aggregated")]
        public string aggregated { get; set; }
        public List<Folder> folder { get; set; }
    }

    public class UniverseDetailsRoot
    {
        public Universe universe { get; set; }
    }

    public class Universe
    {
        public int id { get; set; }
        public string cuid { get; set; }
        public string name { get; set; }
        public string description { get; set; }
        public string type { get; set; }
        public string subType { get; set; }
        public int folderId { get; set; }
        public string path { get; set; }
        public int maxRowsRetrieved { get; set; }
        public int maxRetrievalTime { get; set; }
        public int revision { get; set; }
        public string connected { get; set; }
        public Outline outline { get; set; }
    }


}
