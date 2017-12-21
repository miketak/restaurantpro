using System.Collections;
using System.Collections.Generic;

namespace RestaurantPro.Core.Domain
{
    public class PoStatus
    {
        public string Status { get; set; }

        public ICollection<PurchaseOrder> PurchaseOrders { get; set;  }
    }
}