using System.Collections.Generic;
using System.Linq;

namespace FlightExcercise.Models
{
    public class Flight
    {
        public IEnumerable<string> RegisteredPassengers { get; }
        public int RegisteredPassengersAmount => RegisteredPassengers.Count();
        public string AircraftId { get; }
        public int MaximumBaggageItemsPerPassenger { get; }
        public int MaximumBaggageWeightInKgPerPassenger { get; }

        public Flight(IEnumerable<string> registeredPassengers, string aircraftId, int maximumBaggageItemsPerPassenger, int maximumBaggageWeightInKgPerPassenger)
        {
            RegisteredPassengers = registeredPassengers;
            AircraftId = aircraftId;
            MaximumBaggageItemsPerPassenger = maximumBaggageItemsPerPassenger;
            MaximumBaggageWeightInKgPerPassenger = maximumBaggageWeightInKgPerPassenger;
        }
    }
}
