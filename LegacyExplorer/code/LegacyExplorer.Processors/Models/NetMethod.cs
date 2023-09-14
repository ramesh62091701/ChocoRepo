using System;

namespace LegacyExplorer.Processors
{
    public class NetMethod
    {
        public string Id
        {
            get
            {
                if (this.Id == String.Empty)
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
        public string TypeId { get; set; }

        public string Name { get; set; }

        public string LineCount { get; set; }
    }


}
