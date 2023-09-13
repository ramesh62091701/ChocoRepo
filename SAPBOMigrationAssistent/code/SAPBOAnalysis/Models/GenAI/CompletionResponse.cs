using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SAPBOAnalysis.Models.GenAI
{
    public class CompletionResponse
    {
        public CompletionResponseValue value { get; set; }
        public List<object> formatters { get; set; }
        public List<object> contentTypes { get; set; }
        public object declaredType { get; set; }
        public int statusCode { get; set; }
    }

    public class CompletionResponseValue
    {
        public string response { get; set; }
        public object errorMessage { get; set; }
        public bool isSuccess { get; set; }
    }
}
