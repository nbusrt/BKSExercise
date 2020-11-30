using FlightExcercise.Models;
using FlightExcercise.Repositories;
using FlightExcercise.Services;
using FlightExcerciseTests.TestDoubles.Repositories;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;

namespace FlightExcerciseTests.Services
{
    [TestClass]
    public class PassengersRegistrationServiceTests
    {
        private const string aircraftId = "aircraft1";
        private const string flightId = "flight1";

        private const string newPassengerId = "moshe";
        private const string passengerId1 = "david";
        private const string passengerId2 = "shalom";

        private static readonly Flight flightWith2Passengers = CreateFlight(passengerId1, passengerId2);

        private IFlightsRepository flightsRepository;
        private IAircraftsRepository aircraftsRepository;
        private PassengersRegistrationService passengersRegistrationService;

        [TestMethod]
        [ExpectedException(typeof(Exception), AllowDerivedTypes = true)]
        public void RegisterPassengerUnknownFlight_ThrowsException()
        {
            // Prepare
            flightsRepository = new InMemoryFlightsRepository(new Dictionary<string, Flight>());
            PrepareAircraftsRepositoryStub(300);

            // Test
            CreatePassengersRegistrationService();
            passengersRegistrationService.RegisterPassenger(flightId, newPassengerId);

            // Assert is the exception
        }

        [TestMethod]
        public void RegisterPassengerEmptyFlight_RegistersPassenger()
        {
            // Prepare
            var emptyFlight = CreateFlight();

            PrepareFlightRepository(emptyFlight);
            PrepareAircraftsRepositoryStub(300);

            // Test
            CreatePassengersRegistrationService();
            passengersRegistrationService.RegisterPassenger(flightId, newPassengerId);

            // Assert
            ValidatePassengerWasRegistered();
            ValidateRegisteredPassengersAmount(1);
        }

        [TestMethod]
        public void RegiserPassengerUnderbookedFlight_RegistersPassenger()
        {
            // Prepare
            PrepareFlightRepository(flightWith2Passengers);
            PrepareAircraftsRepositoryStub(300);

            // Test
            CreatePassengersRegistrationService();
            passengersRegistrationService.RegisterPassenger(flightId, newPassengerId);

            // Assert
            ValidatePassengerWasRegistered();
            ValidateRegisteredPassengersAmount(3);
        }

        [TestMethod]
        [ExpectedException(typeof(InvalidOperationException))]
        public void RegiserPassengerOverbookedFlight_ThrowsException()
        {
            // Prepare
            PrepareFlightRepository(flightWith2Passengers);
            PrepareAircraftsRepositoryStub(2);

            try
            {
                // Test
                CreatePassengersRegistrationService();
                passengersRegistrationService.RegisterPassenger(flightId, newPassengerId);
            }
            finally
            {
                // Assert
                ValidatePassengerWasNotRegistered();
                ValidateRegisteredPassengersAmount(2);
            }
        }

        private static Flight CreateFlight(params string[] passengerIds)
        {
            return new Flight(passengerIds, aircraftId, 0, 0);
        }

        private void PrepareFlightRepository(Flight flight)
        {
            flightsRepository = new InMemoryFlightsRepository(new Dictionary<string, Flight> { { flightId, flight } });
        }

        private void PrepareAircraftsRepositoryStub(int passengersCapacity)
        {
            var aircraft = new Aircraft(passengersCapacity, 0);
            aircraftsRepository = new AircraftsRepositoryStub(new Dictionary<string, Aircraft> { { aircraftId, aircraft } });
        }

        private void CreatePassengersRegistrationService()
        {
            passengersRegistrationService = new PassengersRegistrationService(flightsRepository, aircraftsRepository);
        }

        private void ValidatePassengerWasRegistered()
        {
            var flight = GetFlightFromRepository();
            Assert.IsTrue(flight.RegisteredPassengers.Contains(newPassengerId));
        }

        private void ValidatePassengerWasNotRegistered()
        {
            var flight = GetFlightFromRepository();
            Assert.IsFalse(flight.RegisteredPassengers.Contains(newPassengerId));
        }

        private void ValidateRegisteredPassengersAmount(int expectedAmount)
        {
            var flight = GetFlightFromRepository();
            Assert.AreEqual(expectedAmount, flight.RegisteredPassengersAmount);
        }

        private Flight GetFlightFromRepository()
        {
            return flightsRepository.GetFlight(flightId);
        }
    }
}
