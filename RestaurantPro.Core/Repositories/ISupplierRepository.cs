using System.Collections.Generic;
using RestaurantPro.Core.Domain;

namespace RestaurantPro.Core.Repositories
{
    public interface ISupplierRepository : IRepository<Supplier>
    {
        void AddOrUpdateSuppliers(List<Supplier> suppliers);

        void FakeDeleteSupplier(Supplier supplier);
    }
}