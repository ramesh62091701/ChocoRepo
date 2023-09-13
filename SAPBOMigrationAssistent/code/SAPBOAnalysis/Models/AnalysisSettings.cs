namespace SAPBOAnalysis.Models
{
    public class AnalysisSettings
    {
        public AnalysisSettings()
        {
        }
        public bool universes { get; set; } = false;
        public bool documents { get; set; } = false;
        public bool reports { get; set; } = false;
        public bool connections { get; set; } = false;
    }
}