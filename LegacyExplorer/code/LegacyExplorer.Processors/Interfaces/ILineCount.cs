using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Reflection;

namespace LegacyExplorer.Processors.Interfaces
{
    public interface ILineCount<T>
    {
        int GetMethodLineCount(T method);
    }
}
