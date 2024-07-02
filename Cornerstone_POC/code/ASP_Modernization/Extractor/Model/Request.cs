namespace Extractor.Model
{
    public class UIRequest
    {
        public string ImagePath { get; set; }
        public string FigmaUrl { get; set; }
        public string AspxPagePath { get; set; }
        public string OutputPath { get; set; }
        public bool IsCustom { get; set; }
        public bool IsFigmaUrlOnly { get; set; }
        public bool IsBothFigmaUrlAndImage{ get; set; }
        public Components Components { get; set; }
        public List<MappedControl> MappedControls { get; set; }
    }
}
