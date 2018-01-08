using System.Collections.Generic;
using System.Linq;
using RestaurantPro.Core;
using RestaurantPro.Core.Domain;
using RestaurantPro.Infrastructure;

namespace RestaurantPro.Models
{
    public class WpfRawMaterial : ValidatableBindableBase
    {
        static IUnitOfWork _unitOfWork = new UnitOfWork(new RestProContext());
        private string _name;

        public int Id { get; set; }

        public string Name
        {
            get { return _name; }
            set { SetProperty(ref _name, value); }
        }

        public int RawMaterialCategoryId { get; set; }

        #region Properties for View

        public string Category
        {
            get
            {
                if (RawMaterialCategoryId == 0)
                    RawMaterialCategoryId = 1;

                return _unitOfWork
                    .RawMaterialCategories
                    .SingleOrDefault(x => x.Id == RawMaterialCategoryId)
                    .Name;
            }
            set
            {
                SetProperty(ref _rawMaterialCategory, value);

                RawMaterialCategoryId = _unitOfWork
                    .RawMaterialCategories
                    .SingleOrDefault(x => x.Name == value).Id;
            }
        }
        
        public static List<string> Categories
        {
            get
            {
                if (_categories == null)
                    Categories = _unitOfWork
                        .RawMaterialCategories
                        .GetAll().Select(a => a.Name).ToList();
                return _categories;
            }
            set { _categories = value; }
        }

        private string _rawMaterialCategory;
        private string _category;
        private static List<string> _categories;
        #endregion
    }
}