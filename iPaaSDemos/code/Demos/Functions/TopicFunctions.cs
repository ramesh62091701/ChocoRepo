using System;
using Azure.Messaging.ServiceBus;
using System.Text;
using System.Threading.Tasks;
using Functions.Models;
using Microsoft.Azure.WebJobs;
using Microsoft.Extensions.Logging;
using Functions.Interfaces;
using Newtonsoft.Json;

namespace Functions
{
    public class TopicFunctions
    {
        private readonly ISettingService _settingService;
        private readonly IOrderService _orderService;
        public TopicFunctions(ISettingService settingService, IOrderService orderService)
        {
            _settingService = settingService;
            _orderService = orderService;
        }


        [FunctionName("FraudDeduction")]
        public async Task FraudDeduction([ServiceBusTrigger($"%{SettingPropertyNames.OrderTopic}%", $"%{SettingPropertyNames.FraudDetectionSubscriptionName}%", Connection = SettingPropertyNames.AzureServiceBusConnection)] string mySbMsg, ILogger log)
        { 
            var order = JsonConvert.DeserializeObject<OrderModel>(mySbMsg);
            await _orderService.SaveEnrichedProperty(order.OrderId, "FraudDeducted", "false", order.AccountId);
            log.LogInformation($"C# ServiceBus topic trigger function processed message: {mySbMsg}");
        }

        [FunctionName("HighValueSubscription")]
        public async Task HighValueSubscription([ServiceBusTrigger($"%{SettingPropertyNames.OrderTopic}%", $"%{SettingPropertyNames.HighValueSubscriptionName}%", Connection = SettingPropertyNames.AzureServiceBusConnection)] string mySbMsg, ILogger log)
        {
            var order = JsonConvert.DeserializeObject<OrderModel>(mySbMsg);
            await _orderService.SaveEnrichedProperty(order.OrderId, "HighValue", "true", order.AccountId);
            await _orderService.SendToLogicApp(order, await _settingService.GetAsync(SettingPropertyNames.LogicAppProcessing));
            log.LogInformation($"C# ServiceBus topic trigger function processed message: {mySbMsg}");
        }

        [FunctionName("ShippingCostSubscription")]
        public async Task ShippingCostSubscription([ServiceBusTrigger($"%{SettingPropertyNames.OrderTopic}%", $"%{SettingPropertyNames.ShippingCostSubscriptionName}%", Connection = SettingPropertyNames.AzureServiceBusConnection)] string mySbMsg, ILogger log)
        {
            var order = JsonConvert.DeserializeObject<OrderModel>(mySbMsg);
            await _orderService.SaveEnrichedProperty(order.OrderId, "ShippingCost", "true", order.AccountId);
            log.LogInformation($"C# ServiceBus topic trigger function processed message: {mySbMsg}");
        }
    }
}
