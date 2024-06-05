using Microsoft.ServiceFabric.Actors.Runtime;
using Microsoft.ServiceFabric.Actors;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Stateless1
{
    public class Myactor : Actor, IMyactor
    {
        public Myactor(ActorService actorService, ActorId actorId) : base(actorService, actorId)
        {
        }

        public Task<string> GetNameAsync()
        {
            return Task.FromResult("Hello from MyActor!");
        }
    }
}
