using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Next_OWL.Services;

namespace Next_OWL.Controllers
{
    [Route("api/OwlSchedule")]
    [ApiController]
    public class OwlScheduleController : ControllerBase
    {
        private readonly OwlService owlService;

        public OwlScheduleController(OwlService owlService)
        {
            this.owlService = owlService;
        }

        // GET api/values
        [HttpGet]
        public async Task<ActionResult<dynamic>> Get()
        {
            return await this.owlService.GetNextGame();
        }

    }
}
