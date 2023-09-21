using LegacyExplorer.Processors.Export;
using System;
using System.ComponentModel;

namespace LegacyExplorer.Processors
{
    public class NetMethod
    {
        #region variable declartion
        private string id;
        #endregion
        [Export, Description("Id")]
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
        [Export, Description("Type Id")]
        public string TypeId { get; set; }
        [Export, Description("Name")]
        public string Name { get; set; }
        [Export]
        public string LineCount { get; set; }
        [Export, Description("Type Name")]
        public string TypeName { get; set; }

    }


}
