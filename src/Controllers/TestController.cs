

using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

namespace Next_OWL.Controllers
{
    [Route("api/test")]
    [ApiController]
    public class TestController : ControllerBase
    {

        // GET api/values
        [HttpGet]
        public ActionResult<DateTime> Get()
        {
            return DateTime.Now;
        }

    }
}
