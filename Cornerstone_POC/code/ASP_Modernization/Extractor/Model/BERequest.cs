using System.Security.Policy;

namespace Extractor.Model
{
    public class BERequest
    {
        public string SolutionPath { get; set; }
        public string OutputPath { get; set; }
        public string ClassName { get; set; }
        public string Framework { get; set; }
        public Boolean MultipleProject {  get; set; }
        public Boolean AddComments { get; set; }
        public Boolean Swagger { get; set; }
    }
}
