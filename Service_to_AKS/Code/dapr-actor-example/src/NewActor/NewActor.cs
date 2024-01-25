namespace DaprNewActor;

using System;
using System.Text.Json;
using System.Threading.Tasks;
using Dapr.Actors.Runtime;
using INewActorInterface;

public class NewActor : Actor, INewActor
{
    private const string StateName = "statestore";

    private string greeting;
    private string id;

    public NewActor(ActorHost host ) : base(host)
    {
        this.id = this.Id.GetId();
    }

    public Task SetGreeting(string greetings)
    {
        var mess = await this.StateManager.GetOrAddStateAsync<String>(greetings);
    }

    public Task<string> GetActorId()
    {
        return Task.FromResult(this.Id.GetId());
    }


    public async Task<string> GetGreeting()
    {
        var mess = await this.StateManager.GetStateAsync<string>(id);
        return mess;
    }


    protected override Task OnActivateAsync()
    {
        return Task.CompletedTask;
    }

}