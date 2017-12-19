using RestaurantPro.Core;
using RestaurantPro.Core.Repositories;
using RestaurantPro.Infrastructure.Repositories;

namespace RestaurantPro.Infrastructure
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly RestProContext _context;

        public UnitOfWork(RestProContext context)
        {
            _context = context;
            Users = new UserRepository(_context);
            WorkCycles = new WorkCycleRepository(_context);
        }

        public IUserRepository Users { get; private set; }
        public IWorkCycleRepository WorkCycles { get; private set; }


        public int Complete()
        {
            return _context.SaveChanges();
        }

        public void Dispose()
        {
            _context.Dispose();
        }
        
    }
}