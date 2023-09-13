using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PowerBIApp.Models.GenericLayout
{
    public class Container
    {
        public int height { get; set; }
        public int width { get; set; }
        public string visualType { get; set; }
        public string value { get; set; }
        public string schema { get; set; }
        public string tableName { get; set; }
        public List<string> columns { get; set; }
    }

    public class Page
    {
        public string name { get; set; }
        public string displayName { get; set; }
        public List<Container> containers { get; set; }

        public double height { get; set; }
        public double width { get; set; }
    }

    public class GenericLayoutRoot
    {
        public string name { get; set; }
        public List<Page> pages { get; set; }
    }


}
