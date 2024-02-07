using Dapr.Actors;
using Dapr.Actors.Client;
using Dapr.Client;
using ECommerce.CheckoutService.Model;
using ECommerce.Service.Interface;
using ECommerce.Services.Model;
using Shared;
using System.Data;
using System.Threading;
using UserActor;


namespace Services
{
    public class CheckoutService : ICheckoutService
    {
        DaprClient _daprClient;

        public CheckoutService(DaprClient daprClient)
        {
            _daprClient = daprClient;
        }

        public async Task<CheckoutSummary> CheckoutAsync(string userId)
        {
            var result = new CheckoutSummary();
            result.Date = DateTime.UtcNow;
            result.Products = new List<CheckoutProduct>();

            //call user actor to get the basket
            IUserActorDapr userActor = GetUserActor(userId);
            BasketItem[] basket = await userActor.GetBasket();

            //get catalog client
            IProductCatalogService catalogService = GetProductCatalogService();

            //constuct CheckoutProduct items by calling to the catalog
            foreach (BasketItem basketLine in basket)
            {
                Product product = await catalogService.GetProductAsync(basketLine.ProductId);
                var checkoutProduct = new CheckoutProduct
                {
                    Product = product,
                    Price = product.Price,
                    Quantity = basketLine.Quantity
                };
                result.Products.Add(checkoutProduct);
            }

            //generate total price
            result.TotalPrice = result.Products.Sum(p => p.Price);

            //clear user basket
            await userActor.ClearBasket();

            await AddToHistoryAsync(result);

            return result;
        }

        public async Task<CheckoutSummary[]> GetOrderHitoryAsync(string userId)
        {
            var result = new List<CheckoutSummary>();

            var val = await _daprClient.GetStateAsync<Dictionary<DateTime, CheckoutSummary>>("history", "history");


            return result.ToArray();
        }

        private IUserActorDapr GetUserActor(string userId)
        {
            return ActorProxy.Create<IUserActorDapr>(new ActorId(userId), "UserActorDapr");
        }

        private IProductCatalogService GetProductCatalogService()
        {
            var result = _daprClient.CreateInvokeMethodRequest(HttpMethod.Get, "ProductCatalog", "");

            _daprClient.InvokeMethodAsync(result);

            return null;

            //var proxyFactory = new ServiceProxyFactory(
            //   c => new FabricTransportServiceRemotingClientFactory());

            //return proxyFactory.CreateServiceProxy<IProductCatalogService>(
            //   new Uri("fabric:/ECommerce/ECommerce.ProductCatalog"),
            //   new ServicePartitionKey(0));
        }

        private async Task AddToHistoryAsync(CheckoutSummary checkout)
        {
            var history = await _daprClient.GetStateAsync<Dictionary<DateTime, CheckoutSummary>>("history", "history");

            //using (ITransaction tx = StateManager.CreateTransaction())
            //{
            //    await history.AddAsync(tx, checkout.Date, checkout);

            //    await tx.CommitAsync();
            //}
        }


        //Below ServiceReplicaListener is from Service Fabric
        //protected override IEnumerable<ServiceReplicaListener> CreateServiceReplicaListeners()
        //{
        //    return new[]
        //    {
        //    new ServiceReplicaListener(context =>
        //       new FabricTransportServiceRemotingListener(context, this))
        // };
        //}
    }
}
