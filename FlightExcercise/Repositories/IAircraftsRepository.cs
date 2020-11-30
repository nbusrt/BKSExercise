using FlightExcercise.Models;

namespace FlightExcercise.Repositories
{
    public interface IAircraftsRepository
    {
        Aircraft GetAircraft(string aircraftId);
    }
}
