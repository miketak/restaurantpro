using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using RestaurantPro.Core.Domain;
using RestaurantPro.Core.Repositories;

namespace RestaurantPro.Infrastructure.Repositories
{
    public class RawMaterialCategoryRepository : Repository<RawMaterialCategory>, IRawMaterialCategoryRepository
    {
        private readonly RestProContext _context;
        public RawMaterialCategoryRepository(DbContext context) 
            : base(context)
        {
            _context = (RestProContext) context;
        }

        public void AddOrUpdateRawMaterials(List<RawMaterialCategory> categories)
        {
            foreach (var category in categories)
            {
                var categoryInDb = _context.RawMaterialCategories
                    .SingleOrDefault(c => c.Id == category.Id);

                if (categoryInDb == null)
                    AddRawMaterialCategory(category);
                else
                    UpdateRawMaterialCategory(categoryInDb, category);
                _context.SaveChanges();
            }
        }

        public void FakeDeleteSupplier(RawMaterialCategory category)
        {
            var categoryInDb = _context.RawMaterialCategories
                .SingleOrDefault(s => s.Id == category.Id);

            if (categoryInDb == null)
                throw new ApplicationException("Illegal Operation");

            _context.RawMaterialCategories.Remove(categoryInDb); //will be removed to active bit deactivation
            _context.SaveChanges();
        }

        private void AddRawMaterialCategory(RawMaterialCategory category)
        {
            _context.RawMaterialCategories.Add(category);
        }

        private void UpdateRawMaterialCategory(RawMaterialCategory categoryInDb, RawMaterialCategory category)
        {
            categoryInDb.Name = category.Name;
            categoryInDb.Description = category.Description;
        }


    }
}