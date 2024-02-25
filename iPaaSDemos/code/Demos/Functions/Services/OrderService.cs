using Functions.Interfaces;
using Functions.Models;
using Microsoft.Identity.Client;
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
        private readonly ISettingService _settingService;

        public OrderService(Func<ITableStorageService<OrderEntity>> repo,
            Func<ITableStorageService<OrderEnrichedEntity>> repoEnriched,
            IServiceBusService serviceBusService,
            ILogicAppService logicAppService,
            ISettingService settingService)
        {
            _repo = repo.Invoke();
            _repoEnriched = repoEnriched.Invoke();
            _serviceBusService = serviceBusService;
            _logicAppService = logicAppService;
            _settingService = settingService;
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

        public async Task<OrderModel> GetOrder(string orderId, string accountId = "123")
        {
            var orderEntity = await _repo.GetAsync(accountId, orderId);
            return JsonConvert.DeserializeObject<OrderModel>(orderEntity.Order);
        }

        public async Task<string> CreateOrder(OrderModel order)
        {
            order.Date = DateTime.Now;
            order.OrderStatus = OrderStatusEnum.Pending;
            var orderEntity = new OrderEntity();
            orderEntity.PartitionKey = order.AccountId;
            orderEntity.RowKey = order.OrderId;
            orderEntity.Order = JsonConvert.SerializeObject(order);
            await _repo.UpsertAsync(orderEntity);
            return order.OrderId;
        }

        public async Task<bool> ProcessOrder(string orderId, string accountId = "123")
        {
            var orderEntity  = await _repo.GetAsync(accountId, orderId);
            var order = JsonConvert.DeserializeObject<OrderModel>(orderEntity.Order);
            //Send To Queue
            await Send(order);
            return true;
        }

        public async Task<bool> CompleteOrder(string orderId, string accountId = "123")
        {
            var orderEntity = await _repo.GetAsync(accountId, orderId);
            var order = JsonConvert.DeserializeObject<OrderModel>(orderEntity.Order);
            //Send To Logic App
            await SendToLogicApp(order, await _settingService.GetAsync(SettingPropertyNames.LogicAppComplete));
            return true;
        }

        public async Task<bool> UpdateOrder(string orderId, OrderModel order, string accountId = "123")
        {
            return true;
        }

        public async Task<bool> DeleteOrder(string orderId, string accountId = "123")
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

        public async Task<bool> SendToLogicApp(OrderModel order, string url)
        {
            await _logicAppService.Send(order, url);
            return true;
        }

    }
}
