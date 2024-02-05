using Dapr.Client;
using Shared;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UserActor
{
    public class UserDaprClient : IUserDaprClient
    {
        public static string StoreName = "Store";
        DaprClient _daprclient;
        public UserDaprClient(DaprClient daprClient) 
        { 
            _daprclient = daprClient;
        }

        public async Task AddToBasket(string userId, Guid productId, int quantity)
        {

            await _daprclient.SaveStateAsync(StoreName, userId, productId.ToString() + ":" + quantity.ToString());

        }

        public async Task ClearBasket(string userId)
        {

            await _daprclient.DeleteStateAsync(StoreName, userId);

        }

        public async Task<BasketItem[]> GetBasket(string userId)
        {
            var result = new List<BasketItem>();

            return result.ToArray();
        }
    }
}
