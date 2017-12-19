using RestaurantPro.Core.Repositories;

namespace RestaurantPro.Core
{
    public interface IUnitOfWork
    {
        IUserRepository Users { get; }

        IWorkCycleRepository WorkCycles { get; }

        int Complete();
    }
}