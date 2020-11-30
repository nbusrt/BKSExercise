using FlightExcercise.Models;
using FlightExcercise.Repositories;
using System.Collections.Generic;

namespace FlightExcerciseTests.TestDoubles.Repositories
{
    public class InMemoryFlightBaggageRepository : IFlightBaggageRepository
    {
        private IDictionary<string, FlightBaggage> flightsBaggage;

        public InMemoryFlightBaggageRepository(IDictionary<string, FlightBaggage> flightsBaggage)
        {
            this.flightsBaggage = flightsBaggage;
        }

        public FlightBaggage GetFlightBaggage(string flightId)
        {
            return flightsBaggage[flightId];
        }

        public void SetFlightBaggage(string flightId, FlightBaggage flightBaggage)
        {
            flightsBaggage[flightId] = flightBaggage;
        }
    }
}
