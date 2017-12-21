using System.Data.Entity.ModelConfiguration;
using RestaurantPro.Core.Domain;

namespace RestaurantPro.Infrastructure.EntityConfigurations
{
    public class SupplierConfiguration : EntityTypeConfiguration<Supplier>
    {
        public SupplierConfiguration()
        {
            HasMany(t => t.PurchaseOrderLines)
                .WithRequired(k => k.Supplier)
                .HasForeignKey(p => p.SupplierId);

            HasMany(t => t.WorkCycleLines)
                .WithRequired(k => k.Supplier)
                .HasForeignKey(p => p.SupplierId);
        }
    }
}