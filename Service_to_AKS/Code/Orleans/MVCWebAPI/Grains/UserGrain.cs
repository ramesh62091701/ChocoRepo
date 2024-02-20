
using Orleans.Runtime;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MVCWebAPI.Interface;
using System.Security.Cryptography.X509Certificates;
using Orleans;
using Microsoft.Extensions.Logging;

namespace MVCWebAPI.Grains
{
    public class UserGrain :Orleans.Grain, IUserGrain
    {
        private const string StateName = "statestore";
        private readonly IPersistentState<Dictionary<Guid, int>> _basket;

        public UserGrain()
        {
        }

        private readonly ILogger logger;

      


        public async Task AddToBasket(Guid productId, int quantity)
        {
            if (!_basket.State.ContainsKey(productId))
            {
                _basket.State[productId] = 0;
            }

            _basket.State[productId] += quantity;

            await _basket.WriteStateAsync();
        }

        public async Task ClearBasket()
        {
            _basket.State.Clear();

            await _basket.WriteStateAsync();
        }

        public Task<string> GetBasket()
        {
            return Task.FromResult("success");
        }

    }
}
