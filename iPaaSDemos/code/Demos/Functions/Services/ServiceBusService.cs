using Azure.Messaging.ServiceBus.Administration;
using Azure.Messaging.ServiceBus;
using System;
using System.Text;
using System.Threading.Tasks;
using Functions.Interfaces;
using Functions.Models;
using Newtonsoft.Json;
using System.Collections.Generic;

namespace Functions.Services
{
    public class ServiceBusService : IServiceBusService
    {
        private static ServiceBusClient s_client;
        private static ServiceBusAdministrationClient s_adminClient;
        private static ServiceBusSender s_sender;
        private readonly ISettingService _settingService;

        public ServiceBusService(ISettingService settingService)
        {
            _settingService = settingService;
        }

        public async Task Send<T>(T obj, Dictionary<string, object> properties) where T : class
        {
            await Initialize(_settingService);
            var messageBody = JsonConvert.SerializeObject(obj);
            var serviceBusMessage = new ServiceBusMessage(Encoding.UTF8.GetBytes(messageBody));
            if(properties != null)
            {
                foreach(var property in properties)
                {
                    serviceBusMessage.ApplicationProperties.Add(property.Key, property.Value);
                }
            }
            await s_sender.SendMessageAsync(serviceBusMessage);
        }

        public static async Task Initialize(ISettingService settingService)
        {
            if (s_client != null) return;
            var connection = await settingService.GetAsync(SettingPropertyNames.AzureServiceBusConnection);
            var topic = await settingService.GetAsync(SettingPropertyNames.OrderTopic);


            var fraudDetectionSubscriptionName = await settingService.GetAsync(SettingPropertyNames.FraudDetectionSubscriptionName);
            var highValueSubscriptionName = await settingService.GetAsync(SettingPropertyNames.HighValueSubscriptionName);
            var shippingCostSubscriptionName = await settingService.GetAsync(SettingPropertyNames.ShippingCostSubscriptionName);


            s_client = new ServiceBusClient(connection);
            s_sender = s_client.CreateSender(topic);
            s_adminClient = new ServiceBusAdministrationClient(connection);

            if (!await s_adminClient.TopicExistsAsync(topic))
            {
                await s_adminClient.CreateTopicAsync(topic);
            }
            if (!await s_adminClient.SubscriptionExistsAsync(topic, fraudDetectionSubscriptionName))
            {
                await s_adminClient.CreateSubscriptionAsync(topic, fraudDetectionSubscriptionName);
                await s_adminClient.DeleteRuleAsync(topic, fraudDetectionSubscriptionName, RuleProperties.DefaultRuleName);
                await s_adminClient.CreateRuleAsync(topic, fraudDetectionSubscriptionName,
                    new CreateRuleOptions(RuleProperties.DefaultRuleName, new TrueRuleFilter()));
            }

            if (!await s_adminClient.SubscriptionExistsAsync(topic, shippingCostSubscriptionName))
            {
                await s_adminClient.CreateSubscriptionAsync(
                    new CreateSubscriptionOptions(topic, shippingCostSubscriptionName),
                    new CreateRuleOptions
                    {
                        Name = "amount",
                        Filter = new SqlRuleFilter("amount > 10")
                    });
            }

            if (!await s_adminClient.SubscriptionExistsAsync(topic, highValueSubscriptionName))
            {
                await s_adminClient.CreateSubscriptionAsync(
                     new CreateSubscriptionOptions(topic, highValueSubscriptionName),
                     new CreateRuleOptions
                     {
                         Name = "amount",
                         Filter = new SqlRuleFilter("amount > 1000")
                     });
            }
        }
    }
}
