namespace FlightExcercise.Services
{
    public interface IBaggageRegistrationService
    {
        void RegisterBaggage(string flightId, string passengerId, int baggageWeightInKg);
    }
}
