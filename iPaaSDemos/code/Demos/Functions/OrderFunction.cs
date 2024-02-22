using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Threading.Tasks;
using Functions.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.Functions.Worker.Http;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.Azure.WebJobs.Extensions.OpenApi.Core.Attributes;
using Microsoft.Azure.WebJobs.Extensions.OpenApi.Core.Enums;
using Microsoft.Extensions.Logging;
using Microsoft.OpenApi.Models;
using Newtonsoft.Json;

namespace Functions
{
    public class OrderFunction
    {
        private readonly ILogger<OrderFunction> _logger;

        public OrderFunction(ILogger<OrderFunction> log)
        {
            _logger = log;
        }

        [FunctionName("GetOrders")]
        [OpenApiOperation(operationId: "GetOrders", tags: new[] { "order" })]
        [OpenApiResponseWithBody(statusCode: HttpStatusCode.OK, contentType: "application/json", bodyType: typeof(List<OrderModel>), Description = "The OK response")]
        public async Task<IActionResult> GetOrders(
            [HttpTrigger(AuthorizationLevel.Anonymous, "get", Route = "orders")] HttpRequest req)
        {
            _logger.LogInformation("C# HTTP trigger function processed a request.");
            var list = new List<OrderModel>();
            list.Add(new OrderModel { OrderId = "1001", CustomerName = "Customer1" });
            list.Add(new OrderModel { OrderId = "1002", CustomerName = "Customer2" });

            return new OkObjectResult(list);
        }

        [FunctionName("GetOrder")]
        [OpenApiOperation(operationId: "GetOrder", tags: new[] { "order" })]
        [OpenApiParameter(name: "orderId", In = ParameterLocation.Path, Required = true, Type = typeof(string), Description = "The **OrderId** parameter")]
        [OpenApiResponseWithBody(statusCode: HttpStatusCode.OK, contentType: "application/json", bodyType: typeof(OrderModel), Description = "The OK response")]
        public async Task<IActionResult> GetOrder(
          [HttpTrigger(AuthorizationLevel.Anonymous, "get", Route = "orders/{orderId}")] HttpRequest req, string orderId)
        {
            _logger.LogInformation("C# HTTP trigger function processed a request.");
            return new OkObjectResult(new OrderModel { OrderId = orderId, CustomerName = "Customer1" });
        }

        [FunctionName("CreateOrder")]
        [OpenApiOperation(operationId: "OrderCreate", tags: new[] { "order" }, Description ="Create a new order", Visibility = OpenApiVisibilityType.Important)]
        [OpenApiRequestBody("application/json", typeof(OrderModel), Description = "Order Id must be defined", Required = true)]
        [OpenApiSecurity("function_key", SecuritySchemeType.ApiKey, Name = "code", In = OpenApiSecurityLocationType.Header)]
        [OpenApiResponseWithBody(statusCode: HttpStatusCode.OK, contentType: "text/plain", bodyType: typeof(string), Description = "The OK response")]
        public async Task<IActionResult> CreateOrder(
           [HttpTrigger(AuthorizationLevel.Function, "post", Route = "orders")] HttpRequest req)
        {
            _logger.LogInformation("C# HTTP trigger function processed a request.");

            string requestBody = await new StreamReader(req.Body).ReadToEndAsync();
            var data = JsonConvert.DeserializeObject<OrderModel>(requestBody);

            return new OkObjectResult(data.OrderId);
        }

        [FunctionName("UpdateOrder")]
        [OpenApiOperation(operationId: "OrderUpdate", tags: new[] { "order" }, Description = "Update order", Visibility = OpenApiVisibilityType.Important)]
        [OpenApiParameter(name: "orderId", In = ParameterLocation.Path, Required = true, Type = typeof(string), Description = "The **OrderId** parameter")]
        [OpenApiRequestBody("application/json", typeof(OrderModel), Description = "Order Id must be defined", Required = true)]
        [OpenApiSecurity("function_key", SecuritySchemeType.ApiKey, Name = "code", In = OpenApiSecurityLocationType.Header)]
        [OpenApiResponseWithBody(statusCode: HttpStatusCode.OK, contentType: "text/plain", bodyType: typeof(string), Description = "The OK response")]
        public async Task<IActionResult> UpdateOrder(
         [HttpTrigger(AuthorizationLevel.Function, "put", Route = "orders/{orderId}")] HttpRequest req, string orderId)
        {
            _logger.LogInformation("C# HTTP trigger function processed a request.");

            string requestBody = await new StreamReader(req.Body).ReadToEndAsync();
            var data = JsonConvert.DeserializeObject<OrderModel>(requestBody);

            return new OkObjectResult(data.OrderId);
        }
    }
}

