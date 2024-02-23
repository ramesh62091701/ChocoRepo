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
        private Random random = null;
        public TopicFunctions(ISettingService settingService, IOrderService orderService)
        {
            _settingService = settingService;
            _orderService = orderService;
            random = new Random();
        }


        [FunctionName("ReceiveOrders")]
        public void Run([ServiceBusTrigger($"%{SettingPropertyNames.OrderTopic}%", $"%{SettingPropertyNames.OrderSubscription1}%", Connection = SettingPropertyNames.AzureServiceBusConnection)] string mySbMsg, ILogger log)
        { 
            log.LogInformation($"C# ServiceBus topic trigger function processed message: {mySbMsg}");
        }
    }
}
