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

        [FunctionName("PlaceOrders")]
        public async Task PlaceOrders([TimerTrigger("0 */5 * * * *")] TimerInfo myTimer, ILogger log)
        {
            var connectionString = await _settingService.GetAsync(SettingPropertyNames.AzureServiceBusConnection);
            var s_client = new ServiceBusClient(connectionString);
            var topic = await _settingService.GetAsync(SettingPropertyNames.OrderTopic);
            var s_sender = s_client.CreateSender(topic);

            try
            {
                // Generate a new order message
                string messageBody = JsonConvert.SerializeObject(await _orderService.GetOrder(random.Next()));
                var message = new ServiceBusMessage(Encoding.UTF8.GetBytes(messageBody));

                // Send the message to the topic
                await s_sender.SendMessageAsync(message);
                log.LogInformation($"Message published: {messageBody}");
            }
            catch (Exception ex)
            {
                log.LogError($"An error occurred: {ex.Message}");
            }
            finally
            {
                await s_client.DisposeAsync();
            }
        }


        [FunctionName("ReceiveOrders")]
        public void Run([ServiceBusTrigger($"%{SettingPropertyNames.OrderTopic}%", $"%{SettingPropertyNames.OrderSubscription1}%", Connection = SettingPropertyNames.AzureServiceBusConnection)] string mySbMsg, ILogger log)
        { 
            log.LogInformation($"C# ServiceBus topic trigger function processed message: {mySbMsg}");
        }
    }
}
