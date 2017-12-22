using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.Infrastructure.Annotations;
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

            Property(u => u.PurchaseOrderNumber)
                .IsRequired()
                .HasMaxLength(10)
                .HasColumnAnnotation(
                    "Index",
                    new IndexAnnotation(new IndexAttribute("IX_PurchaseOrderNumber"){ IsUnique = true }));
        }
    }
}