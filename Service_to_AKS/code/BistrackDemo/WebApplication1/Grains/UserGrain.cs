using Orleans.Runtime;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Security.Cryptography.X509Certificates;
using Orleans;
using WebApplication1.Interface;

namespace WebApplication1.Grains
{
    public class UserGrain : Grain, IUserGrain
    {
        private const string StateName = "statestore";
        private readonly IPersistentState<Dictionary<Guid, int>> _basket;

        public UserGrain([PersistentState(StateName, StateName)] IPersistentState<Dictionary<Guid, int>> basketState)
        {
            _basket = basketState;
        }


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

        public Task<BasketItem[]> GetBasket()
        {
            var items = new List<BasketItem>();

            foreach (var entry in _basket.State)
            {
                items.Add(new BasketItem
                {
                    ProductId = entry.Key,
                    Quantity = entry.Value
                });
            }

            return Task.FromResult(items.ToArray());
        }

    }
}
