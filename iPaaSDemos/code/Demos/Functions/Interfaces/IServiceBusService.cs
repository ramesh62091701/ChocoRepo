using System.Collections.Generic;
using System.Threading.Tasks;

namespace Functions.Interfaces
{
    public interface IServiceBusService
    {
        Task Send<T>(T obj, Dictionary<string, object> properties) where T : class;
    }
}
