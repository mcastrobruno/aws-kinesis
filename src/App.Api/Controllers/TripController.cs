using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using App.Api.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace App.Api.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class TripController : ControllerBase
    {

        private readonly ILogger<TripController> _logger;

        public TripController(ILogger<TripController> logger)
        {
            _logger = logger;
        }

        [HttpGet]
        public ActionResult Get()
        {
            return Ok();
        }

        [HttpPost] 
        public ActionResult Post([FromBody] TripModel trip)
        {



            throw new NotImplementedException();
        }
    }
}
