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

        IStatusRepository Statuses { get; }

        IPurchaseOrderRepository PurchaseOrders { get; }

        IRawMaterialsRepository RawMaterials { get; }

        IRawMaterialCategoryRepository RawMaterialCategories { get; }
        ILocationRepository Locations { get; }

        IInventorySettingsRepository InventorySettings { get; }

        IWorkCycleStatusRepository WorkCycleStatuses { get; }

        int Complete();
    }
}