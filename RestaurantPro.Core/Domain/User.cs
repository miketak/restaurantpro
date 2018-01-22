using System.Collections.Generic;
using RestaurantPro.Core.Repositories;

namespace RestaurantPro.Core.Domain
{
    public class User
    {
        public int Id { get; set; }
        public string Username { get; set; }

        public string FirstName { get; set; }

        public string LastName { get; set; }

        public string Email { get; set; }

        //Remove this now future Michael. You know this is evil and bad practice. Regards. Michael 12/17/17:21:21CDT
        public byte[] PasswordHash { get; set; }

        public byte[] SaltHash { get; set; }

        #region Navigation Properties

        public virtual ICollection<PurchaseOrder> PurchaseOrders { get; set; }

        public virtual ICollection<WorkCycleAdjustments> WorkCycleAdjustments { get; set; }

        public virtual ICollection<WorkCycleTransaction> WorkCycleTransactions { get; set; }

        public virtual ICollection<PurchaseOrderTransaction> PurchaseOrderTransactions { get; set; }

        public virtual ICollection<InventoryTransaction> InventoryTransactions { get; set; }

        #endregion

    }
}