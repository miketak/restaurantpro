using System.Data.Entity.ModelConfiguration;
using RestaurantPro.Core.Domain;

namespace RestaurantPro.Infrastructure.EntityConfigurations
{
    public class WcStatusConfiguration : EntityTypeConfiguration<WcStatus>
    {
        public WcStatusConfiguration()
        {
            HasKey(t => t.Status);
            Property(u => u.Status).HasMaxLength(15);

            HasMany(t => t.WorkCycle)
                .WithRequired(k => k.Status)
                .HasForeignKey(p => p.StatusId);
        }
        
    }
}