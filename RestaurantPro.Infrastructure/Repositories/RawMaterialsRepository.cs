using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using RestaurantPro.Core.Domain;
using RestaurantPro.Core.Repositories;

namespace RestaurantPro.Infrastructure.Repositories
{
    public class RawMaterialsRepository : Repository<RawMaterial>, IRawMaterialsRepository
    {
        private readonly RestProContext _context;

        public RawMaterialsRepository(DbContext context) 
            : base(context)
        {
            _context = (RestProContext) context;
        }

        public void AddOrUpdateRawMaterials(List<RawMaterial> rawMaterials)
        {
            foreach (var rawMaterial in rawMaterials)
            {
                var rawMaterialInDb = _context.RawMaterials
                    .SingleOrDefault(r => r.Id == rawMaterial.Id);

                if ( rawMaterialInDb == null)
                    AddRawMaterial(rawMaterial);
                else
                    UpdateRawMaterial(rawMaterialInDb, rawMaterial);
                _context.SaveChanges();
            }
        }

        public void FakeDeleteSupplier(RawMaterial rawMaterial)
        {
            var rawMaterialInDb = _context.RawMaterials.SingleOrDefault(s => s.Id == rawMaterial.Id);

            if( rawMaterialInDb == null)
                throw new ApplicationException("Illegal Operation");

            _context.RawMaterials.Remove(rawMaterialInDb); //will be remove to active bit deactivation
            _context.SaveChanges();
        }

        private void AddRawMaterial(RawMaterial rawMaterial)
        {
            _context.RawMaterials.Add(rawMaterial);
        }

        private void UpdateRawMaterial(RawMaterial rawMaterialInDb, RawMaterial rawMaterial)
        {
            rawMaterialInDb.Name = rawMaterial.Name;
            rawMaterialInDb.RawMaterialCategoryId = rawMaterial.RawMaterialCategoryId;
        }
    }
}