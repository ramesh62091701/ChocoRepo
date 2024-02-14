using Microsoft.AspNetCore.Mvc;
using OrleansMVC.Interface;
using OrleansMVC.Model;
using Orleans;

namespace MVCOrleans.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BasketController : ControllerBase
    {
        private readonly IGrainFactory grainFactory;

        public BasketController(IGrainFactory grainFactory) {
            this.grainFactory = grainFactory;
        }
        [HttpGet("{userId}")]
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

        [HttpPost("{userId}")]
        public async Task AddAsync(
           string userId,
           [FromBody] ApiBasketAddRequest request)
        {
            IUserGrain actor = GetActor(userId);

            await actor.AddToBasket(request.ProductId, request.Quantity);
        }

        [HttpDelete("{userId}")]
        public async Task DeleteAsync(string userId)
        {
            IUserGrain actor = GetActor(userId);

            await actor.ClearBasket();
        }

        private IUserGrain GetActor(string userId)
        {
            var grain = grainFactory.GetGrain<IUserGrain>(userId);
            return grain;
        }
    }
}
