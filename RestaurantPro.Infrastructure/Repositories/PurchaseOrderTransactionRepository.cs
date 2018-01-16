using System.Data.Entity;
using RestaurantPro.Core.Domain;
using RestaurantPro.Core.Repositories;

namespace RestaurantPro.Infrastructure.Repositories
{
    public class PurchaseOrderTransactionRepository : Repository<PurchaseOrderTransaction>, IPurchaseOrderTransactionRepository
    {
        public PurchaseOrderTransactionRepository(DbContext context) 
            : base(context)
        {
        }
    }
}