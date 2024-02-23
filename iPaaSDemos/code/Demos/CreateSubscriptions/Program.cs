using Azure.Messaging.ServiceBus.Administration;
using Azure.Messaging.ServiceBus;

internal class Program
{

    private const string FraudDetectionSubscriptionName = "FraudDetectionSubscription";
    private const string HighValueSubscriptionName = "HighValueSubscription";
    private const string ShippingCostSubscriptionName = "ShippingCostSubscription";

    private static ServiceBusClient s_client;
    private static ServiceBusAdministrationClient s_adminClient;

    private static async Task Main(string[] args)
    {
        await Initialize();
        Console.WriteLine("Intialized");
        Console.ReadKey();
    }

    public static async Task Initialize()
    {
        var connection = Environment.GetEnvironmentVariable("ConnStr");
        var topic = "ordertopic";
        s_client = new ServiceBusClient(connection);
        s_adminClient = new ServiceBusAdministrationClient(connection);

        if (!await s_adminClient.TopicExistsAsync(topic))
        {
            await s_adminClient.CreateTopicAsync(topic);
        }
        if (!await s_adminClient.SubscriptionExistsAsync(topic, FraudDetectionSubscriptionName))
        {
            await s_adminClient.CreateSubscriptionAsync(topic, FraudDetectionSubscriptionName);
            await s_adminClient.DeleteRuleAsync(topic, FraudDetectionSubscriptionName, RuleProperties.DefaultRuleName);
            await s_adminClient.CreateRuleAsync(topic, FraudDetectionSubscriptionName,
                new CreateRuleOptions(RuleProperties.DefaultRuleName, new TrueRuleFilter()));
        }

        if (!await s_adminClient.SubscriptionExistsAsync(topic, ShippingCostSubscriptionName))
        {
            await s_adminClient.CreateSubscriptionAsync(
                new CreateSubscriptionOptions(topic, ShippingCostSubscriptionName),
                new CreateRuleOptions
                {
                    Name = "amount",
                    Filter = new SqlRuleFilter("amount > 10")
                });
        }

        if (!await s_adminClient.SubscriptionExistsAsync(topic, HighValueSubscriptionName))
        {
            await s_adminClient.CreateSubscriptionAsync(
                 new CreateSubscriptionOptions(topic, HighValueSubscriptionName),
                 new CreateRuleOptions
                 {
                     Name = "amount",
                     Filter = new SqlRuleFilter("amount > 1000")
                 });


            await s_client.DisposeAsync();
        }
    }
}