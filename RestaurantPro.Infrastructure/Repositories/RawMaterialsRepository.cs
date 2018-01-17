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

        public IEnumerable<RawMaterial> GetRawMaterials()
        {
            return _context.RawMaterials.Where(x => x.Active).ToList();
        }

        public RawMaterial ReturnRawMaterialIfExists(string rawMaterialName)
        {
            return _context
                .RawMaterials.Where(x => x.Active)
                .SingleOrDefault(raw => raw.Name.ToLower() == rawMaterialName.ToLower());
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

        public void FakeDelete(RawMaterial rawMaterial)
        {
            var rawMaterialInDb = _context.RawMaterials.SingleOrDefault(s => s.Id == rawMaterial.Id);

            if( rawMaterialInDb == null)
                throw new ApplicationException("Illegal Operation");

            rawMaterial.Active = false;
            UpdateRawMaterial(rawMaterialInDb, rawMaterial);
            _context.SaveChanges();
        }


        private void AddRawMaterial(RawMaterial rawMaterial)
        {
            rawMaterial.Active = true;
            _context.RawMaterials.Add(rawMaterial);
        }

        private void UpdateRawMaterial(RawMaterial rawMaterialInDb, RawMaterial rawMaterial)
        {
            rawMaterialInDb.Name = rawMaterial.Name;
            rawMaterialInDb.RawMaterialCategoryId = rawMaterial.RawMaterialCategoryId;
            rawMaterialInDb.Active = rawMaterial.Active;
        }
    }
}