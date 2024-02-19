using Microsoft.Extensions.Logging;
using SharedFiles.Interface;

namespace MVCWebAPI.Grains
{
    public class UserGrain : Grain, IUserGrain
    {
        private Object1 obj = new Object1();

        private static Object1 StaticObj = new Object1();
        private readonly ILogger logger;

        public UserGrain(ILogger<UserGrain> logger)
        {
            this.logger = logger;
        }

        public async Task AddToBasket(Guid productId, int quantity)
        {
           throw new NotImplementedException();
        }

        public async Task ClearBasket()
        {
            throw new NotImplementedException();
        }

        public Task<string> GetBasket()
        {
            obj.Id = obj.Id + 1;
            StaticObj.Id = StaticObj.Id + 1;
            return Task.FromResult("success:" + obj.Id  + ":" + StaticObj.Id);
        }

    }

    public class Object1
    {
        public string Id { get; set; }
    }
}
