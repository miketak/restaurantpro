using System.Data.Entity;
using RestaurantPro.Core.Domain;
using RestaurantPro.Core.Repositories;

namespace RestaurantPro.Infrastructure.Repositories
{
    public class RawMaterialCategoryRepository : Repository<RawMaterialCategory>, IRawMaterialCategoryRepository
    {
        public RawMaterialCategoryRepository(DbContext context) 
            : base(context)
        {
        }
    }
}