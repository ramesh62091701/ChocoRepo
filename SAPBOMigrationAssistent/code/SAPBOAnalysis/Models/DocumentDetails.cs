using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace SAPBOAnalysis.Models.DocumentDetails
{
    public class Document
    {
        public string id { get; set; }
        public string cuid { get; set; }
        public string name { get; set; }
        public int folderId { get; set; }
        public string path { get; set; }
        public string pathIds { get; set; }
        public DateTime updated { get; set; }
        public string scheduled { get; set; }
        public string state { get; set; }
        public string createdBy { get; set; }
        public string lastAuthor { get; set; }
        public int size { get; set; }
        public string refreshOnOpen { get; set; }
    }

    public class DocumentDetailRoot
    {
        public Document document { get; set; }
    }


    public class DocumentPropertiesRoot
    {
        public DocumentProperties properties { get; set; }
    }
    public class DocumentProperties
    {
        private Dictionary<string, string> _props;
        public List<DocumentProperty> property { get; set; }

        public Dictionary<string, string> PropertiesDict
        {
            get
            {
                if (_props == null)
                {
                    _props = property.ToDictionary(x => x.key, y => y.value);
                }
                return _props;
            }
        }
    }

    public class DocumentProperty
    {
        [JsonProperty("@key")]
        public string key { get; set; }

        [JsonProperty("$")]
        public string value { get; set; }
    }


    public class DocumentDpProp
    {
        public string dpname { get; set; }
        public string dpid { get; set; }
        public string dsname { get; set; }
        public string dsid { get; set; }
        public string dsdesc { get; set; }
        public string dspath { get; set; }
    }
}
