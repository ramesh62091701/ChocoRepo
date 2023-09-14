using System.Collections.Generic;

namespace LegacyExplorer.Processors
{
    public class ScannerOutput
    {
        public ScannerOutput()
        {
            Assemblies = new List<NetAssembly>();
        }
        public List<NetAssembly> Assemblies { get; set; }
    }


}
