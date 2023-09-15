using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LegacyExplorer.Processors.Models
{
    public class NetReference
    {
        #region variable declartion
        private string id;
        #endregion

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
        public string AssemblyId { get; set; }

        public string Name { get; set; }
        public string Location { get; set; }

    }

}
