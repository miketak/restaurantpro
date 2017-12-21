using System.Data.Entity.ModelConfiguration;
using RestaurantPro.Core.Domain;

namespace RestaurantPro.Infrastructure.EntityConfigurations
{
    public class PurchaseOrderConfiguration : EntityTypeConfiguration<PurchaseOrder>
    {
        public PurchaseOrderConfiguration()
        {
            HasMany(t => t.PurchaseOrderLines)
                .WithRequired(k => k.PurchaseOrder)
                .HasForeignKey(p => p.PurchaseOrderId);
        }
    }
}