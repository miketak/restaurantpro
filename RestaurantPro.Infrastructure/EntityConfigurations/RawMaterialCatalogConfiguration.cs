using System.Data.Entity.ModelConfiguration;
using RestaurantPro.Core.Domain;

namespace RestaurantPro.Infrastructure.EntityConfigurations
{
    public class RawMaterialCatalogConfiguration : EntityTypeConfiguration<RawMaterialCatalog>
    {
        public RawMaterialCatalogConfiguration()
        {
            ToTable("RawMaterialCatalog");
        }
        
    }
}