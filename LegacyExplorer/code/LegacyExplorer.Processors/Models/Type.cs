using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace LegacyExplorer.Processors.Models
{
    internal class Type
    {
        public long Id { get; set; }
        public long AssemblyId { get; set; }
        public long TypeName { get; set; }
        public string TypeOfType { get; set; }
        public string NameSpace { get; set; }
        public string Category { get; set; }
        
    }
}
