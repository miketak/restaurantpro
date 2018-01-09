using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using RestaurantPro.Core.Domain;
using RestaurantPro.Core.Repositories;

namespace RestaurantPro.Infrastructure.Repositories
{
    public class LocationRepository : Repository<Location>, ILocationRepository
    {
        private readonly RestProContext _context;

        public LocationRepository(DbContext context) 
            : base(context)
        {
            _context = (RestProContext) context;
        }

        public IEnumerable<Location> GetLocations()
        {
            return _context
                .Locations
                .Where(a => a.Active).ToList();
        }

        public void AddOrUpdate(List<Location> locations)
        {
            foreach (var location in locations)
            {
                var locationInDb = _context.Locations
                    .SingleOrDefault(c => c.LocationId == location.LocationId);

                if (locationInDb == null)
                    AddLocation(location);
                else
                    UpdateLocation(locationInDb, location);
                _context.SaveChanges();
            }
        }

        public void FakeDelete(Location location)
        {
            var locationInDb = _context.Locations
                .SingleOrDefault(s => s.LocationId == location.LocationId);

            if (locationInDb == null)
                throw new ApplicationException("Illegal Operation");

            location.Active = false;
            UpdateLocation(locationInDb, location);
            _context.SaveChanges();
        }

        private void AddLocation(Location location)
        {
            _context.Locations.Add(location);
        }

        private void UpdateLocation(Location locationInDb, Location location)
        {
            locationInDb.LocationId = location.LocationId;
            locationInDb.Active = location.Active;
        }
    }
}