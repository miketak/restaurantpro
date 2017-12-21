using RestaurantPro.Core.Repositories;
using RestaurantPro.Core.Services;

namespace RestaurantPro.Core
{
    public interface IUnitOfWork
    {
        IUserRepository Users { get; }

        IWorkCycleRepository WorkCycles { get; }

        ISupplierRepository Suppliers { get; }

        IUserAuthenticationService UserAuthenticationService { get; }

        int Complete();
    }
}