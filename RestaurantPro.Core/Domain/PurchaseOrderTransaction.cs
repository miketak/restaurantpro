using System;

namespace RestaurantPro.Core.Domain
{
    public class PurchaseOrderTransaction
    {
        public int Id { get; set; }

        public int PurchaseOrderId { get; set; }

        public int RawMaterialId { get; set; }

        public int SupplierId { get; set; }

        public int TrackingNumber { get; set; }

        public double QuantityReceived { get; set; }

        public DateTime DateReceived { get; set; }

        /// <summary>
        /// Full name of person who delivered the raw material
        /// </summary>
        public string DeliveredBy { get; set; }

        /// <summary>
        /// Employee who received the raw material
        /// </summary>
        public int ReceivedBy { get; set; }

        public DateTime TransactionDate { get; set; }

        #region Navigation Properties

        public virtual RawMaterial RawMaterial { get; set; }

        public virtual PurchaseOrder PurchaseOrder { get; set; }

        public virtual Supplier Supplier { get; set; }

        public virtual User User { get; set; }

        #endregion
    }
}