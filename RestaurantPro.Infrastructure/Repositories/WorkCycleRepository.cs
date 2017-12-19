using System.Data.Entity;
using RestaurantPro.Core.Domain;
using RestaurantPro.Core.Repositories;

namespace RestaurantPro.Infrastructure.Repositories
{
    public class WorkCycleRepository : Repository<WorkCycle>, IWorkCycleRepository
    {
        private DbContext _context;

        public WorkCycleRepository(DbContext context) 
            : base(context)
        {
            _context = context;
        }

        public void DeactivateWorkCycle(int id)
        {
            var a = SingleOrDefault(x => x.Id == id);
            a.Active = false;
            _context.SaveChanges();

        }
    }
}