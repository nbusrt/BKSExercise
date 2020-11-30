using FlightExcercise.Models;
using FlightExcercise.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;

namespace FlightExcercise.Services
{
    public class BaggageRegistrationService : IBaggageRegistrationService
    {
        private readonly IFlightsRepository flightsRepository;
        private readonly IAircraftsRepository aircraftsRepository;
        private readonly IFlightBaggageRepository flightBaggageRepository;

        public BaggageRegistrationService(IFlightsRepository flightsRepository, IAircraftsRepository aircraftsRepository, IFlightBaggageRepository flightBaggageRepository)
        {
            this.flightsRepository = flightsRepository;
            this.aircraftsRepository = aircraftsRepository;
            this.flightBaggageRepository = flightBaggageRepository;
        }

        public void RegisterBaggage(string flightId, string passengerId, int baggageWeightInKg)
        {
            var flight = flightsRepository.GetFlight(flightId);
            var flightBaggage = flightBaggageRepository.GetFlightBaggage(flightId);

            ValidatePassengerBaggage(flight, flightBaggage, passengerId, baggageWeightInKg);
            RegisterValidatedBaggage(flightBaggage, flightId, passengerId, baggageWeightInKg);
        }

        private void ValidatePassengerBaggage(Flight flight, FlightBaggage flightBaggage, string passengerId, int baggageWeightInKg)
        {
            ValidatePassengerIsRegistered(flight, passengerId);
            ValidateAircraftCapacity(flight, flightBaggage, baggageWeightInKg);
            ValidatePassengerBaggageItems(flight, flightBaggage, passengerId, baggageWeightInKg);
        }

        private void ValidatePassengerIsRegistered(Flight flight, string passengerId)
        {
            if (!flight.RegisteredPassengers.Contains(passengerId)) throw new InvalidOperationException("Passenger is not registered to flight.");
        }

        private void ValidateAircraftCapacity(Flight flight, FlightBaggage flightBaggage, int baggageWeightInKg)
        {
            var aircraftBaggageCapacityInKg = GetAircraftBaggageCapacityInKg(flight);
            
            var currentFlightBaggageInKg = flightBaggage.TotalWeightInKg;
            var newFlightBaggageInKg = currentFlightBaggageInKg + baggageWeightInKg;

            if (newFlightBaggageInKg > aircraftBaggageCapacityInKg) throw new InvalidOperationException("Baggage weight will exceed aircraft baggage capacity.");
        }

        private void ValidatePassengerBaggageItems(Flight flight, FlightBaggage flightBaggage, string passengerId, int baggageWeightInKg)
        {
            var passengerBaggage = flightBaggage.GetPassengerBaggage(passengerId);

            ValidatePassengerBaggageItemsAmount(flight, passengerBaggage);
            ValidatePassengerBaggageItemsWeight(flight, passengerBaggage, baggageWeightInKg);
        }

        private void ValidatePassengerBaggageItemsAmount(Flight flight, PassengerBaggage passengerBaggage)
        {
            var previousPassengerBaggageItemsAmount = passengerBaggage.BaggageItems.Count();
            var newPassengerBaggageItemsAmount = previousPassengerBaggageItemsAmount + 1;

            var maximumBaggageItemsPerPassenger = flight.MaximumBaggageItemsPerPassenger;

            if (newPassengerBaggageItemsAmount > maximumBaggageItemsPerPassenger) throw new InvalidOperationException("Passenger already registered the maximum allowed amount of baggage items.");
        }

        private void ValidatePassengerBaggageItemsWeight(Flight flight, PassengerBaggage passengerBaggage, int baggageWeightInKg)
        {
            var previousPassengerBaggageTotalWeight = passengerBaggage.TotalWeightInKg;
            var newPassengerBaggageTotalWeight = previousPassengerBaggageTotalWeight + baggageWeightInKg;

            var maximumBaggageWeightInKgPerPassenger = flight.MaximumBaggageWeightInKgPerPassenger;

            if (newPassengerBaggageTotalWeight > maximumBaggageWeightInKgPerPassenger) throw new InvalidOperationException("Baggage weight will exceed maximum allowed baggage weight per passenger.");
        }

        private int GetAircraftBaggageCapacityInKg(Flight flight)
        {
            var aircraftId = flight.AircraftId;
            var aircraft = aircraftsRepository.GetAircraft(aircraftId);

            return aircraft.BaggageCapacityInKg;
        }

        private void RegisterValidatedBaggage(FlightBaggage previousFlightBaggage, string flightId, string passengerId, int baggageWeightInKg)
        {
            var newPassengerBaggage = AddBaggageItemToPassengerBaggage(previousFlightBaggage, passengerId, baggageWeightInKg);
            var newFlightBaggage = UpdatePassengerBaggageInFlightBaggage(previousFlightBaggage, newPassengerBaggage, passengerId);
            
            flightBaggageRepository.SetFlightBaggage(flightId, newFlightBaggage);
        }

        private PassengerBaggage AddBaggageItemToPassengerBaggage(FlightBaggage previousFlightBaggage, string passengerId, int baggageWeightInKg)
        {
            var previousPassengerBaggage = previousFlightBaggage.GetPassengerBaggage(passengerId);
            var previousPassengerBaggageItems = previousPassengerBaggage.BaggageItems;

            var newBaggageItem = new BaggageItem(baggageWeightInKg);
            var newPassengerBaggageItems = previousPassengerBaggageItems.Append(newBaggageItem);
            var newPassengerBaggage = new PassengerBaggage(newPassengerBaggageItems);

            return newPassengerBaggage;
        }

        private FlightBaggage UpdatePassengerBaggageInFlightBaggage(FlightBaggage previousFlightBaggage, PassengerBaggage newPassengerBaggage, string passengerId)
        {
            var previousPassengersBaggage = previousFlightBaggage.PassengersBaggage;

            var newPassengersBaggage = new Dictionary<string, PassengerBaggage>(previousPassengersBaggage);
            newPassengersBaggage[passengerId] = newPassengerBaggage;

            var newFlightBaggage = new FlightBaggage(newPassengersBaggage);
            return newFlightBaggage;
        }
    }
}
