using Dapr.Actors;
using ECommerce_Dapr.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECommerce_Dapr.Interfaces
{
    public interface IUserActorDapr : IActor
    {
        Task AddToBasket(Guid productId, int quantity);

        Task<BasketItem[]> GetBasket();

        Task ClearBasket();
    }
}
