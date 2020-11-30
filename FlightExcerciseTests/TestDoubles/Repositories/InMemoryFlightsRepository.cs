using FlightExcercise.Models;
using FlightExcercise.Repositories;
using System.Collections.Generic;

namespace FlightExcerciseTests.TestDoubles.Repositories
{
    public class InMemoryFlightsRepository : IFlightsRepository
    {
        private readonly IDictionary<string, Flight> flights;

        public InMemoryFlightsRepository(IDictionary<string, Flight> flights)
        {
            this.flights = flights;
        }

        public Flight GetFlight(string flightId)
        {
            return flights[flightId];
        }

        public void SetFlight(string flightId, Flight flight)
        {
            flights[flightId] = flight;
        }
    }
}
