using FlightExcercise.Services;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace FlightExcercise.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PassengerRegistrationController : ControllerBase
    {
        private readonly IPassengersRegistrationService passengersRegistrationService;

        // ToDo - Create controllers with dependencies on service layer, ideally through a DI container
        public PassengerRegistrationController(IPassengersRegistrationService passengersRegistrationService) : base()
        {
            this.passengersRegistrationService = passengersRegistrationService;
        }

        [HttpPost("{flightId}/{passengerId}")]
        public ActionResult Post(string flightId, string passengerId)
        {
            try
            {
                passengersRegistrationService.RegisterPassenger(flightId, passengerId);
                return Ok();
            }
            catch (InvalidOperationException err)
            {
                return BadRequest(err);
            }
        }
    }
}
