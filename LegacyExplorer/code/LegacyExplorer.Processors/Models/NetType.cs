using LegacyExplorer.Processors.Export;
using System;
using System.Collections.Generic;

namespace LegacyExplorer.Processors
{
    public class NetType
    {
        #region variable declartion
        private string id;
        #endregion

        [Export]
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

        [Export]
        public string AssemblyId { get; set; }
        [Export]
        public string Name { get; set; }
        [Export]
        public string TypeOfType { get; set; }

        [Export]
        public string Namespage { get; set; }
        [Export]
        public string Category { get; set; }

    }


}
