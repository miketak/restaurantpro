using System.Collections.Generic;
using RestaurantPro.Core.Domain;

namespace RestaurantPro.Core.Services
{
    public interface IInventoryService
    {
        void ConfirmWorkCycle(int workCycleId, User user);

        void ProcurePurchaseOrder(PurchaseOrder oldPurchaseOrder,
            IEnumerable<PurchaseOrderTransaction> purchaseOrderTransactions, User user);
    }
}