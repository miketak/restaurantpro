using System.Data.Entity;
using RestaurantPro.Core.Domain;
using RestaurantPro.Core.Repositories;

namespace RestaurantPro.Infrastructure.Repositories
{
    public class RawMaterialsRepository : Repository<RawMaterial>, IRawMaterialsRepository
    {
        public RawMaterialsRepository(DbContext context) 
            : base(context)
        {
        }

    }
}