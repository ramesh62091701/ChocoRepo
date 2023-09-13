using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SAPBOAnalysis.Models.Universe
{
    public class UniverseRoot
    {
        public Universes universes { get; set; }
    }

    public class Universe
    {
        public int id { get; set; }
        public string cuid { get; set; }
        public string name { get; set; }
        public string description { get; set; }
        public string type { get; set; }
        public string subType { get; set; }
        public int folderId { get; set; }
        public int revision { get; set; }
    }

    public class Universes
    {
        public List<Universe> universe { get; set; }
    }


}
