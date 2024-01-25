namespace ActorClient;
using System;
using System.Threading.Tasks;
using Dapr.Actors;
using Dapr.Actors.Client;
using IBankActorInterface;
using INewActorInterface;

public class Program
{
    public static async Task Main(string[] args)
    {
        Console.WriteLine("Creating a New Actor");
        var bankUser2 = ActorProxy.Create<INewActor>(ActorId.CreateRandom(), "NewActor");
        Console.WriteLine("New Actor assigned");

        await bankUser2.SetGreeting("Welcome to New Service!!!!!!!!!");

        string id = await bankUser2.GetActorId();
        Console.WriteLine("Id : "+id);

        string greeting = await bankUser2.GetGreeting();
        Console.WriteLine("Message : "+greeting);
        Console.ReadLine();
    }
}