using Orleans;
using System;
using System.Threading.Tasks;

namespace Shared.Interface
{
    public interface IUserGrain : IGrainWithIntegerKey
    {
        Task AddToBasket(Guid productId, int quantity);

        Task<string> GetBasket(string id);

        Task ClearBasket();
    }
}
