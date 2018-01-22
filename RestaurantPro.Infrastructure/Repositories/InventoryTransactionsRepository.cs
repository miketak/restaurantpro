using System.Data.Entity;
using RestaurantPro.Core.Domain;
using RestaurantPro.Core.Repositories;

namespace RestaurantPro.Infrastructure.Repositories
{
    public class InventoryTransactionsRepository : Repository<InventoryTransaction>, IInventoryTrasactionsRepository
    {
        public InventoryTransactionsRepository(DbContext context) 
            : base(context)
        {
        }
    }
}