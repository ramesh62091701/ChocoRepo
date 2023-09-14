using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LegacyExplorer.Processors.Models
{
    public class NetReference
    {
        public string Id
        {
            get
            {
                if (this.Id == String.Empty)
                {
                    this.Id = new Guid().ToString();
                }
                return this.Id;
            }
            set
            {
                this.Id = value;
            }
        }
        public string AssemblyId { get; set; }

        public string Name { get; set; }
        public string Location { get; set; }

    }

}
