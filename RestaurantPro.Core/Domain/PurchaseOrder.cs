using System;
using System.Collections.Generic;

namespace RestaurantPro.Core.Domain
{
    public class PurchaseOrder
    {
        public int Id { get; set; }

        public string PurchaseOrderNumber { get; set; }

        public DateTime DateCreated { get; set; }

        public int CreatedBy { get; set; }

        public string StatusId { get; set; }

        public int? WorkCycleId { get; set; }

        public bool Active { get; set; }

        public IEnumerable<PurchaseOrderLine> Lines { get; set; }


        #region Navigation Properties

        public virtual User User { get; set; }

        public virtual PoStatus Status { get; set; }

        public virtual WorkCycle WorkCycle { get; set; }

        public virtual ICollection<PurchaseOrderLine> PurchaseOrderLines { get; set; }

        #endregion

    }
}