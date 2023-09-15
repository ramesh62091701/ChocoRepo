using LegacyExplorer.Processors.Models;
using System;
using System.Collections.Generic;

namespace LegacyExplorer.Processors
{
    public class NetAssembly
    {
        #region variable declartion
        private string id;
        #endregion
        public NetAssembly()
        {
            Types = new List<NetType>();
            References = new List<NetReference>();
        }
        public string Id
        {
            get
            {
                if (string.IsNullOrEmpty(this.id))
                {
                    this.id = Guid.NewGuid().ToString();
                }
                return this.id;
            }
            set
            {
                this.id = value;
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
