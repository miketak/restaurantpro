using System.Data.Entity;
using RestaurantPro.Core.Domain;
using RestaurantPro.Core.Repositories;

namespace RestaurantPro.Infrastructure.Repositories
{
    public class WorkCycleTransactionRepository : Repository<WorkCycleTransaction>, IWorkCycleTransactionRepository
    {
        public WorkCycleTransactionRepository(DbContext context) : base(context)
        {
        }
    }
}