using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ASP_to_React_Migration.Utils
{
    public class Configuration
    {
        public Configuration()
        {
            
            gptApiKey = "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";
            gptUrl = "https://api.openai.com/v1/chat/completions";
        }
        public string gptApiKey { get; set; } 
        public string gptUrl { get; set; } 

    }
}
