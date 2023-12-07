using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace FrontEndCodeGenerator
{
    public class Event
    {
        public List<string> AllLines = new List<string>();
        public List<string> BackEndLines = new List<string>();
        public List<string> Callers = new List<string>();
        public long EndLine;
        public string ExactLine;
        public List<string> FrontEndLines = new List<string>();
        public string Name;
        public long StartLine;
        public string Type;
        public List<string> UnKnownLines = new List<string>();
    }

    public class WinForm
    {
        public double EventEstimation { get; set; }
        public string FileFullPath { get; set; }
        public double FormIntergartionEstimation { get; set; }
        public string Name { get; set; }

        public double TotalEstimation
        {
            get
            {
                return UIEstimation + EventEstimation + FormIntergartionEstimation;
            }
        }

        public double UIEstimation { get; set; }
    }

    public class UIControl
    {
        public string Name { get; set; }
        public string Text { get; set; }
        public string Type { get; set; }

        public long MaxLength { get; set; }
        public long Left { get; set; }
        public long Top { get; set; }
        public long Width { get; set; }
        public bool Visible { get; set; }
        public long Height { get; set; }

        public List<UIControl> Child = new List<UIControl>();
        public List<string> Events = new List<string>();

        public bool IsDisabledModified;
        public bool IsMaxLengthModified;
        public bool IsVisibleModified;
        public long ParentHeight { get; set; }
        public string ToolTip { get; set; }
        public string HotKey { get; set; }
        public long TopRounded
        {
            get
            {
                return (ParentHeight + Top) / 10;
            }
        }
        public bool IsEditableDropdown = true;

        public override string ToString()
        {
            return JsonConvert.SerializeObject(this, Newtonsoft.Json.Formatting.Indented);
        }

    }
 
    public enum WorkflowType
    {
        Backend,
        Angular,
        AppStudio
    }
}