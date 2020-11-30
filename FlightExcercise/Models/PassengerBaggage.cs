using System.Collections.Generic;
using System.Linq;

namespace FlightExcercise.Models
{
    public class PassengerBaggage
    {
        public IEnumerable<BaggageItem> BaggageItems { get; }
        public int TotalWeightInKg => BaggageItems.Sum(baggageItem => baggageItem.WeightInKg);

        public PassengerBaggage(IEnumerable<BaggageItem> baggageItems)
        {
            BaggageItems = baggageItems;
        }
    }
}
