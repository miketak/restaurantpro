using System.Data.Entity.ModelConfiguration;
using RestaurantPro.Core.Domain;

namespace RestaurantPro.Infrastructure.EntityConfigurations
{
    public class PurchaseOrderTransactionConfiguration : EntityTypeConfiguration<PurchaseOrderTransaction>
    {
        public PurchaseOrderTransactionConfiguration()
        {
            Ignore(p => p.LocationId);

        }
        
    }
}