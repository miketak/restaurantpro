namespace RestaurantPro.Core.Domain
{
    public class PurchaseOrderInformation
    {
        public int Id { get; set; }

        public int RawMaterialId { get; set; }

        public int SupplierId { get; set; }

        public int WorkCycleId { get; set; }

        public int PurchaseOrderId { get; set; }

        public double OrderedQuantity { get; set; }

        public double PendingQuantity { get; set; }

        public double DateReceived { get; set; }

        public decimal TotalValue { get; set; }

        public string DeliveredBy { get; set; }

    }
}