using Orleans;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using WebApplication1.Interface;
using WebApplication1.Model;

namespace MVCOrleans.Controllers
{
    [Route("api/[controller]")]
    public class BasketController : Controller
    {
        private readonly IGrainFactory grainFactory;

        public BasketController(IGrainFactory grainFactory) {
            this.grainFactory = grainFactory;
        }
        [HttpGet]
        [Route("api/Basket/{userId}")]
        public async Task<ApiBasket> GetAsync(string userId)
        {
            IUserGrain actor = GetActor(userId);

            BasketItem[] products = await actor.GetBasket();

            return new ApiBasket()
            {
                UserId = userId,
                Items = products.Select(
                    p => new ApiBasketItem
                    {
                        ProductId = p.ProductId.ToString(),
                        Quantity = p.Quantity
                    })
                    .ToArray()
            };
        }

        [HttpPost]
        [Route("api/Basket/{userId}")]
        public async Task AddAsync(string userId, ApiBasketAddRequest request)
        {
            IUserGrain actor = GetActor(userId);

            await actor.AddToBasket(request.ProductId, request.Quantity);
        }

        [HttpDelete]
        [Route("api/Basket/{userId}")]
        public async Task DeleteAsync(string userId)
        {
            IUserGrain actor = GetActor(userId);

            await actor.ClearBasket();
        }

        private IUserGrain GetActor(string userId)
        {
            var grain =  grainFactory.GetGrain<IUserGrain>(userId);
            return grain;
        }
    }
}
