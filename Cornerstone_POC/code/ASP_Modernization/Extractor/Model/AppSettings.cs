using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Extractor.Model
{
    public class AppSettings
    {
        public string GptApiKey { get; set; }
        public string GptUrl { get; set; }
        public string FigmaToken { get; set; }
        public string LogFolder { get; set; }

    }
}
