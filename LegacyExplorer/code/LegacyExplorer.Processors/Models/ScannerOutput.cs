using LegacyExplorer.Processors.Models;
using System.Collections.Generic;

namespace LegacyExplorer.Processors
{
    public class ScannerOutput
    {
        public ScannerOutput()
        {
            Assemblies = new List<NetAssembly>();
            Fields = new List<NetField>();
            References = new List<NetReference>();
            Methods = new List<NetMethod>();
            Types = new List<NetType>();

        }
        public List<NetAssembly> Assemblies { get; set; }

        public List<NetType> Types { get; set; }
        public List<NetField> Fields { get; set; }
        public List<NetReference> References { get; set; }
        public List<NetMethod> Methods { get; set; }
    }


}
