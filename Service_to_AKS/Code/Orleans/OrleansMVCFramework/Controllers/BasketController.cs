
using Orleans;
using Orleans.Configuration;
using OrleansMVCFramework;
using Shared.Interface;
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
           
            IUserGrain actor = GetActor(id.ToString());

            return await actor.GetBasket(id.ToString());
        }

        private IUserGrain GetActor(string userId)
        {
            var grain = WebApiApplication.grainFactory.GetGrain<IUserGrain>(Convert.ToInt16(userId));
            return grain;
        }

        // POST: api/Basket
        public void Post([FromBody] string value)
        {
        }

        // PUT: api/Basket/5
        public void Put(int id, [FromBody] string value)
        {
        }

        // DELETE: api/Basket/5
        public void Delete(int id)
        {
        }

    }
}
