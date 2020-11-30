using FlightExcercise.Models;

namespace FlightExcercise.Repositories
{
    public interface IFlightBaggageRepository
    {
        FlightBaggage GetFlightBaggage(string flightId);
        void SetFlightBaggage(string flightId, FlightBaggage flightBaggage);
    }
}
