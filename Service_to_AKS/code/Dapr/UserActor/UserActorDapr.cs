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
        private const string StateNames = "StateNames";
        public UserActorDapr(ActorHost host) : base(host)
        {
        }

        public async Task AddToBasket(Guid productId, int quantity)
        {
            await StateManager.AddOrUpdateStateAsync(StateNames, productId.ToString(), (id, products) => $"{products},{productId.ToString()}");
            await StateManager.AddOrUpdateStateAsync(productId.ToString(),
               quantity,
               (id, oldQuantity) => oldQuantity + quantity);
        }

        public async Task ClearBasket()
        {
            var products = await StateManager.GetStateAsync<string>(StateNames);

            foreach (string productId in products.Split(","))
            {
                if (string.IsNullOrEmpty(productId)) continue;
                await StateManager.RemoveStateAsync(productId);
            }
        }
    }
}
