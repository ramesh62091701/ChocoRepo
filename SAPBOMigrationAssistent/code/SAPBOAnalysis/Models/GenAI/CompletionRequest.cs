using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace SAPBOAnalysis.Models.GenAI
{
    public class CompletionRequest
    {
        public string Platform { get; set; }
        public string Prompt { get; set; } = string.Empty;
        public string Model { get; set; } = string.Empty;
        public float Temperature { get; set; }
        public int MaxTokens { get; set; }
        public int MaxTokensToSample { get; set; }
        public int N { get; set; }
    }
}
