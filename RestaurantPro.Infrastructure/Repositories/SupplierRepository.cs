using System.Data.Entity;
using RestaurantPro.Core.Domain;
using RestaurantPro.Core.Repositories;

namespace RestaurantPro.Infrastructure.Repositories
{
    public class SupplierRepository : Repository<Supplier>, ISupplierRepository
    {
        public SupplierRepository(DbContext context) : base(context)
        {
        }
    }
}