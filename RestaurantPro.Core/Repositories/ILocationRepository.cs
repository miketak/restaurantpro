using System.Collections.Generic;
using RestaurantPro.Core.Domain;

namespace RestaurantPro.Core.Repositories
{
    public interface ILocationRepository : IRepository<Location>
    {
        void AddOrUpdate(List<Location> locations);
        void FakeDelete(Location location);
    }
}