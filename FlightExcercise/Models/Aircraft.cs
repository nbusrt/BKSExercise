namespace FlightExcercise.Models
{
    public class Aircraft
    {
        public int PassengersCapacity { get; }
        public int BaggageCapacityInKg { get; }

        public Aircraft(int passengersCapacity, int baggageCapacityInKg)
        {
            PassengersCapacity = passengersCapacity;
            BaggageCapacityInKg = baggageCapacityInKg;
        }
    }
}
