using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Dapr.Client;
using ECommerce.API.Model;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using ProductCatalog.Model;

namespace ECommerce.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductsController : ControllerBase
    {
        DaprClient _daprClient;

        public ProductsController(DaprClient daprClient)
        {
            _daprClient = daprClient;
        }

        [HttpGet]
        public async Task<IEnumerable<ApiProduct>> GetAsync()
        {
            var response = _daprClient.CreateInvokeMethodRequest(HttpMethod.Get, "productcatalog", "GetAllProductAsync");

            IEnumerable<Product> allProducts = await _daprClient.InvokeMethodAsync<Product[]>(response);

            return allProducts.Select(p => new ApiProduct
            {
                Id = p.Id,
                Name = p.Name,
                Description = p.Description,
                Price = p.Price,
                IsAvailable = p.Availability > 0
            });
        }

        [HttpPost]
        public async Task PostAsync([FromBody] ApiProduct product)
        {
            var newProduct = new Product()
            {
                Id = Guid.NewGuid(),
                Name = product.Name,
                Description = product.Description,
                Price = product.Price,
                Availability = 100
            };
            string jsonNewProduct = JsonConvert.SerializeObject(newProduct);
            var response = _daprClient.CreateInvokeMethodRequest(HttpMethod.Get, "productcatalog", "AddProductAsync", jsonNewProduct);

            await _daprClient.InvokeMethodAsync(response);

        }
    }
}