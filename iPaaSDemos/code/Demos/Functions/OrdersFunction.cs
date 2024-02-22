using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Threading.Tasks;
using Functions.Interfaces;
using Functions.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.Azure.WebJobs.Extensions.OpenApi.Core.Attributes;
using Microsoft.Azure.WebJobs.Extensions.OpenApi.Core.Enums;
using Microsoft.Extensions.Logging;
using Microsoft.OpenApi.Models;
using Newtonsoft.Json;

namespace Functions
{
    public class OrdersFunction
    {
        private readonly ILogger<OrdersFunction> _logger;
        private readonly IOrderService _orderService;

        public OrdersFunction(ILogger<OrdersFunction> log, IOrderService orderService)
        {
            _logger = log;
            _orderService = orderService;
        }

        [FunctionName("GetOrders")]
        [OpenApiOperation(operationId: "GetOrders", tags: new[] { "order" })]
        [OpenApiResponseWithBody(statusCode: HttpStatusCode.OK, contentType: "application/json", bodyType: typeof(List<OrderModel>), Description = "The OK response")]
        public async Task<IActionResult> GetOrders(
            [HttpTrigger(AuthorizationLevel.Anonymous, "get", Route = "orders")] HttpRequest req)
        {
            _logger.LogInformation("C# HTTP trigger function processed a request to get orders.");
            return new OkObjectResult(await _orderService.GetOrders());
        }

        [FunctionName("GetOrder")]
        [OpenApiOperation(operationId: "GetOrder", tags: new[] { "order" })]
        [OpenApiParameter(name: "orderId", In = ParameterLocation.Path, Required = true, Type = typeof(int), Description = "The **OrderId** parameter")]
        [OpenApiResponseWithBody(statusCode: HttpStatusCode.OK, contentType: "application/json", bodyType: typeof(OrderModel), Description = "The OK response")]
        public async Task<IActionResult> GetOrder(
          [HttpTrigger(AuthorizationLevel.Anonymous, "get", Route = "orders/{orderId}")] HttpRequest req, int orderId)
        {
            _logger.LogInformation($"C# HTTP trigger function processed a request to get order {orderId}.");
            return new OkObjectResult(await _orderService.GetOrder(orderId));
        }

        [FunctionName("CreateOrder")]
        [OpenApiOperation(operationId: "OrderCreate", tags: new[] { "order" }, Description ="Create a new order", Visibility = OpenApiVisibilityType.Important)]
        [OpenApiRequestBody("application/json", typeof(OrderModel), Description = "Order Id must be defined", Required = true)]
        [OpenApiResponseWithBody(statusCode: HttpStatusCode.OK, contentType: "text/plain", bodyType: typeof(int), Description = "The OK response")]
        public async Task<IActionResult> CreateOrder(
           [HttpTrigger(AuthorizationLevel.Function, "post", Route = "orders")] HttpRequest req)
        {
            _logger.LogInformation("C# HTTP trigger function processed a request to create order.");

            string requestBody = await new StreamReader(req.Body).ReadToEndAsync();
            var data = JsonConvert.DeserializeObject<OrderModel>(requestBody);

            return new OkObjectResult(await _orderService.CreateOrder(data));
        }

        [FunctionName("UpdateOrder")]
        [OpenApiOperation(operationId: "OrderUpdate", tags: new[] { "order" }, Description = "Update order", Visibility = OpenApiVisibilityType.Important)]
        [OpenApiParameter(name: "orderId", In = ParameterLocation.Path, Required = true, Type = typeof(int), Description = "The **OrderId** parameter")]
        [OpenApiRequestBody("application/json", typeof(OrderModel), Description = "Order Id must be defined", Required = true)]
        [OpenApiResponseWithBody(statusCode: HttpStatusCode.OK, contentType: "text/plain", bodyType: typeof(bool), Description = "The OK response")]
        public async Task<IActionResult> UpdateOrder(
         [HttpTrigger(AuthorizationLevel.Function, "put", Route = "orders/{orderId}")] HttpRequest req, int orderId)
        {
            _logger.LogInformation("C# HTTP trigger function processed a request to update order.");

            string requestBody = await new StreamReader(req.Body).ReadToEndAsync();
            var data = JsonConvert.DeserializeObject<OrderModel>(requestBody);

            return new OkObjectResult(await _orderService.UpdateOrder(orderId, data));
        }

        [FunctionName("DeleteOrder")]
        [OpenApiOperation(operationId: "OrderDelete", tags: new[] { "order" }, Description = "Delete order", Visibility = OpenApiVisibilityType.Important)]
        [OpenApiParameter(name: "orderId", In = ParameterLocation.Path, Required = true, Type = typeof(int), Description = "The **OrderId** parameter")]
        [OpenApiResponseWithBody(statusCode: HttpStatusCode.OK, contentType: "text/plain", bodyType: typeof(bool), Description = "The OK response")]
        public async Task<IActionResult> DeleteOrder(
         [HttpTrigger(AuthorizationLevel.Function, "delete", Route = "orders/{orderId}")] HttpRequest req, int orderId)
        {
            _logger.LogInformation("C# HTTP trigger function processed a request to delete order.");

            string requestBody = await new StreamReader(req.Body).ReadToEndAsync();
            var data = JsonConvert.DeserializeObject<OrderModel>(requestBody);

            return new OkObjectResult(await _orderService.DeleteOrder(orderId));
        }
    }
}

