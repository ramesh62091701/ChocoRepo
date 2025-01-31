﻿using LegacyExplorer.Processors.Export;
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
        public string Location { get; set; }
        [Export]
        public string Version { get; set; }
        [Export]
        public string VersionCompatibility { get; set; }
        [Export]
        public string NameSpace { get; set; }
        [Export]
        public string TypeOfType { get; set; }

    }

}
