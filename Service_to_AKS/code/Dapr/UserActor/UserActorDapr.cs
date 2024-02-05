using Dapr.Actors.Runtime;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UserActor
{
    internal class UserActorDapr : Actor
    {
        public UserActorDapr(ActorHost host) : base(host)
        {
        }

        public async Task AddToBasket(Guid productId, int quantity)
        {
            await StateManager.AddOrUpdateStateAsync(productId.ToString(),
               quantity,
               (id, oldQuantity) => oldQuantity + quantity);
        }

        public async Task ClearBasket()
        {
            IEnumerable<string> productIDs = await StateManager.GetStateAsync();

            foreach (string productId in productIDs)
            {
                await StateManager.RemoveStateAsync(productId);
            }
        }
    }
}
