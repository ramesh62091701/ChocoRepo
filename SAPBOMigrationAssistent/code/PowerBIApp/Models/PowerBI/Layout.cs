using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PowerBIApp.Models.PowerBI.Layout
{
    public class Item
    {
        public int type { get; set; }
        public string path { get; set; }
        public string name { get; set; }
    }

    public class ResourcePackage
    {
        public ResourcePackage2 resourcePackage { get; set; }
    }

    public class ResourcePackage2
    {
        public string name { get; set; }
        public double type { get; set; }
        public List<Item> items { get; set; }
        public bool disabled { get; set; }
    }

    public class LayoutRoot
    {
        public int id { get; set; }
        public List<ResourcePackage> resourcePackages { get; set; }
        public List<Section> sections { get; set; }
        public string config { get; set; }
        public int layoutOptimization { get; set; }
    }

    public class Section
    {
        public int id { get; set; }
        public string name { get; set; }
        public string displayName { get; set; }
        public string filters { get; set; }
        public double ordinal { get; set; }
        public List<VisualContainer> visualContainers { get; set; }
        public string config { get; set; }
        public double displayOption { get; set; }
        public double width { get; set; }
        public double height { get; set; }
    }

    public class VisualContainer
    {
        public double x { get; set; }
        public double y { get; set; }
        public double z { get; set; }
        public double width { get; set; }
        public double height { get; set; }
        public string config { get; set; }
        public string filters { get; set; }
        public int? tabOrder { get; set; }
        public string query { get; set; }
        public string dataTransforms { get; set; }
    }

}