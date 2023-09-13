using SAPBOAnalysis.Models.QueryHelper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SAPBOAnalysis.Models
{
    public class UniverseAnalysis
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Type { get; set; }
        public int FolderId { get; set; }
        public int FolderCount { get; set; }
        public string FolderNames { get; set; }
        public int ObjectsCount { get; set; }
        public string Objects { get; set; }
        public int CalculatedObjectsCount { get; set; }
        public string CalculatedObjects { get; set; }
    }

    public class UniverseSchema
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Type { get; set; }

        public List<QueryTableDetails> queryDetails { get; set; }

        public List<RelationShip> RelationShips { get; set; }
    }
}
