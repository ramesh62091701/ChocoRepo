﻿using System;

namespace LegacyExplorer.Processors
{
    public class NetField
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
                    this.id = new Guid().ToString();
                }
                return this.id;
            }
            set
            {
                this.id = value;
            }
        }
        public string TypeId { get; set; }

        public string Name { get; set; }

        public string FieldType { get; set; }
    }


}
