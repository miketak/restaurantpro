using System.Data.Entity.ModelConfiguration;
using RestaurantPro.Core.Domain;

namespace RestaurantPro.Infrastructure.EntityConfigurations
{
    public class PoStatusConfiguration : EntityTypeConfiguration<PoStatus>
    {
        public PoStatusConfiguration()
        {
            HasKey(t => t.Status);
            Property(u => u.Status).HasMaxLength(15);

            HasMany(t => t.PurchaseOrders)
                .WithRequired(k => k.Status)
                .HasForeignKey(p => p.StatusId);
        }
    }
}