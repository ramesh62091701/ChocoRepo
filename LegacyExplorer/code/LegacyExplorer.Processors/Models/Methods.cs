using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LegacyExplorer.Processors.Models
{
    internal class Methods
    {
        public long Id { get; set; }
        public long TypeId { get; set; }
        public string MethodName { get; set; }
        public int LineCount { get; set; }
        public string NameSpace { get; set; }

    }
}
