using Microsoft.AspNetCore.Mvc;
using SharedFiles.Interface;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace Orleans_Client.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BasketController : ControllerBase 
    {
        private readonly IClusterClient _grainFactory;

        public BasketController(IClusterClient grainFactory)
        {
            _grainFactory = grainFactory;
        }

        // GET: api/<BasketController>
        [HttpGet]
        public IEnumerable<string> Get()
        {
            return new string[] { "value1", "value2" };
        }

        // GET api/<BasketController>/5
        [HttpGet("{id}")]
        public async Task<string> Get(int id)
        {
            var userAgentGrain = _grainFactory.GetGrain<IUserGrain>(id);
            
            return await userAgentGrain.GetBasket(id.ToString());
        }

        // POST api/<BasketController>
        [HttpPost]
        public void Post([FromBody] string value)
        {
        }

        // PUT api/<BasketController>/5
        [HttpPut("{id}")]
        public void Put(int id, [FromBody] string value)
        {
        }

        // DELETE api/<BasketController>/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
        }
    }
}
