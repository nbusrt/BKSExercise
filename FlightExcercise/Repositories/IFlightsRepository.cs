using FlightExcercise.Models;

namespace FlightExcercise.Repositories
{
    public interface IFlightsRepository
    {
        Flight GetFlight(string flightId);

        void SetFlight(string flightId, Flight flight);
    }
}
