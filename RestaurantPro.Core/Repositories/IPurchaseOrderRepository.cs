using RestaurantPro.Core.Domain;

namespace RestaurantPro.Core.Repositories
{
    public interface IPurchaseOrderRepository : IRepository<PurchaseOrder>
    {
        void AddPurchaseOrder(PurchaseOrder purchaseOrder);
    }
}