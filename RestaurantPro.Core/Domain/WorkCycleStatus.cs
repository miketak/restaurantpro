namespace RestaurantPro.Core.Domain
{
    public enum WorkCycleStatus
    {
        Draft,
        Active,
        Closed
    }

    public enum PurchaseOrderStatus
    {
        Canceled,
        Changed,
        Closed,
        InProgress,
        New,
        Received
    }
}