namespace RestaurantPro.Core.Domain
{
    public class PurchaseOrderLine
    {
        public int PurchaseOrderId { get; set; }

        public int RawMaterialId { get; set; }

        public int SupplierId { get; set; }

        public double UnitPrice { get; set; }

        public float Quantity { get; set; }

        public string UnitOfMeasure { get; set; }

        #region Equals

        protected bool Equals(PurchaseOrderLine other)
        {
            return PurchaseOrderId == other.PurchaseOrderId && RawMaterialId == other.RawMaterialId &&
                   SupplierId == other.SupplierId && UnitPrice.Equals(other.UnitPrice) &&
                   Quantity.Equals(other.Quantity) && string.Equals(UnitOfMeasure, other.UnitOfMeasure);
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            if (obj.GetType() != GetType()) return false;
            return Equals((PurchaseOrderLine) obj);
        }

        public override int GetHashCode()
        {
            unchecked
            {
                var hashCode = PurchaseOrderId;
                hashCode = (hashCode * 397) ^ RawMaterialId;
                hashCode = (hashCode * 397) ^ SupplierId;
                hashCode = (hashCode * 397) ^ UnitPrice.GetHashCode();
                hashCode = (hashCode * 397) ^ Quantity.GetHashCode();
                hashCode = (hashCode * 397) ^ (UnitOfMeasure != null ? UnitOfMeasure.GetHashCode() : 0);
                return hashCode;
            }
        }

        #endregion

        #region Navigation Properties

        public virtual PurchaseOrder PurchaseOrder { get; set; }

        public virtual RawMaterial RawMaterial { get; set; }

        public virtual Supplier Supplier { get; set; }
        public string SupplierStringTemp { get; set; }
        public string RawMaterialStringTemp { get; set; }

        #endregion
    }
}