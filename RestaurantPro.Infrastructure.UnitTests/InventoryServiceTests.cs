using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using RestaurantPro.Core;
using RestaurantPro.Core.Domain;
using RestaurantPro.Core.Services;
using RestaurantPro.Infrastructure.Services;

namespace RestaurantPro.Infrastructure.UnitTests
{
    [TestClass]
    public class InventoryServiceTests
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IRestProService _services;

        public InventoryServiceTests()
        {
            _unitOfWork = new UnitOfWork(new RestProContext());
            _services = new RestProService(_unitOfWork);
        }

        [TestMethod]
        public void ConfirmWorkCycle()
        {
            var user = new User {Id = 1};

            var workCycleId = _unitOfWork.WorkCycles.GetWorkCycles().ToArray()[5].Id;
            var workCycle = _unitOfWork.WorkCycles.GetWorkCycleById(workCycleId, true);
            Trace.WriteLine(string.Format("InventoryServiceTestStarted\n" +
                            "User with Id {0}\n" +
                            "WorkCycleId = {1} " +
                            "with {2} lines", user.Id, workCycle.Id, workCycle.WorkCycleLines.Count));
                            
            if (workCycle == null)
                throw new AssertFailedException();

            

            _services.InventoryService.ConfirmWorkCycle(workCycle.Id, user);

            var poId = _unitOfWork.PurchaseOrders.FirstOrDefault(x => x.WorkCycleId == workCycle.Id).Id;
            var po = _unitOfWork.PurchaseOrders.GetPurchaseOrderById(poId, true);
            Trace.WriteLine(
                string.Format("Successful with {0} lines in PurchaseOrder", po.PurchaseOrderLines.Count )
                );


            Assert.AreEqual(workCycle.WorkCycleLines.Count, po.PurchaseOrderLines.Count);

            _unitOfWork.PurchaseOrders.Remove(po);
            _unitOfWork.Complete();
        }

        [TestMethod]
        public void ProcurePurchaseOrderTest()
        {
            var purchaseOrder = GetPurchaseOrder();

            var user = new User {Id = 2};

            if (purchaseOrder == null) return;
            var poTransaction = new List<PurchaseOrderTransaction>();

            _services.InventoryService.ProcurePurchaseOrder(purchaseOrder, poTransaction, user);
        }

        private PurchaseOrder GetPurchaseOrder()
        {

            var poId = _unitOfWork.PurchaseOrders.GetAll().ToArray()[1].Id;
            var purchaseOrder = _unitOfWork.PurchaseOrders.GetPurchaseOrderById(poId, true);
            purchaseOrder.Lines = new List<PurchaseOrderLine>(purchaseOrder.PurchaseOrderLines);

            _unitOfWork.PurchaseOrders.DetachPurchaseOrder(purchaseOrder);

            return purchaseOrder;
        }
        
    }
}