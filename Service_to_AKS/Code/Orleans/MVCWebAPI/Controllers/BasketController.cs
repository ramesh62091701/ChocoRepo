using MVCWebAPI.Interface;
using Orleans;
using Orleans.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;

namespace MVCWebAPI.Controllers
{
    public class BasketController : ApiController
    {
        private IClusterClient grainFactory;
        public BasketController()
        {
           
        }
 
        // GET: api/Basket
        public IEnumerable<string> Get()
        {
            return new string[] { "value1", "value2" };
        }

        // GET: api/Basket/5
        public async Task<string> Get(int id)
        {
            grainFactory = await StartClientWithRetries();
            IUserGrain actor = GetActor("test");

            return await actor.GetBasket();
        }

        private IUserGrain GetActor(string userId)
        {
            var grain = grainFactory.GetGrain<IUserGrain>(0);
            return grain;
        }

        // POST: api/Basket
        public void Post([FromBody]string value)
        {
        }

        // PUT: api/Basket/5
        public void Put(int id, [FromBody]string value)
        {
        }

        // DELETE: api/Basket/5
        public void Delete(int id)
        {
        }

        private static async Task<IClusterClient> StartClientWithRetries()
        {
            Orleans.IClusterClient client;
            client = new Orleans.ClientBuilder()
                .UseLocalhostClustering()
                .Configure<ClusterOptions>(options =>
                {
                    options.ClusterId = "dev";
                    options.ServiceId = "OrleansBasics";
                })
                .Build();

            await client.Connect();
            Console.WriteLine("Client successfully connect to silo host");
            return client;
        }
    }
}
