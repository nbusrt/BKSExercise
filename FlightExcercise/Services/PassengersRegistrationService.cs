using FlightExcercise.Models;
using FlightExcercise.Repositories;
using System;
using System.Linq;

namespace FlightExcercise.Services
{
    public class PassengersRegistrationService
    {
        private readonly IFlightsRepository flightsRepository;
        private readonly IAircraftsRepository aircraftsRepository;

        public PassengersRegistrationService(IFlightsRepository flightsRepository, IAircraftsRepository aircraftsRepository)
        {
            this.flightsRepository = flightsRepository;
            this.aircraftsRepository = aircraftsRepository;
        }

        public void RegisterPassenger(string flightId, string passengerId)
        {
            var flight = flightsRepository.GetFlight(flightId);

            ValidatePassengersCapacity(flight);
            RegisterValidatedPassenger(flight, flightId, passengerId);
        }

        private void ValidatePassengersCapacity(Flight flight)
        {
            var aircraft = GetFlightAircraft(flight);

            var passengersCapacity = aircraft.PassengersCapacity;
            var currentPassengersAmount = flight.RegisteredPassengersAmount;

            if (currentPassengersAmount >= passengersCapacity) throw new InvalidOperationException("Cannot register passenger since it will overbook the flight.");
        }

        private Aircraft GetFlightAircraft(Flight flight)
        {
            var aircraftId = flight.AircraftId;
            return aircraftsRepository.GetAircraft(aircraftId);
        }

        private void RegisterValidatedPassenger(Flight flight, string flightId, string passengerId)
        {
            var newFlight = AddPassengerToFlight(flight, passengerId);
            flightsRepository.SetFlight(flightId, newFlight);
        }

        private Flight AddPassengerToFlight(Flight flight, string passengerId)
        {
            var newRegisteredPassengers = flight.RegisteredPassengers.Append(passengerId);
            var newFlight = new Flight(newRegisteredPassengers, flight.AircraftId, 
                flight.MaximumBaggageItemsPerPassenger, flight.MaximumBaggageWeightInKgPerPassenger);

            return newFlight;
        }
    }
}
