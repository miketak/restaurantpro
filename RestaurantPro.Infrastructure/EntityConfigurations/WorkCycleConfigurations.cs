using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.Infrastructure.Annotations;
using System.Data.Entity.ModelConfiguration;
using RestaurantPro.Core.Domain;

namespace RestaurantPro.Infrastructure.EntityConfigurations
{
    public class WorkCycleConfigurations : EntityTypeConfiguration<WorkCycle>
    {
        public WorkCycleConfigurations()
        {
            Property(u => u.Name)
                .IsRequired()
                .HasMaxLength(24)
                .HasColumnAnnotation(IndexAnnotation.AnnotationName,
                    new IndexAnnotation(new IndexAttribute("IX_WorkCycleName", 1) {IsUnique = true}));

            HasMany(t => t.PurchaseOrders)
                .WithRequired(k => k.WorkCycle)
                .HasForeignKey(p => p.WorkCycleId)
                .WillCascadeOnDelete(false);

            HasMany(t => t.WorkCycleLines)
                .WithRequired(k => k.WorkCycle)
                .HasForeignKey(p => p.WorkCycleId);
        }
    }
}