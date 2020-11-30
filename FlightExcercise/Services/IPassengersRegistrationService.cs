namespace FlightExcercise.Services
{
    public interface IPassengersRegistrationService
    {
        void RegisterPassenger(string flightId, string passengerId);
    }
}
