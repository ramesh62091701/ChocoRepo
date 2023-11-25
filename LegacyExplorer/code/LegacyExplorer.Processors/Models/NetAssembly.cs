using LegacyExplorer.Processors.Export;
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
        public string Name { get; set; }
        [Export]
        public string FileName { get; set; }
        [Export] 
        public string Type { get; set; }
        [Export]
        public string Location { get; set; }
        [Export]
        public string Framework { get; set; }
        [Export]
        public string Title { get; set; }
        [Export]
        public string Company { get; set; }
        [Export]
        public string Copyright { get; set; }
        [Export]
        public string Version { get; set; }

    }


}
