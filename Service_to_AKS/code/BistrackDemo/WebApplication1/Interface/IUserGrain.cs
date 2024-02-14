using Orleans;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WebApplication1.Interface
{
    public interface IUserGrain : IGrainWithStringKey
    {
        Task AddToBasket(Guid productId, int quantity);

        Task<BasketItem[]> GetBasket();

        Task ClearBasket();
    }
}
