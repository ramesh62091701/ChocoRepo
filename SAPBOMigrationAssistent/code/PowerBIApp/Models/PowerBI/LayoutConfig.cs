using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PowerBIApp.Models.PowerBI.LayoutConfig
{
    public class BaseTheme
    {
        public string name { get; set; }
        public string version { get; set; }
        public double type { get; set; }
    }

    public class Expanded
    {
        public Expr expr { get; set; }
    }

    public class Expr
    {
        public Literal Literal { get; set; }
    }

    public class Literal
    {
        public string Value { get; set; }
    }

    public class Objects
    {
        public List<Section> section { get; set; }
        public List<OutspacePane> outspacePane { get; set; }
    }

    public class OutspacePane
    {
        public Properties properties { get; set; }
    }

    public class Properties
    {
        public VerticalAlignment verticalAlignment { get; set; }
        public Expanded expanded { get; set; }
    }

    public class LayoutConfigRoot
    {
        public string version { get; set; }
        public ThemeCollection themeCollection { get; set; }
        public int activeSectionIndex { get; set; }
        public bool defaultDrillFilterOtherVisuals { get; set; }
        public Settings settings { get; set; }
        public Objects objects { get; set; }
    }

    public class Section
    {
        public Properties properties { get; set; }
    }

    public class Settings
    {
        public bool useNewFilterPaneExperience { get; set; }
        public bool allowChangeFilterTypes { get; set; }
        public bool useStylableVisualContainerHeader { get; set; }
        public int queryLimitOption { get; set; }
        public int exportDataMode { get; set; }
        public bool useDefaultAggregateDisplayName { get; set; }
    }

    public class ThemeCollection
    {
        public BaseTheme baseTheme { get; set; }
    }

    public class VerticalAlignment
    {
        public Expr expr { get; set; }
    }


}
