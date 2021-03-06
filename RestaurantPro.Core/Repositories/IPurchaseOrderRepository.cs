﻿using System.Collections.Generic;
using RestaurantPro.Core.Domain;

namespace RestaurantPro.Core.Repositories
{
    public interface IPurchaseOrderRepository : IRepository<PurchaseOrder>
    {
        void AddPurchaseOrder(PurchaseOrder purchaseOrder);
        PurchaseOrder GetPurchaseOrderByPurchaseOrderNumber(string purchaseOrderNumber, bool isActive);
        PurchaseOrder GetPurchaseOrderById(int purchaseOrderId, bool isActive);
        void UpdatePurchaseOrder(PurchaseOrder purchaseOrder);
        IEnumerable<PurchaseOrder> GetPurchaseOrdersWithoutLines();

        void DetachPurchaseOrder(PurchaseOrder purchaseOrder);

        void Dispose();
    }
}