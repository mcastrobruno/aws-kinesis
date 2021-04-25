using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using App.Api.Models;
using awskinesis.shared.Kinesis;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace App.Api.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class TripController : ControllerBase
    {
        private readonly IKinesisPublisher<TripModel> _publisher;
        private readonly ILogger<TripController> _logger;

        public TripController(ILogger<TripController> logger, IKinesisPublisher<TripModel> publisher)
        {
            _logger = logger;
            _publisher = publisher;
        }

        [HttpGet]
        public ActionResult Get()
        {
            return Ok();
        }

        [HttpPost] 
        public async Task<ActionResult> PostAsync([FromBody] TripModel trip)
        {
            await _publisher.PublishAsync(trip);
            return Ok();
        }
    }
}
