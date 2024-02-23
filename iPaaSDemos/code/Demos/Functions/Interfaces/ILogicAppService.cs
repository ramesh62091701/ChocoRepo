using Functions.Models;
using System.Threading.Tasks;

namespace Functions.Interfaces
{
    public interface ILogicAppService
    {
        Task Send(OrderModel order);
    }
}
