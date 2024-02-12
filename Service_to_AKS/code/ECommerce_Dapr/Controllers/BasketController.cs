using System;
using System.Linq;
using System.Threading.Tasks;
using Dapr.Actors.Client;
using Dapr.Actors;
using ECommerce_Dapr.Interfaces;
using ECommerce_Dapr.Model;
using Microsoft.AspNetCore.Mvc;
using ECommerce_Dapr;

namespace ECommerce.API.Controllers
{
   [Route("api/[controller]")]
   [ApiController]
   public class BasketController : ControllerBase
   {
      [HttpGet("{userId}")]
      public async Task<ApiBasket> GetAsync(string userId)
      {
         IUserActorDapr actor = GetActor(userId);

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
         IUserActorDapr actor = GetActor(userId);

         await actor.AddToBasket(request.ProductId, request.Quantity);
      }

      [HttpDelete("{userId}")]
      public async Task DeleteAsync(string userId)
      {
         IUserActorDapr actor = GetActor(userId);

         await actor.ClearBasket();
      }

      private IUserActorDapr GetActor(string userId)
      {
            return ActorProxy.Create<IUserActorDapr>(new ActorId(userId), "UserActorDapr");
      }
   }
}
