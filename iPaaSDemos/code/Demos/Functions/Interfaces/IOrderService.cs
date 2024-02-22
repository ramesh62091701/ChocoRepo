using Functions.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Functions.Interfaces
{
    public interface IOrderService
    {
        Task<List<OrderModel>> GetOrders();
        Task<OrderModel> GetOrder(int orderId);
        Task<int> CreateOrder(OrderModel order);

        Task<bool> UpdateOrder(int orderId, OrderModel order);

        Task<bool> DeleteOrder(int orderId);
    }
}
