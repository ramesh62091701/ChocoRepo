using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SAPBOAnalysis.Models.UniverseBussinessView
{
    public class BusinessView
    {
        [JsonProperty("@masterView")]
        public string masterView { get; set; }
        public string id { get; set; }
        public string name { get; set; }
        public string description { get; set; }
        public List<FolderRef> folderRef { get; set; }
    }

    public class BusinessViews
    {
        public List<BusinessView> businessView { get; set; }
    }

    public class FolderRef
    {
        [JsonProperty("@id")]
        public string id { get; set; }
        public List<ItemRef> itemRef { get; set; }
        public List<FolderRef> folderRef { get; set; }
    }

    public class ItemRef
    {
        [JsonProperty("@id")]
        public string id { get; set; }
        public List<ItemRef> itemRef { get; set; }
    }

    public class UniverseBussinesViewRoot
    {
        public BusinessViews businessViews { get; set; }
    }


}
