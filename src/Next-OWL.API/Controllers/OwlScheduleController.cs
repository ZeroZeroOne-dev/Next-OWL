using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Next_OWL.Models.Output;
using Next_OWL.Services;

namespace Next_OWL.Controllers
{
    [Route("api/schedule")]
    [ApiController]
    public class OwlScheduleController : ControllerBase
    {
        private readonly IOwlService owlService;

        public OwlScheduleController(IOwlService owlService)
        {
            this.owlService = owlService;
        }

        [HttpGet("next")]
        public async Task<Game> GetNext()
        {
            return await this.owlService.GetNext();
        }

        [HttpGet]
        public async Task<ActionResult> Get([FromQuery] int count = 10)
        {
            if (count < 1 || count > 10)
            {
                return BadRequest("invalid count");
            }

            var games = await this.owlService.GetFuture(count);

            return (ActionResult)Ok(games);
        }
    }
}
