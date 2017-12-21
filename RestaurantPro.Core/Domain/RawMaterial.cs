using System.Collections.Generic;
using System.ComponentModel;

namespace RestaurantPro.Core.Domain
{
    public class RawMaterial
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public virtual RawMaterialCategory RawMaterialCategory { get; set; }

        public int RawMaterialCategoryId { get; set; }

        public virtual ICollection<RawMaterialCatalog> RawMaterialCatalogs { get; set; }

    }
}