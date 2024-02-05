using Shared;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UserActor
{
    internal interface IUserDaprClient
    {
        Task AddToBasket(string userId, Guid productId, int quantity);

        Task<BasketItem[]> GetBasket(string userId);

        Task ClearBasket(string userId);
    }
}
