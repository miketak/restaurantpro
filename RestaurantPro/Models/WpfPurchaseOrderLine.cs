namespace RestaurantPro.Models
{
    public class WpfPurchaseOrderLine
    {
        public int PurchaseOrderId { get; set; }

        public int RawMaterialId { get; set; }

        public int SupplierId { get; set; }

        public double UnitPrice { get; set; }

        public float Quantity { get; set; }

        public string UnitOfMeasure { get; set; }

        #region Purchase Order Lines View - Datagrid

        

        #endregion
    }
}