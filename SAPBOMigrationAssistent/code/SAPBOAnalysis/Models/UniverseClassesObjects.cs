using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SAPBOAnalysis.Models
{
    public class UniverseClassesObjects
    {
        public int UniverseId { get; set; }
        public string FolderId { get; set; }
        public string FolderName { get; set; }
        public string ObjectId { get; set; }
        public string ObjectName { get; set; }
        public string DataType { get; set; }
        public string Type { get; set; }
        public bool IsCalculated { get; set; }

    }
}
