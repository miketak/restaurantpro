using System.Data.Entity;
using RestaurantPro.Core.Domain;
using RestaurantPro.Core.Repositories;

namespace RestaurantPro.Infrastructure.Repositories
{
    public class WorkCycleStatusRepository: Repository<WcStatus>, IWorkCycleStatusRepository
    {
        public WorkCycleStatusRepository(DbContext context) 
            : base(context)
        {
        }
    }
}