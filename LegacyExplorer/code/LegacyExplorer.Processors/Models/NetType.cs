using System;
using System.Collections.Generic;

namespace LegacyExplorer.Processors
{
    public class NetType
    {
        public NetType()
        {
            Fields = new List<NetField>();
            Methods = new List<NetMethod>();
        }
            
        public string Id
        {
            get
            {
                if (string.IsNullOrEmpty(this.Id))
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

        public string TypeOfType { get; set; }
        public string Namespage { get; set; }

        public string Category { get; set; }

        public List<NetField> Fields { get; set; }

        public List<NetMethod> Methods { get; set; }    

    }


}
