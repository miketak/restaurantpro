using System.Collections.Generic;
using RestaurantPro.Core.Domain;

namespace RestaurantPro.Core.Repositories
{
    public interface IRawMaterialCategoryRepository : IRepository<RawMaterialCategory>
    {
        void AddOrUpdate(List<RawMaterialCategory> categories);
        void FakeDelete(RawMaterialCategory category);
    }
}