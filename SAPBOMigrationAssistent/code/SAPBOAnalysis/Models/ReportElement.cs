using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection.Metadata;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace SAPBOAnalysis.Models.ReportElement
{
    public class ReportElementRoot
    {
        public ReportElement element { get; set; }
    }

    public class ReportElement
    {
        [JsonProperty("@type")]
        public string type { get; set; }

        [JsonProperty("@isLinkedToSharedElement")]
        public string isLinkedToSharedElement { get; set; }
        public int id { get; set; }
        public string reference { get; set; }
        public string uiref { get; set; }
        public string name { get; set; }
        public int parentId { get; set; }
        //public Size size { get; set; }
        //public Position position { get; set; }
        //public Hide hide { get; set; }
        //public Padding padding { get; set; }
        //public Style style { get; set; }
        public ReportElementContent content { get; set; }
    }

    public class ReportElementContent
    {
        public ReportElementExpression expression { get; set; }

        public override string ToString()
        {
            return expression?.ToString() ?? "";
        }
    }

    public class ReportElementExpression
    {
        public Formula formula { get; set; }
        public Format format { get; set; }

        public override string ToString()
        {
            return $"{formula?.ToString() ?? string.Empty},{format?.ToString() ?? string.Empty}";
        }
    }

    public class Format
    {
        [JsonProperty("@type")]
        public string type { get; set; }
        public Samples samples { get; set; }
        public Template template { get; set; }
        public string token { get; set; }

        public override string ToString()
        {
            return $"FormatType={type},FormatTemplate={template?.ToString() ?? string.Empty},FormatSamples={samples?.ToString() ?? string.Empty}";
        }
    }

    public class Formula
    {
        [JsonProperty("@type")]
        public string type { get; set; }

        [JsonProperty("$")]
        public string value { get; set; }

        public override string ToString()
        {
            return $"FormulaType={type},FormulaValue={value}";
        }
    }

    public class Samples
    {
        [JsonProperty("@positive")]
        public string positive { get; set; }

        public override string ToString()
        {
            return positive;
        }
    }

    public class Template
    {
        [JsonProperty("@positive")]
        public string positive { get; set; }

        public override string ToString()
        {
            return positive;
        }
    }

    public class Alignment
    {
        [JsonProperty("@horizontal")]
        public string horizontal { get; set; }

        [JsonProperty("@vertical")]
        public string vertical { get; set; }

        [JsonProperty("@wrapText")]
        public string wrapText { get; set; }
    }

    public class Background
    {
        [JsonProperty("@mode")]
        public string mode { get; set; }
        public Color color { get; set; }
        public Image image { get; set; }
    }

    public class Border
    {
        public Top top { get; set; }
        public Bottom bottom { get; set; }
        public Left left { get; set; }
        public Right right { get; set; }
    }

    public class Bottom
    {
        [JsonProperty("@thickness")]
        public string thickness { get; set; }

        [JsonProperty("@rgb")]
        public string rgb { get; set; }

        [JsonProperty("@style")]
        public string style { get; set; }
    }

    public class Color
    {
        [JsonProperty("@rgb")]
        public string rgb { get; set; }
    }

    public class Font
    {
        [JsonProperty("@italic")]
        public string italic { get; set; }

        [JsonProperty("@bold")]
        public string bold { get; set; }

        [JsonProperty("@strikethrough")]
        public string strikethrough { get; set; }

        [JsonProperty("@underline")]
        public string underline { get; set; }

        [JsonProperty("@rgb")]
        public string rgb { get; set; }

        [JsonProperty("@size")]
        public int size { get; set; }

        [JsonProperty("@face")]
        public string face { get; set; }
    }

    public class Hide
    {
        [JsonProperty("@always")]
        public string always { get; set; }

        [JsonProperty("@whenEmpty")]
        public string whenEmpty { get; set; }
    }

    public class Image
    {
        [JsonProperty("@src")]
        public string src { get; set; }

        [JsonProperty("@display")]
        public string display { get; set; }
        public Alignment alignment { get; set; }
    }

    public class Left
    {
        [JsonProperty("@thickness")]
        public string thickness { get; set; }

        [JsonProperty("@rgb")]
        public string rgb { get; set; }

        [JsonProperty("@style")]
        public string style { get; set; }
    }

    public class Padding
    {
        [JsonProperty("@left")]
        public int left { get; set; }

        [JsonProperty("@right")]
        public int right { get; set; }

        [JsonProperty("@top")]
        public int top { get; set; }

        [JsonProperty("@bottom")]
        public int bottom { get; set; }
    }

    public class Position
    {
        [JsonProperty("@x")]
        public int x { get; set; }

        [JsonProperty("@y")]
        public int y { get; set; }

        [JsonProperty("@horizontalAnchorType")]
        public string horizontalAnchorType { get; set; }

        [JsonProperty("@verticalAnchorType")]
        public string verticalAnchorType { get; set; }

        [JsonProperty("@repeatOnEveryHorizontalPage")]
        public string repeatOnEveryHorizontalPage { get; set; }

        [JsonProperty("@oneHorizontalPage")]
        public string oneHorizontalPage { get; set; }

        [JsonProperty("@newHorizontalPage")]
        public string newHorizontalPage { get; set; }

        [JsonProperty("@repeatOnEveryVerticalPage")]
        public string repeatOnEveryVerticalPage { get; set; }

        [JsonProperty("@oneVerticalPage")]
        public string oneVerticalPage { get; set; }

        [JsonProperty("@newVerticalPage")]
        public string newVerticalPage { get; set; }

        [JsonProperty("@row")]
        public int row { get; set; }
    }

    public class Right
    {
        [JsonProperty("@thickness")]
        public string thickness { get; set; }

        [JsonProperty("@rgb")]
        public string rgb { get; set; }

        [JsonProperty("@style")]
        public string style { get; set; }
    }

    public class Size
    {
        [JsonProperty("@minimalWidth")]
        public int minimalWidth { get; set; }

        [JsonProperty("@minimalHeight")]
        public int minimalHeight { get; set; }

        [JsonProperty("@autofitWidth")]
        public string autofitWidth { get; set; }

        [JsonProperty("@autofitHeight")]
        public string autofitHeight { get; set; }
    }

    public class Style
    {
        public Border border { get; set; }
        public Background background { get; set; }
        public Font font { get; set; }
        public Alignment alignment { get; set; }
    }

    public class Top
    {
        [JsonProperty("@thickness")]
        public string thickness { get; set; }

        [JsonProperty("@rgb")]
        public string rgb { get; set; }

        [JsonProperty("@style")]
        public string style { get; set; }
    }
}
