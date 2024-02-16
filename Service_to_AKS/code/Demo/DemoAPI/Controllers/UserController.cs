using Microsoft.AspNetCore.Mvc;
using Required.Interface;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace DemoAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly IClusterClient _client;

        public UserController(IClusterClient client)
        {
            _client = client;
        }

        [HttpPost("login/{userId}")]
        public IActionResult Login(string userId)
        {
            _client.GetGrain<IGrainUser>(userId);
            return Ok("Grain created");
        }

        [HttpGet("getForm/{userId}")]
        public async Task<IActionResult> GetForm(string userId)
        {
            var grain = _client.GetGrain<IGrainUser>(userId);
            var uiModel = await grain.GetFormsAsync();

            return Ok(uiModel);
        }

        // GET api/<UserController>/5
        [HttpGet("{id}")]
        public string Get(int id)
        {
            return "value";
        }
    }
}
