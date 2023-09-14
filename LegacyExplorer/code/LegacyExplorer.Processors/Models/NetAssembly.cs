using LegacyExplorer.Processors.Models;
using System;
using System.Collections.Generic;

namespace LegacyExplorer.Processors
{
    public class NetAssembly
    {
        public NetAssembly()
        {
            Types = new List<NetType>();
            References = new List<NetReference>();
        }
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
                        
        public string Name { get; set; }

        public string FileName { get; set; }
        public string Type { get; set; }

        public string Location { get; set; }

        public List<NetType> Types { get; set; }

        public List<NetReference>  References { get; set; }
    }


}
