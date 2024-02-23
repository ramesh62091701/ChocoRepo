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
        private readonly ITableStorageService<OrderEnrichedEntity> _repoEnriched;
        private readonly IServiceBusService _serviceBusService;
        private readonly ILogicAppService _logicAppService;

        public OrderService(Func<ITableStorageService<OrderEntity>> repo,
            Func<ITableStorageService<OrderEnrichedEntity>> repoEnriched,
            IServiceBusService serviceBusService,
            ILogicAppService logicAppService)
        {
            _repo = repo.Invoke();
            _repoEnriched = repoEnriched.Invoke();
            _serviceBusService = serviceBusService;
            _logicAppService = logicAppService;
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
            order.Date = DateTime.Now;
            var orderEntity = new OrderEntity();
            orderEntity.PartitionKey = order.AccountId;
            orderEntity.RowKey = order.OrderId;
            orderEntity.Order = JsonConvert.SerializeObject(order);
            await _repo.UpsertAsync(orderEntity);
            await Send(order);
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

        public async Task<bool> Send(OrderModel order)
        {
            var props = new Dictionary<string, object>();
            props.Add("amount", order.TotalAmount);
            await _serviceBusService.Send(order, props);
            return true;
        }

        public async Task<bool> SaveEnrichedProperty(string orderId, string property, string value, string accountId = "123")
        {
            var entity = new OrderEnrichedEntity();
            entity.OrderId = orderId;
            entity.Property = property;
            entity.Value = value;
            entity.PartitionKey = accountId;
            entity.RowKey = $"{orderId}_{property}";
            await _repoEnriched.UpsertAsync(entity);
            return true;
        }

        public async Task<bool> SendToLogicApp(OrderModel order)
        {
            await _logicAppService.Send(order);
            return true;
        }

    }
}
