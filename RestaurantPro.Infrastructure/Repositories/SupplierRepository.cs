using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using RestaurantPro.Core.Domain;
using RestaurantPro.Core.Repositories;

namespace RestaurantPro.Infrastructure.Repositories
{
    public class SupplierRepository : Repository<Supplier>, ISupplierRepository
    {
        private readonly RestProContext _context;

        public SupplierRepository(DbContext context) : base(context)
        {
            _context = (RestProContext) context;
        }

        public void AddOrUpdateSuppliers(List<Supplier> suppliers)
        {
            foreach (var supplier in suppliers)
            {
                var supplierInDb = _context.Suppliers
                    .SingleOrDefault(s => s.Id == supplier.Id);

                if ( supplierInDb == null)
                    AddSupplier(supplier);
                else
                    UpdateSupplier(supplierInDb, supplier);

                _context.SaveChanges();
            }
        }

        public void FakeDeleteSupplier(Supplier supplier)
        {
            var supplierInDb = _context.Suppliers.SingleOrDefault(s => s.Id == supplier.Id);

            if ( supplierInDb == null)
                throw new ApplicationException("Illegal Operation");

            supplierInDb.Active = false;
            _context.SaveChanges();
        }

        private void UpdateSupplier(Supplier supplierInDb, Supplier supplier)
        {
            supplierInDb.Name = supplier.Name;
            supplierInDb.Address = supplier.Address;
            supplierInDb.Telephone = supplier.Telephone;
            supplierInDb.Email = supplier.Email;
            supplierInDb.Active = true;
        }

        private void AddSupplier(Supplier supplier)
        {
            supplier.Active = true;
            _context.Suppliers.Add(supplier);
        }
    }
}