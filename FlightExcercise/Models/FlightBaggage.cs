using System.Collections.Generic;
using System.Linq;

namespace FlightExcercise.Models
{
    public class FlightBaggage
    {
        public IDictionary<string, PassengerBaggage> PassengersBaggage { get; }
        public int TotalWeightInKg => PassengersBaggage.Values.Sum(passengersBaggage => passengersBaggage.TotalWeightInKg);

        public FlightBaggage(IDictionary<string, PassengerBaggage> passengersBaggage)
        {
            PassengersBaggage = passengersBaggage;
        }

        public PassengerBaggage GetPassengerBaggage(string passengerId)
        {
            if (PassengersBaggage.ContainsKey(passengerId)) return PassengersBaggage[passengerId];
            else return new PassengerBaggage(Enumerable.Empty<BaggageItem>());
        }
    }
}
