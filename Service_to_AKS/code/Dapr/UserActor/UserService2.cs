using Dapr.Client;
using Shared;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UserActor
{
    internal class UserService2 : IUserActorDapr
    {
        public static string StateNames = "StateName";
        DaprClient _daprclient;
        public UserService2(DaprClient daprClient) 
        { 
            _daprclient = daprClient;
        }

        public async Task AddToBasket(Guid productId, int quantity)
        {
            //await StateManager.AddOrUpdateStateAsync(StateNames, productId.ToString(), (id, products) => $"{products},{productId.ToString()}");

            await _daprclient.SaveStateAsync(StateNames, productId.ToString(), quantity);

        }

        public async Task ClearBasket(string userId)
        {
            //var products = await StateManager.GetStateAsync<string>(StateNames);
            var productss = await _daprclient.GetStateAsync<string>(StateNames , userId);

            foreach (string productId in productss)
            {
                if (string.IsNullOrEmpty(productId)) continue;
               // await StateManager.RemoveStateAsync(productId);
                await _daprclient.DeleteStateAsync(StateNames,productId);
            }
        }

        public async Task<BasketItem[]> GetBasket(string userId)
        {
            var result = new List<BasketItem>();

            //var products = await StateManager.GetStateAsync<string>(StateNames);
            var productss = await _daprclient.GetStateAsync<BasketItem>(StateNames , userId);

            foreach (string productId in productss)
            {
                if (string.IsNullOrEmpty(productId)) continue;
                //int quantity = await StateManager.GetStateAsync<int>(productId);
                int quantityy = await _daprclient.GetStateAsync<int>(StateNames,productId);
                result.Add(
                   new BasketItem
                   {
                       ProductId = new Guid(productId),
                       Quantity = quantityy
                   });
            }

            return result.ToArray();
        }
    }
}
