using System.Collections.Generic;
using RestaurantPro.Core.Domain;

namespace RestaurantPro.Core.Repositories
{
    public interface ILocationRepository : IRepository<Location>
    {

        IEnumerable<Location> GetLocations();
        void AddOrUpdate(List<Location> locations);
        void FakeDelete(Location location);
    }
}