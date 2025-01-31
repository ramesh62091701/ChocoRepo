﻿using LegacyExplorer.Processors.Export;
using System;

namespace LegacyExplorer.Processors
{
    public class NetBaseClass
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
        public string TypeId { get; set; }
        public string TypeName { get; set; }
        [Export]
        public string Namespace { get; set; }

        [Export]
        public string Name { get; set; }
        [Export]
        public string FullName { get; set; }
        [Export]
        public string TypeOfType { get; set; }


    }


}
