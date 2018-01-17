using System.Data.Entity;
using RestaurantPro.Core.Domain;
using RestaurantPro.Infrastructure.EntityConfigurations;

namespace RestaurantPro.Infrastructure
{

    public class RestProContext : DbContext
    {
        public RestProContext()
            : base("RestProContext")
        {
            Configuration.LazyLoadingEnabled = false;
        }

        public virtual DbSet<User> Users { get; set; }

        public virtual DbSet<WorkCycle> WorkCycles { get; set; }

        public virtual DbSet<Supplier> Suppliers { get; set; }

        public virtual DbSet<RawMaterial> RawMaterials { get; set; }

        public virtual DbSet<RawMaterialCategory> RawMaterialCategories { get; set; }

        public virtual DbSet<PoStatus> PurchaseOrderStatuses { get; set; }

        public virtual DbSet<RawMaterialCatalog> RawMaterialCatalog { get; set; }

        public virtual DbSet<PurchaseOrder> PurchaseOrders { get; set; }

        public virtual DbSet<PurchaseOrderLine> PurchaseOrderLines { get; set; }

        public virtual DbSet<Location> Locations { get; set; }

        public virtual DbSet<WorkCycleLines> WorkCycleLines { get; set; }

        public virtual DbSet<InventorySettings> InventorySettings { get; set; }

        public virtual DbSet<WcStatus> WorkCycleStatuses { get; set; }

        public virtual DbSet<WorkCycleAdjustments> WorkCycleAdjustments { get; set; }

        public virtual DbSet<WorkCycleTransaction> WorkCycleTransactions { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Configurations.Add(new PurchaseOrderLinesConfiguration());
            modelBuilder.Configurations.Add(new UserConfiguration());
            modelBuilder.Configurations.Add(new WorkCycleConfigurations());
            modelBuilder.Configurations.Add(new PoStatusConfiguration());
            modelBuilder.Configurations.Add(new PurchaseOrderConfiguration());
            modelBuilder.Configurations.Add(new RawMaterialCatalogConfiguration());
            modelBuilder.Configurations.Add(new SupplierConfiguration());
            modelBuilder.Configurations.Add(new WorkCycleLinesConfiguration());
            modelBuilder.Configurations.Add(new WcStatusConfiguration());
            modelBuilder.Configurations.Add(new WorkCycleAdjustmentsConfiguration());
            modelBuilder.Configurations.Add(new PurchaseOrderTransactionConfiguration());
        }
 
    }
        
}