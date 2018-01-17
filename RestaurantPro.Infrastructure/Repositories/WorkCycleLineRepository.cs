using System.Data.Entity;
using RestaurantPro.Core.Domain;
using RestaurantPro.Core.Repositories;

namespace RestaurantPro.Infrastructure.Repositories
{
    public class WorkCycleLineRepository : Repository<WorkCycleLines>, IWorkCycleLineRepository
    {
        public WorkCycleLineRepository(DbContext context) : base(context)
        {
        }
    }
}