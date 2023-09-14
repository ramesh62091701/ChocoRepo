using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LegacyExplorer.Processors.Models
{
    internal class Fields
    {
        public long Id { get; set; }
        public long TypeId { get; set; }
        public string FieldName { get; set; }
        public string FieldType { get; set; }
        public string NameSpace { get; set; }

    }
}
