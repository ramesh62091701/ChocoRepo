using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Extractor.Model
{
    public class MethodDependency
    {
        public string CallerMethod { get; set; }
        public string CalledMethod { get; set; }
    }
}
