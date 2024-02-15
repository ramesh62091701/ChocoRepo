using Orleans;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MVCWebAPI.Interface
{
    public interface IUserGrain : IGrainWithIntegerKey
    {
        Task AddToBasket(Guid productId, int quantity);

        Task<string> GetBasket();

        Task ClearBasket();
    }
}
