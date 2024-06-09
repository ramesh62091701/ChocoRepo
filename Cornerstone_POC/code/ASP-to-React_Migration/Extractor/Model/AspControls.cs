using Newtonsoft.Json;

namespace Extractor.Model
{
    public class AspControls
    {
        public List<AspControls> aspControls { get; set; }
        public List<UcControl> ucControls { get; set; }
    }

    public class AspControl
    {
        public string type { get; set; }
        public string id { get; set; }
        public string runat { get; set; }
        public string contentPlaceHolderID { get; set; }
        public string value { get; set; }
        public string visible { get; set; }
        public string cssClass { get; set; }
        public string onClick { get; set; }
        public string style { get; set; }
        public string onItemDataBound { get; set; }
        public string rel { get; set; }
        public string @class { get; set; }
        public string alternateText { get; set; }
        public string text { get; set; }
    }


    public class UcControl
    {
        public string type { get; set; }
        public string runat { get; set; }
        public string id { get; set; }
        public string watermarkText { get; set; }
        public string startDate { get; set; }
        public string ignoreTimezone { get; set; }
    }
}
