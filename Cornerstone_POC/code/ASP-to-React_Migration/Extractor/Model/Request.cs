namespace Extractor.Model
{
    public class Request
    {
        public string ImagePath { get; set; }
        public string FigmaUrl { get; set; }
        public string AspxPagePath { get; set; }
        public string OutputPath { get; set; }
        public bool IsCustom { get; set; }
        public bool IsFigmaUrl { get; set; }
        public ControlResponse ControlResponse { get; set; }

        public Dictionary<string, string> Mapping { get; set; }
    }
}
