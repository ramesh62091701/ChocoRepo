using Microsoft.ServiceFabric.Actors;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Stateless1
{
    public interface IMyactor : IActor
    {
        Task<string> GetNameAsync();
    }
}
