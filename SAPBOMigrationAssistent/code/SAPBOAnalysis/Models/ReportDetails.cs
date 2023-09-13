using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;
using Newtonsoft.Json;
using SAPBOAnalysis.Models;

namespace SAPBOAnalysis.Models.ReportDetails
{
    public class ReportDetailsRoot
    {
        public ReportDetails report { get; set; }
    }

    public class ReportDetails
    {
        [JsonProperty("@hasDatafilter")]
        public string hasDatafilter { get; set; }

        [JsonProperty("@hasDriller")]
        public string hasDriller { get; set; }
        public int id { get; set; }
        public string name { get; set; }
        public string reference { get; set; }
        public string showDataChanges { get; set; }
        public string showFolding { get; set; }
        public List<object> section { get; set; }
        //public Style style { get; set; }
        //public PageSettings pageSettings { get; set; }
        //public string paginationMode { get; set; }
    }

    public class Format
    {
        [JsonProperty("@width")]
        public int width { get; set; }

        [JsonProperty("@height")]
        public int height { get; set; }

        [JsonProperty("@orientation")]
        public string orientation { get; set; }

        [JsonProperty("@papersizeId")]
        public string papersizeId { get; set; }

        [JsonProperty("$")]
        public string papersize { get; set; }
    }

    public class HyperLinkColors
    {
        [JsonProperty("@active")]
        public string active { get; set; }

        [JsonProperty("@hover")]
        public string hover { get; set; }

        [JsonProperty("@link")]
        public string link { get; set; }

        [JsonProperty("@visited")]
        public string visited { get; set; }
    }

    public class Margins
    {
        [JsonProperty("@bottom")]
        public int bottom { get; set; }

        [JsonProperty("@top")]
        public int top { get; set; }

        [JsonProperty("@right")]
        public int right { get; set; }

        [JsonProperty("@left")]
        public int left { get; set; }
    }

    public class PageSettings
    {
        public Margins margins { get; set; }
        public Format format { get; set; }
        public Records records { get; set; }
        public Scaling scaling { get; set; }
    }

    public class Records
    {
        [JsonProperty("@horizontal")]
        public int horizontal { get; set; }

        [JsonProperty("@vertical")]
        public int vertical { get; set; }
    }

    public class Scaling
    {
        [JsonProperty("@factor")]
        public int factor { get; set; }
    }

    public class Style
    {
        public HyperLinkColors hyperLinkColors { get; set; }
    }


    public class ReportElement
    {
        [JsonProperty("@type")]
        public string type { get; set; }

        [JsonProperty("@hasHiddenObjects")]
        public string hasHiddenObjects { get; set; }
        public int id { get; set; }
        public string name { get; set; }
        //public Size size { get; set; }
        public int? parentId { get; set; }

        [JsonProperty("@hasDatafilter")]
        public string hasDatafilter { get; set; }

        [JsonProperty("@hierarchicalOrder")]
        public string hierarchicalOrder { get; set; }
    }

    public class ReportElements
    {
        public List<ReportElement> element { get; set; }
    }

    public class ReportElementsRoot
    {
        public ReportElements elements { get; set; }
    }

}
