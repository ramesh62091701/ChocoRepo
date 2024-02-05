using Dapr.Actors.Runtime;
using Shared;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UserActor
{
    public class UserActorDapr : Actor , IUserActorDapr
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

        public async Task<BasketItem[]> GetBasket()
        {
            var result = new List<BasketItem>();

            var products = await StateManager.GetStateAsync<string>(StateNames);

            foreach (string productId in products.Split(","))
            {
                if (string.IsNullOrEmpty(productId)) continue;
                int quantity = await StateManager.GetStateAsync<int>(productId);
                result.Add(
                   new BasketItem
                   {
                       ProductId = new Guid(productId),
                       Quantity = quantity
                   });
            }

            return result.ToArray();
        }

        /// <summary>
        /// This method is called whenever an actor is activated.
        /// An actor is activated the first time any of its methods are invoked.
        /// </summary>
        protected override Task OnActivateAsync()
        {
            ActorEventSource.Current.ActorMessage(this.Id.ToString(), "Actor activated.");
            return Task.FromResult(true);
        }
    }
}
