using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Dapr.Client;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using ECommerce.CheckoutService.Model;
using ECommerce.API.Model;

namespace ECommerce.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CheckoutController : ControllerBase
    {
        private static readonly Random rnd = new Random(DateTime.UtcNow.Second);
        DaprClient _daprClient;

        public CheckoutController(DaprClient daprClient)
        {
            _daprClient = daprClient;
        }

        [Route("{userId}")]
        public async Task<ApiCheckoutSummary> CheckoutAsync(string userId)
        {
            CheckoutSummary summary =
               await GetCheckoutService().CheckoutAsync(userId);

            return ToApiCheckoutSummary(summary);
        }

        [Route("history/{userId}")]
        public async Task<IEnumerable<ApiCheckoutSummary>> GetHistoryAsync(
           string userId)
        {
            IEnumerable<CheckoutSummary> history =
               await GetCheckoutService().GetOrderHitoryAsync(userId);

            return history.Select(ToApiCheckoutSummary);
        }


        private ApiCheckoutSummary ToApiCheckoutSummary(CheckoutSummary model)
        {
            return new ApiCheckoutSummary
            {
                Products = model.Products.Select(p => new ApiCheckoutProduct
                {
                    ProductId = p.Product.Id,
                    ProductName = p.Product.Name,
                    Price = p.Price,
                    Quantity = p.Quantity
                }).ToList(),
                Date = model.Date,
                TotalPrice = model.TotalPrice
            };
        }

        private ICheckoutService GetCheckoutService()
        {
            long key = LongRandom();

            var userid = new { userId = 123 };
            string jsonUserId = JsonConvert.SerializeObject(userid);
            var result = _daprClient.CreateInvokeMethodRequest(HttpMethod.Get, "checkoutservice", "checkoutasync", jsonUserId);

            _daprClient.InvokeMethodAsync(result);

            return null;
        }

        private long LongRandom()
        {
            byte[] buf = new byte[8];
            rnd.NextBytes(buf);
            long longRand = BitConverter.ToInt64(buf, 0);
            return longRand;
        }
    }
}