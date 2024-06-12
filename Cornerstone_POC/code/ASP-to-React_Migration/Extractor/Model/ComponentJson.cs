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

    public class Breadcrumb
    {
        [JsonProperty("type")]
        public string Type { get; set; }

        [JsonProperty("paths")]
        public List<BreadcrumbPath> Paths { get; set; }
    }

    public class DataGrid
    {
        [JsonProperty("type")]
        public string Type { get; set; }

        [JsonProperty("table-name")]
        public string TableName { get; set; }

        [JsonProperty("total-rows")]
        public int TotalRows { get; set; }

        [JsonProperty("column-names")]
        public List<string> ColumnNames { get; set; }

        [JsonProperty("rows")]
        public List<Dictionary<string, string>> Rows { get; set; }

        [JsonProperty("pages")]
        public string Pages { get; set; }
    }

    public class DatePickerInitialValue
    {
        [JsonProperty("start")]
        public string Start { get; set; }

        [JsonProperty("end")]
        public string End { get; set; }
    }

    public class DatePicker
    {
        [JsonProperty("type")]
        public string Type { get; set; }

        [JsonProperty("initialValue")]
        public DatePickerInitialValue InitialValue { get; set; }
    }

    public class TextArea
    {
        [JsonProperty("type")]
        public string Type { get; set; }

        [JsonProperty("property-name")]
        public string PropertyName { get; set; }
    }

    public class Button
    {
        [JsonProperty("type")]
        public string Type { get; set; }

        [JsonProperty("button-names")]
        public List<string> ButtonNames { get; set; }
    }

    public class FigmaComponent
    {
        [JsonProperty("type")]
        public string Type { get; set; }

        [JsonProperty("paths")]
        public List<BreadcrumbPath> Paths { get; set; }

        [JsonProperty("table-name")]
        public string TableName { get; set; }

        [JsonProperty("total-rows")]
        public int? TotalRows { get; set; }

        [JsonProperty("column-names")]
        public List<string> ColumnNames { get; set; }

        [JsonProperty("rows")]
        public List<Dictionary<string, string>> Rows { get; set; }

        [JsonProperty("pages")]
        public string Pages { get; set; }

        [JsonProperty("initialValue")]
        public DatePickerInitialValue InitialValue { get; set; }

        [JsonProperty("property-name")]
        public string PropertyName { get; set; }

        [JsonProperty("button-names")]
        public List<string> ButtonNames { get; set; }
    }


}
