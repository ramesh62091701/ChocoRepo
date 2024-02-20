using Orleans;
using Shared.Interface;
using System;
using System.Threading.Tasks;


namespace OServer.Grains
{
    public class UserGrain : Grain, IUserGrain
    {
        private Object1 obj = new Object1();


        public async Task AddToBasket(Guid productId, int quantity)
        {
            throw new NotImplementedException();
        }

        public async Task ClearBasket()
        {
            throw new NotImplementedException();
        }

        public Task<string> GetBasket(string id)
        {
            obj.Id = obj.Id + 1;
            var session = SessionManager.CreateSession(id);
            session.Count = session.Count + 1;
            return Task.FromResult("success:" + obj.Id + ":" + session.Count);
        }

    }

    public class Object1
    {
        public string Id { get; set; }
    }
}
