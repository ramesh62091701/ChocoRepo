using System.Threading.Tasks;
using System.Linq;
using System.Web.Http;
using Akka.Actor;
using AkkaDemo2.Actor;



namespace AkkaDemo2.Controllers
{
    [RoutePrefix("api/message")]
    public class MessageController : ApiController
    {
        public static ActorSystem  system = ActorSystem.Create("MyActorSystem");

        private readonly IActorRef mainActor;

        public MessageController() {
           
            mainActor = system.ActorOf(Props.Create(() => new MainActor()));
        }

        [HttpGet]
        [Route("")]
        public async Task<IHttpActionResult> GetAsync()
        {
            await mainActor.Ask("Get");
            return Ok();
        }

        [HttpPost]
        [Route("add")]
        public async Task<IHttpActionResult> AddAsync([FromBody] string message)
        {
            mainActor.Tell(new AddMessageCommand(message));
            return Ok();
        }

        [HttpDelete]
        [Route("delete")]
        public async Task<IHttpActionResult> DeleteAsync()
        {
            mainActor.Tell("Clear");
            return Ok();
        }

    }
}
