using System.Collections.Generic;
using RestaurantPro.Core.Domain;

namespace RestaurantPro.Core.Repositories
{
    public interface IRawMaterialCategoryRepository : IRepository<RawMaterialCategory>
    {
        void AddOrUpdateRawMaterials(List<RawMaterialCategory> categories);
        void FakeDeleteSupplier(RawMaterialCategory category);
    }
}