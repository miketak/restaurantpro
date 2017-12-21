using System.Data.Entity.ModelConfiguration;
using RestaurantPro.Core.Domain;

namespace RestaurantPro.Infrastructure.EntityConfigurations
{
    public class WorkCycleLinesConfiguration : EntityTypeConfiguration<WorkCycleLines>
    {
        public WorkCycleLinesConfiguration()
        {
            HasKey(table => new
            {
                table.RawMaterialId,
                table.SupplierId,
                table.WorkCycleId
            });
        }
        
    }
}