using System.Data.Entity.ModelConfiguration;
using RestaurantPro.Core.Domain;

namespace RestaurantPro.Infrastructure.EntityConfigurations
{
    public class RawMaterialConfiguration : EntityTypeConfiguration<RawMaterial>
    {
        public RawMaterialConfiguration()
        {
            HasMany(t => t.PurchaseOrderLines)
               .WithRequired(k => k.RawMaterial)
               .HasForeignKey(p => p.RawMaterialId);

            HasMany(t => t.WorkCycleLines)
                .WithRequired(k => k.RawMaterial)
                .HasForeignKey(p => p.RawMaterialId);
        }
    }
}