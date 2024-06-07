namespace Extractor.Model
{
    public class Request
    {
        public string ImagePath { get; set; }
        public string AspxPagePath { get; set; }
        public string OutputPath { get; set; }
        public bool IsCSOD { get; set; }
        public bool IsFigmaUrl { get; set; }
    }
}
