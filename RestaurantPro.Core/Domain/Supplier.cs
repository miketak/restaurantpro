using System.Collections;
using System.Collections.Generic;

namespace RestaurantPro.Core.Domain
{
    public class Supplier
    {

        public int Id { get; set; }

        public string Name { get; set; }

        public string Address { get; set; }

        public string Telephone { get; set; }

        public string Email { get; set; }

        public bool Active { get; set; }

        #region Navigation Properties

        public virtual ICollection<RawMaterialCatalog> RawMaterialCatalog { get; set; }

        public virtual ICollection<PurchaseOrderLines> PurchaseOrderLines { get; set; }

        public virtual ICollection<WorkCycleLines> WorkCycleLines { get; set; }

        #endregion

        
    }
}