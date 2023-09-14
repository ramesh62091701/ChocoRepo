using System;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LegacyExplorer.Processors
{
    public interface IScanner<TInput, TOutput>
    {
        TOutput Scan(TInput input);
    }


}
