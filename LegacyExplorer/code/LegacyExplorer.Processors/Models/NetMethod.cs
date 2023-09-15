using System;

namespace LegacyExplorer.Processors
{
    public class NetMethod
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
        public string TypeId { get; set; }

        public string Name { get; set; }

        public string LineCount { get; set; }
    }


}
