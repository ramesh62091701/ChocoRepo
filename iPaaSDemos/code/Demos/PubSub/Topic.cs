using System;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Host;
using Microsoft.Extensions.Logging;

namespace PubSub
{
    public class Topic
    {
        [FunctionName("Topic")]
        public void Run([ServiceBusTrigger("OrdersTopic", "mysubscription", Connection = "connection")]string mySbMsg, ILogger log)
        {
            log.LogInformation($"C# ServiceBus topic trigger function processed message: {mySbMsg}");
        }
    }
}
