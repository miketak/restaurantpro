using System.Collections.Generic;
using RestaurantPro.Core.Domain;

namespace RestaurantPro.Core.Repositories
{
    public interface ISupplierRepository : IRepository<Supplier>
    {
        IEnumerable<Supplier> GetSuppliers();

        void AddOrUpdateSuppliers(List<Supplier> suppliers);

        void FakeDeleteSupplier(Supplier supplier);
    }
}