using System;
using System.Text;
using Azure.Messaging.ServiceBus;
using Microsoft.Azure.WebJobs;
using Microsoft.Extensions.Logging;
using System.Threading.Tasks;

namespace PubSub
{
    public class Function
    {
        private static string connection = "ConnStrOrdersTopic";
        private static string topicName = "OrdersTopic";

        [FunctionName("OrderPlace")]
        public static async Task Run([TimerTrigger("0 */5 * * * *")]TimerInfo myTimer, ILogger log)
        {
            var s_client = new ServiceBusClient(Environment.GetEnvironmentVariable(connection));
            var s_sender = s_client.CreateSender(topicName);

            try
            {
                // Generate a new order message
                string messageBody = "New order received. Order ID: 12345";
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
    }
}
