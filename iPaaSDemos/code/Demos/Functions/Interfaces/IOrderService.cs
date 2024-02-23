using Functions.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Functions.Interfaces
{
    public interface IOrderService
    {
        Task<List<OrderModel>> GetOrders();
        Task<OrderModel> GetOrder(string orderId, string accountId = "123'");
        Task<string> CreateOrder(OrderModel order);

        Task<bool> UpdateOrder(string orderId, OrderModel order, string accountId = "123'");

        Task<bool> DeleteOrder(string orderId, string accountId = "123'");
    }
}
