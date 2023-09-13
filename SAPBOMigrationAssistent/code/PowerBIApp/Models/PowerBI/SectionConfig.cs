using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PowerBIApp.Models.PowerBI.SectionConfig
{
    public class General
    {
        public Properties properties { get; set; }
    }

    public class Layout
    {
        public int id { get; set; }
        public Position position { get; set; }
    }

    public class Objects
    {
        public List<General> general { get; set; }
    }

    public class Paragraph
    {
        public List<TextRun> textRuns { get; set; }
        public string horizontalTextAlignment { get; set; }
    }

    public class Position
    {
        public double x { get; set; }
        public double y { get; set; }
        public double z { get; set; }
        public double width { get; set; }
        public double height { get; set; }
        public int tabOrder { get; set; }
    }

    public class Properties
    {
        public List<Paragraph> paragraphs { get; set; }
    }

    public class SectionConfigRoot
    {
        public string name { get; set; }
        public List<Layout> layouts { get; set; }
        public SingleVisual singleVisual { get; set; }
    }

    public class SingleVisual
    {
        public string visualType { get; set; }
        public bool drillFilterOtherVisuals { get; set; }
        public Objects objects { get; set; }
    }

    public class TextRun
    {
        public string value { get; set; }
        public TextStyle textStyle { get; set; }
    }

    public class TextStyle
    {
        public string fontWeight { get; set; }
        public string fontSize { get; set; }
        public string color { get; set; }
    }


}
