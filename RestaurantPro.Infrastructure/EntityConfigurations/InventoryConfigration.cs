using System.Data.Entity.ModelConfiguration;
using RestaurantPro.Core.Domain;

namespace RestaurantPro.Infrastructure.EntityConfigurations
{
    public class InventoryConfigration : EntityTypeConfiguration<Inventory>
    {
        public InventoryConfigration()
        {
            ToTable("Inventory");
        }
        
    }
}