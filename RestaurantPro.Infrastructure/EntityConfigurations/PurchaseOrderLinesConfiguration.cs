using System.Data.Entity.ModelConfiguration;
using RestaurantPro.Core.Domain;

namespace RestaurantPro.Infrastructure.EntityConfigurations
{
    public class PurchaseOrderLinesConfiguration : EntityTypeConfiguration<PurchaseOrderLines>
    {
        public PurchaseOrderLinesConfiguration()
        {
            HasKey(table => new
            {
                table.PurchaseOrderId,
                table.SupplierId,
                table.RawMaterialId
            });
        }
    }
}