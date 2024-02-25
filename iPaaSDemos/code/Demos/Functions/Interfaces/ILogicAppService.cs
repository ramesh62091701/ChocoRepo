using Functions.Models;
using System.Threading.Tasks;

namespace Functions.Interfaces
{
    public interface ILogicAppService
    {
        Task Send(OrderModel order);
        Task Send(OrderModel order, string url);
    }
}
