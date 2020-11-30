using FlightExcercise.Services;
using Microsoft.AspNetCore.Mvc;
using System;

namespace FlightExcercise.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BaggageRegistrationController : ControllerBase
    {
        private readonly IBaggageRegistrationService baggageRegistrationService;

        // ToDo - Create controllers with dependencies on service layer, ideally through a DI container
        public BaggageRegistrationController(IBaggageRegistrationService baggageRegistrationService) : base()
        {
            this.baggageRegistrationService = baggageRegistrationService;
        }

        [HttpPost("{flightId}/{passengerId}")]
        public ActionResult Post(string flightId, string passengerId, [FromBody] int baggageWeightInKg)
        {
            try
            {
                baggageRegistrationService.RegisterBaggage(flightId, passengerId, baggageWeightInKg);
                return Ok();
            }
            catch (InvalidOperationException err)
            {
                return BadRequest(err);
            }
        }
    }
}
