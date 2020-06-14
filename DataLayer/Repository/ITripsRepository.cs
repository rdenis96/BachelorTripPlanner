using DataLayer.Enums;
using DataLayer.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace DataLayer.Repository
{
    public interface ITripsRepository : IBasicRepository<Trip>
    {
        List<Trip> GetByType(TripType type);
    }
}