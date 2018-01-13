using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.Infrastructure.Annotations;
using System.Data.Entity.ModelConfiguration;
using RestaurantPro.Core.Domain;

namespace RestaurantPro.Infrastructure.EntityConfigurations
{
    public class UserConfiguration : EntityTypeConfiguration<User>
    {
        public UserConfiguration()
        {
            Property(u => u.Username)
                .IsRequired()
                .HasMaxLength(10)
                .HasColumnAnnotation(IndexAnnotation.AnnotationName,
                    new IndexAnnotation(new IndexAttribute("IX_Username", 1) {IsUnique = true}));

            Property(u => u.Email)
                .IsRequired()
                .HasMaxLength(64)
                .HasColumnAnnotation(IndexAnnotation.AnnotationName,
                    new IndexAnnotation(new IndexAttribute("IX_Email", 1) {IsUnique = true}));

            HasMany(t => t.PurchaseOrders)
                .WithRequired(k => k.User)
                .HasForeignKey(p => p.CreatedBy);

            HasMany(u => u.WorkCycleAdjustments)
                .WithRequired(k => k.User)
                .HasForeignKey(p => p.CreatedBy)
                .WillCascadeOnDelete(false);

            HasMany(u => u.WorkCycleTransactions)
                .WithRequired(k => k.User)
                .HasForeignKey(p => p.CreatedBy)
                .WillCascadeOnDelete(false);
        }
    }
}