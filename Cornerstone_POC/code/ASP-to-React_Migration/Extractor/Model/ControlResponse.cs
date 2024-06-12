using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Extractor.Model
{
    public  class ControlResponse
    {
        public List<AspComponent> AspxComponents { get; set; }

        public List<FigmaComponent> FigmaComponents { get; set; }
    }
}
