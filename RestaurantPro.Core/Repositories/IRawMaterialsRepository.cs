using System.Collections.Generic;
using RestaurantPro.Core.Domain;

namespace RestaurantPro.Core.Repositories
{
    public interface IRawMaterialsRepository : IRepository<RawMaterial>
    {
        void AddOrUpdateRawMaterials(List<RawMaterial> rawMaterials);
        void FakeDelete(RawMaterial rawMaterial);
        IEnumerable<RawMaterial> GetRawMaterials();

        RawMaterial ReturnRawMaterialIfExists(string rawMaterialName);
    }
}