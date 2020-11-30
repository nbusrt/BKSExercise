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
    public class BaggageRegistrationServiceTests
    {
        private const string aircraftId = "aircraft1";
        private const string flightId = "flight1";
        private const string passengerId = "moshe";
        private const string otherPassengerId = "david";
        private const int maximumBaggageItemsPerPassenger = 3;
        private const int maximumBaggageWeightInKgPerPassenger = 20;

        private static readonly Flight flight = CreateFlight(passengerId, otherPassengerId);

        private const int otherPassengerBaggageWeight1 = 15;
        private const int otherPassengerBaggageWeight2 = 5;
        private const int otherPassengerBaggageTotalWeight = otherPassengerBaggageWeight1 + otherPassengerBaggageWeight2;
        private static readonly PassengerBaggage otherPassengerBaggage = CreatePassengerBaggage(otherPassengerBaggageWeight1, otherPassengerBaggageWeight2);

        private IFlightsRepository flightsRepository;
        private IAircraftsRepository aircraftsRepository;
        private IFlightBaggageRepository flightBaggageRepository;
        private BaggageRegistrationService baggageRegistrationService;

        [TestMethod]
        [ExpectedException(typeof(InvalidOperationException))]
        public void RegisterBaggageUnregisteredPassenger_ThrowsException()
        {
            // Prepare
            var flightWithoutPassenger = CreateFlight(otherPassengerId);

            PrepareFlightRepository(flightWithoutPassenger);
            PrepareAircraftsRepositoryStub(5000);
            PrepareFlightBaggageRepository();

            try
            {
                // Test
                CreateBaggageRegistrationService();
                baggageRegistrationService.RegisterBaggage(flightId, passengerId, 5);
            }
            finally
            {
                // Assert
                ValidatePassengerBaggageItems();
                ValidateFlightBaggageTotalWeight(otherPassengerBaggageTotalWeight);
            }
        }

        [TestMethod]
        [ExpectedException(typeof(InvalidOperationException))]
        public void RegisterBaggageAircraftBaggageIsFull_ThrowsException()
        {
            // Prepare
            PrepareFlightRepository(flight);
            PrepareAircraftsRepositoryStub(20);
            PrepareFlightBaggageRepository();

            try
            {
                // Test
                CreateBaggageRegistrationService();
                baggageRegistrationService.RegisterBaggage(flightId, passengerId, maximumBaggageWeightInKgPerPassenger + 5);
            }
            finally
            {
                // Assert
                ValidatePassengerBaggageItems();
                ValidateFlightBaggageTotalWeight(otherPassengerBaggageTotalWeight);
            }
        }

        [TestMethod]
        public void RegisterBaggageFirstBaggageItemValid_RegistersBaggage()
        {
            // Prepare
            const int newBaggageWeightInKg = 6;

            PrepareFlightRepository(flight);
            PrepareAircraftsRepositoryStub(5000);
            PrepareFlightBaggageRepository();

            // Test
            CreateBaggageRegistrationService();
            baggageRegistrationService.RegisterBaggage(flightId, passengerId, newBaggageWeightInKg);

            // Assert
            ValidatePassengerBaggageItems(newBaggageWeightInKg);
            ValidateFlightBaggageTotalWeight(otherPassengerBaggageTotalWeight + newBaggageWeightInKg);
        }

        [TestMethod]
        [ExpectedException(typeof(InvalidOperationException))]
        public void RegisterBaggageFirstBaggageItemAboveMaxWeight_ThrowsException()
        {
            // Prepare
            const int newBaggageWeightInKg = maximumBaggageWeightInKgPerPassenger + 5;

            PrepareFlightRepository(flight);
            PrepareAircraftsRepositoryStub(5000);
            PrepareFlightBaggageRepository();

            try
            {
                // Test
                CreateBaggageRegistrationService();
                baggageRegistrationService.RegisterBaggage(flightId, passengerId, newBaggageWeightInKg);
            }
            finally
            {
                // Assert
                ValidatePassengerBaggageItems();
                ValidateFlightBaggageTotalWeight(otherPassengerBaggageTotalWeight);
            }
        }

        [TestMethod]
        public void RegisterBaggageAdditionalBaggageItemValid_RegistersBaggage()
        {
            // Prepare
            const int existingBaggageWeight1 = 4;
            const int existingBaggageWeight2 = 7;
            const int newBaggageWeightInKg = 6;
            
            var existingPassengerBaggage = CreatePassengerBaggage(existingBaggageWeight1, existingBaggageWeight2);

            PrepareFlightRepository(flight);
            PrepareAircraftsRepositoryStub(5000);
            PrepareFlightBaggageRepository(existingPassengerBaggage);

            // Test
            CreateBaggageRegistrationService();
            baggageRegistrationService.RegisterBaggage(flightId, passengerId, newBaggageWeightInKg);

            // Assert
            ValidatePassengerBaggageItems(existingBaggageWeight1, existingBaggageWeight2, newBaggageWeightInKg);
            ValidateFlightBaggageTotalWeight(otherPassengerBaggageTotalWeight + existingBaggageWeight1  + 
                existingBaggageWeight2 + newBaggageWeightInKg);
        }

        [TestMethod]
        [ExpectedException(typeof(InvalidOperationException))]
        public void RegisterBaggageAdditionalBaggageItemAboveMaxWeight_ThrowsException()
        {
            // Prepare
            const int existingBaggageWeight1 = 4;
            const int existingBaggageWeight2 = 7;
            const int newBaggageWeightInKg = 10;

            var existingPassengerBaggage = CreatePassengerBaggage(existingBaggageWeight1, existingBaggageWeight2);

            PrepareFlightRepository(flight);
            PrepareAircraftsRepositoryStub(5000);
            PrepareFlightBaggageRepository(existingPassengerBaggage);
            
            try
            {
                // Test
                CreateBaggageRegistrationService();
                baggageRegistrationService.RegisterBaggage(flightId, passengerId, newBaggageWeightInKg);
            }
            finally
            {
                // Assert
                ValidatePassengerBaggageItems(existingBaggageWeight1, existingBaggageWeight2);
                ValidateFlightBaggageTotalWeight(otherPassengerBaggageTotalWeight + existingBaggageWeight1 +
                    existingBaggageWeight2);
            }
        }

        [TestMethod]
        [ExpectedException(typeof(InvalidOperationException))]
        public void RegisterBaggageAdditionalBaggageItemAboveMaxAmount_ThrowsException()
        {
            // Prepare
            const int existingBaggageWeight1 = 2;
            const int existingBaggageWeight2 = 3;
            const int existingBaggageWeight3 = 5;
            const int newBaggageWeightInKg = 3;

            var existingPassengerBaggage = CreatePassengerBaggage(existingBaggageWeight1, existingBaggageWeight2, existingBaggageWeight3);

            PrepareFlightRepository(flight);
            PrepareAircraftsRepositoryStub(5000);
            PrepareFlightBaggageRepository(existingPassengerBaggage);

            try
            {
                // Test
                CreateBaggageRegistrationService();
                baggageRegistrationService.RegisterBaggage(flightId, passengerId, newBaggageWeightInKg);
            }
            finally
            {
                // Assert
                ValidatePassengerBaggageItems(existingBaggageWeight1, existingBaggageWeight2, existingBaggageWeight3);
                ValidateFlightBaggageTotalWeight(otherPassengerBaggageTotalWeight + existingBaggageWeight1 +
                    existingBaggageWeight2 + existingBaggageWeight3);
            }
        }

        private static Flight CreateFlight(params string[] passengerIds)
        {
            return new Flight(passengerIds, aircraftId, maximumBaggageItemsPerPassenger, maximumBaggageWeightInKgPerPassenger);
        }

        private static PassengerBaggage CreatePassengerBaggage(params int[] baggageItemsWeightInKg)
        {
            var baggageItems = baggageItemsWeightInKg.Select(baggageItemWeightInKg => new BaggageItem(baggageItemWeightInKg));
            return new PassengerBaggage(baggageItems);
        }

        private void PrepareFlightRepository(Flight flight)
        {
            flightsRepository = new InMemoryFlightsRepository(new Dictionary<string, Flight> { { flightId, flight } });
        }

        private void PrepareAircraftsRepositoryStub(int baggageCapacityInKg)
        {
            var aircraft = new Aircraft(0, baggageCapacityInKg);
            aircraftsRepository = new AircraftsRepositoryStub(new Dictionary<string, Aircraft> { { aircraftId, aircraft } });
        }

        private void PrepareFlightBaggageRepository()
        {
            var flightBaggage = new FlightBaggage(new Dictionary<string, PassengerBaggage> { { otherPassengerId, otherPassengerBaggage } });
            flightBaggageRepository = new InMemoryFlightBaggageRepository(new Dictionary<string, FlightBaggage> { { flightId, flightBaggage } });
        }

        private void PrepareFlightBaggageRepository(PassengerBaggage passengerBaggage)
        {
            var flightBaggage = new FlightBaggage(new Dictionary<string, PassengerBaggage> { { otherPassengerId, otherPassengerBaggage }, { passengerId, passengerBaggage } });
            flightBaggageRepository = new InMemoryFlightBaggageRepository(new Dictionary<string, FlightBaggage> { { flightId, flightBaggage } });
        }

        private void CreateBaggageRegistrationService()
        {
            baggageRegistrationService = new BaggageRegistrationService(flightsRepository, aircraftsRepository, flightBaggageRepository);
        }

        private void ValidatePassengerBaggageItems(params int[] expectedBaggageItemsWeightInKg)
        {
            var passengerBaggage = flightBaggageRepository.GetFlightBaggage(flightId).GetPassengerBaggage(passengerId);
            
            var expectedBaggageItems = expectedBaggageItemsWeightInKg.Select(weightInKg => new BaggageItem(weightInKg));
            var actualBaggageItems = passengerBaggage.BaggageItems;

            CollectionAssert.AreEquivalent(expectedBaggageItems.ToArray(), actualBaggageItems.ToArray());
        }

        private void ValidateFlightBaggageTotalWeight(int expectedTotalWeightInKg)
        {
            var actualTotalWeightInKg = flightBaggageRepository.GetFlightBaggage(flightId).TotalWeightInKg;
            Assert.AreEqual(expectedTotalWeightInKg, actualTotalWeightInKg);
        }
    }
}
