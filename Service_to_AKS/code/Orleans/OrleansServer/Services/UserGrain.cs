using Microsoft.Extensions.Logging;
using SharedFiles.Interface;

namespace MVCWebAPI.Grains
{
    public class UserGrain : Grain, IUserGrain
    {

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
            return Task.FromResult("success");
        }

    }
}
