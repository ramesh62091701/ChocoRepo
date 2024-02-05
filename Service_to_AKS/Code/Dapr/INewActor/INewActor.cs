namespace INewActorInterface;

using System.Threading.Tasks;
using Dapr.Actors;

public interface INewActor : IActor
{
    Task<string> GetActorId();
    Task<string> GetGreeting();
    Task SetGreeting(string greeting);

}