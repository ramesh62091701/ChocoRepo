using Functions.Interfaces;
using Functions.Models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Functions.Services
{
    public class OrderService : IOrderService
    {
        private readonly ITableStorageService<OrderEntity> _repo;
        public OrderService(Func<ITableStorageService<OrderEntity>> repo)
        {
            _repo = repo.Invoke();
        }
        public async Task<List<OrderModel>> GetOrders()
        {
            var list = new List<OrderModel>();
            var orders = _repo.GetAllRows();
            foreach (var orderEntity in orders)
            {
                list.Add(JsonConvert.DeserializeObject<OrderModel>(orderEntity.Order));
            }
            return list;
        }

        public async Task<OrderModel> GetOrder(string orderId, string accountId = "123'")
        {
            var orderEntity = await _repo.GetAsync(accountId, orderId);
            return JsonConvert.DeserializeObject<OrderModel>(orderEntity.Order);
        }

        public async Task<string> CreateOrder(OrderModel order)
        {
            var orderEntity = new OrderEntity();
            orderEntity.PartitionKey = order.AccountId;
            orderEntity.RowKey = order.OrderId;
            orderEntity.Order = JsonConvert.SerializeObject(order);
            await _repo.UpsertAsync(orderEntity);
            return order.OrderId;
        }

        public async Task<bool> UpdateOrder(string orderId, OrderModel order, string accountId = "123'")
        {
            return true;
        }

        public async Task<bool> DeleteOrder(string orderId, string accountId = "123'")
        {
            return true;
        }

    }
}
