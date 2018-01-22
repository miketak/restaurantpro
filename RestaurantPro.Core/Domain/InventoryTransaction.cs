using System;

namespace RestaurantPro.Core.Domain
{
    public class InventoryTransaction
    {
        public int Id { get; set; }

        public int InventoryId { get; set; }

        public double Quantity { get; set; }

        public int CreatedBy { get; set; }

        public DateTime TransactionDate { get; set; }

        #region Navigation Properties

        public virtual Inventory Inventory { get; set; }

        public virtual User User { get; set; }

        #endregion
    }
}