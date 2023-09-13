namespace Sonata.Assets.DotnetScanner.Projects.Entities
{
    public class ProjectScannerOutput
    {
        public string? ProjectPath { get; set; }

        public List<ProjProperty> CSprojProperies = new List<ProjProperty>();
        public List<ProjReferences> CSprojReferences = new List<ProjReferences>();

        public Exception? Error { get; set; }
    }
}