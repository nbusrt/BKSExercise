using FlightExcercise.Models;
using FlightExcercise.Repositories;
using System;
using System.Collections.Generic;

namespace FlightExcerciseTests.TestDoubles.Repositories
{
    public class AircraftsRepositoryStub : IAircraftsRepository
    {
        private readonly IDictionary<string, Aircraft> aircrafts;

        public AircraftsRepositoryStub(IDictionary<string, Aircraft> aircrafts)
        {
            this.aircrafts = aircrafts;
        }

        public Aircraft GetAircraft(string aircraftId)
        {
            return aircrafts[aircraftId];
        }
    }
}
