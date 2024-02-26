using Azure.Messaging.ServiceBus.Administration;
using Azure.Messaging.ServiceBus;
using System;
using System.CommandLine;
using Azure.Identity;
using System.CommandLine.Invocation;
using System.Threading;

public class Program
{
    private const string TopicName = "TopicSubscriptionWithSessions";
    private const string TopicNameWithoutSession = "TopicSubscriptionWithoutSessions";

    private const string NoFilterSubscriptionName = "NoFilterSubscription";


    private static ServiceBusClient s_client;
    private static ServiceBusAdministrationClient s_adminClient;
    private static ServiceBusSender s_sender;

    public static async Task Main(string[] args)
    {
        var command = new RootCommand("Demonstrates the Topic Filters feature of Azure Service Bus.")
            {
                new Option<string>(
                    alias: "--namespace",
                    description: "Fully qualified Service Bus Queue namespace to use") {Name = "FullyQualifiedNamespace"},
                new Option<string>(
                    alias: "--connection-variable",
                    description: "The name of an environment variable containing the connection string to use.") {Name = "Connection"},
            };
        command.Handler = CommandHandler.Create<string, string>(RunAsync);
        await command.InvokeAsync(args);
    }

    private static async Task RunAsync(string fullyQualifiedNamespace, string connection)
    {
        if (!string.IsNullOrEmpty(connection))
        {
            s_client = new ServiceBusClient(Environment.GetEnvironmentVariable(connection));
            s_adminClient = new ServiceBusAdministrationClient(Environment.GetEnvironmentVariable(connection));
        }
        else if (!string.IsNullOrEmpty(fullyQualifiedNamespace))
        {
            var defaultAzureCredential = new DefaultAzureCredential();
            s_client = new ServiceBusClient(fullyQualifiedNamespace, defaultAzureCredential);
            s_adminClient = new ServiceBusAdministrationClient(fullyQualifiedNamespace, defaultAzureCredential);
        }
        else
        {
            throw new ArgumentException(
                "Either a fully qualified namespace or a connection string environment variable must be specified.");
        }

        Console.WriteLine($"Creating topic {TopicName}");
        await s_adminClient.CreateTopicAsync(TopicName);
        await s_adminClient.CreateTopicAsync(TopicNameWithoutSession);

        s_sender = s_client.CreateSender(TopicName);

        // First Subscription is already created with default rule. Leave as is.
        Console.WriteLine($"Creating subscription {NoFilterSubscriptionName}");
        var sp = await s_adminClient.CreateSubscriptionAsync(TopicNameWithoutSession, NoFilterSubscriptionName);
        sp.Value.RequiresSession = true;
        sp.Value.TopicName = TopicName;

       await s_adminClient.CreateSubscriptionAsync(new CreateSubscriptionOptions(sp.Value));


        Console.WriteLine($"SubscriptionName: {NoFilterSubscriptionName}, Removing and re-adding Default Rule");
        await s_adminClient.DeleteRuleAsync(TopicName, NoFilterSubscriptionName, RuleProperties.DefaultRuleName);
        await s_adminClient.CreateRuleAsync(TopicName, NoFilterSubscriptionName,
            new CreateRuleOptions(RuleProperties.DefaultRuleName, new TrueRuleFilter()));

        // Send messages to Topic
        await SendMessagesAsync();

        await ReceiveMessagesAsync(NoFilterSubscriptionName, "Session 1");
        await ReceiveMessagesAsync(NoFilterSubscriptionName, "Session 2");

        Console.ResetColor();

        Console.WriteLine("=======================================================================");
        Console.WriteLine("Completed Receiving all messages. Disposing clients and deleting topic.");
        Console.WriteLine("=======================================================================");

        Console.WriteLine("Disposing sender");
        await s_sender.CloseAsync();
        Console.WriteLine("Disposing client");
        await s_client.DisposeAsync();

        Console.WriteLine("Deleting topic");

        // Deleting the topic will handle deleting all the subscriptions as well.
        await s_adminClient.DeleteTopicAsync(TopicName);
        await s_adminClient.DeleteTopicAsync(TopicNameWithoutSession);
    }

    private static async Task SendMessagesAsync()
    {
        Console.WriteLine($"==========================================================================");
        Console.WriteLine("Creating messages to send to Topic");
        List<ServiceBusMessage> messages = new();
        messages.Add(CreateMessage(subject: "Red", "Order 1", "Session 1"));
        messages.Add(CreateMessage(subject: "Blue", "Order 1", "Session 2"));
        messages.Add(CreateMessage(subject: "Red", "Order 2", "Session 1"));
        messages.Add(CreateMessage(subject: "Blue", "Order 2", "Session 2"));
        messages.Add(CreateMessage(subject: "Red", "Order 3", "Session 1"));
        messages.Add(CreateMessage(subject: "Blue", "Order 3", "Session 2"));

        Console.WriteLine("Sending messages to send to Topic");
        await s_sender.SendMessagesAsync(messages);
        Console.WriteLine($"==========================================================================");
    }

    private static ServiceBusMessage CreateMessage(string subject, string order, string sessionId)
    {
        ServiceBusMessage message = new() { Subject = subject, SessionId =  sessionId};
        message.ApplicationProperties.Add("Color", subject);
        message.ApplicationProperties.Add("Order", order);

        PrintMessage(message, sessionId);

        return message;
    }

    private static void PrintMessage(ServiceBusMessage message, string sessionId)
    {
        Console.ForegroundColor = (ConsoleColor)Enum.Parse(typeof(ConsoleColor), message.Subject);
        Console.WriteLine($"Created message with color: {message.ApplicationProperties["Color"]}, Order: {message.ApplicationProperties["Order"]}, Session Id: {sessionId}");
        Console.ResetColor();
    }

    private static void PrintReceivedMessage(ServiceBusReceivedMessage message)
    {
        Console.ForegroundColor = (ConsoleColor)Enum.Parse(typeof(ConsoleColor), message.Subject);
        Console.WriteLine($"Received message with color: {message.ApplicationProperties["Color"]}, Order: {message.ApplicationProperties["Order"]}");
        Console.ResetColor();
    }

    private static async Task ReceiveMessagesAsync(string subscriptionName, string sessionId)
    {
        var subscriptionReceiver = await s_client.AcceptSessionAsync(
            TopicName,
            subscriptionName,
            sessionId,
             new ServiceBusSessionReceiverOptions { ReceiveMode = ServiceBusReceiveMode.ReceiveAndDelete });


        Console.WriteLine($"==========================================================================");
        Console.WriteLine($"{DateTime.Now} :: Receiving Messages From Subscription: {subscriptionName}");
        int receivedMessageCount = 0;
        while (true)
        {
            var receivedMessage = await subscriptionReceiver.ReceiveMessageAsync(TimeSpan.FromSeconds(1));
            if (receivedMessage != null)
            {
                PrintReceivedMessage(receivedMessage);
                receivedMessageCount++;
            }
            else
            {
                break;
            }
        }

        Console.WriteLine($"{DateTime.Now} :: Received '{receivedMessageCount}' Messages From Subscription: {subscriptionName}");
        Console.WriteLine($"==========================================================================");
        await subscriptionReceiver.CloseAsync();
        await subscriptionReceiver.DisposeAsync();
    }
}