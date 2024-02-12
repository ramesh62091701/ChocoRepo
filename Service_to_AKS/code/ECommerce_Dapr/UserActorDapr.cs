using Dapr.Actors.Runtime;
using ECommerce_Dapr.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECommerce_Dapr
{
    public class UserActorDapr : Actor , IUserActorDapr
    {
        private const string StateNames = "statestore";
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

        protected override Task OnActivateAsync()
        {
            return base.OnActivateAsync();
        }
    }
}
