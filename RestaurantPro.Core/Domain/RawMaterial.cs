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


        #region Navigation Properties

        public virtual ICollection<RawMaterialCatalog> RawMaterialCatalog { get; set; }

        public virtual ICollection<PurchaseOrderLines> PurchaseOrderLines { get; set; }

        public virtual ICollection<WorkCycleLines> WorkCycleLines { get; set; }

        #endregion

        

    }
}