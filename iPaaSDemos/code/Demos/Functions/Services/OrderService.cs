using Functions.Interfaces;
using Functions.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Functions.Services
{
    public class OrderService : IOrderService
    {
        public async Task<List<OrderModel>> GetOrders()
        {
            var list = new List<OrderModel>();
            list.Add(CreateOrder(1001));
            list.Add(CreateOrder(1002));
            return list;
        }

        public async Task<OrderModel> GetOrder(int orderId)
        {
            return CreateOrder(orderId);
        }

        public async Task<int> CreateOrder(OrderModel order)
        {
            return order.OrderId;
        }

        public async Task<bool> UpdateOrder(int orderId, OrderModel order)
        {
            return true;
        }

        public async Task<bool> DeleteOrder(int orderId)
        {
            return true;
        }

        private OrderModel CreateOrder(int orderId)
        {
            var order = new OrderModel();
            order.Products.Add(new ProductModel { ProductId = 5001, Quantity = 10, Price = 10 });
            order.OrderId = orderId;
            order.CustomerId = 11;
            order.CustomerName = "Customer1";
            order.OrderStatus = OrderStatusEnum.Pending;
            order.TotalPrice = 100;
            return order;
        }
    }
}
