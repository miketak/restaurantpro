using RestaurantPro.Core;
using RestaurantPro.Core.Repositories;
using RestaurantPro.Core.Services;
using RestaurantPro.Infrastructure.Repositories;
using RestaurantPro.Infrastructure.Services;

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
            UserAuthenticationService = new UserAuthenticationService(new UserRepository(_context));
            Suppliers = new SupplierRepository(_context);
            Statuses = new StatusRepository(_context);
            PurchaseOrders = new PurchaseOrderRepository(_context);
            RawMaterials = new RawMaterialsRepository(_context);
            RawMaterialCategories = new RawMaterialCategoryRepository(_context);
            Locations = new LocationRepository(_context);
            InventorySettings = new InventorySettingsRepository(_context);
            WorkCycleStatuses = new WorkCycleStatusRepository(_context);
            InventoryService = new InventoryService(this);
        }

        public IUserRepository Users { get; private set; }
        public IWorkCycleRepository WorkCycles { get; private set; }
        public IUserAuthenticationService UserAuthenticationService { get; private set; }
        public ISupplierRepository Suppliers { get; private set; }
        public IStatusRepository Statuses { get; private set; }
        public IPurchaseOrderRepository PurchaseOrders { get; private set; }
        public IRawMaterialsRepository RawMaterials { get; private set; }
        public IRawMaterialCategoryRepository RawMaterialCategories { get; private set; }
        public ILocationRepository Locations { get; private set; }
        public IInventorySettingsRepository InventorySettings { get; private set; }
        public IWorkCycleStatusRepository WorkCycleStatuses { get; private set; }
        public IInventoryService InventoryService { get; private set; }


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