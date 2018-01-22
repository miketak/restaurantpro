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
        private PurchaseOrder _purchaseOrder;
        private User _user;

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

            var poId = _unitOfWork.PurchaseOrders.FirstOrDefault(x => x.WorkCycleId == workCycle.Id).Id; //problematic ...make workcycleId unique in db. Regards Mike 1/22/18:12:28
            var po = _unitOfWork.PurchaseOrders.GetPurchaseOrderById(poId, true);
            Trace.WriteLine(
                string.Format("Successful with {0} lines in PurchaseOrder", po.PurchaseOrderLines.Count )
                );

            Assert.AreEqual(workCycle.WorkCycleLines.Count, po.PurchaseOrderLines.Count);

            //_unitOfWork.PurchaseOrders.Remove(po);
           // _unitOfWork.Complete();
        }

        [TestMethod]
        public void ProcurePurchaseOrderTest()
        {
            GeneratePurchaseOrder();

            _user = new User {Id = 2};

            if (_purchaseOrder == null) return;
            var poTransaction = GetPurchaseOrderTransactions();

            _services.InventoryService.ProcurePurchaseOrder(_purchaseOrder, poTransaction, _user);
        }

        private void GeneratePurchaseOrder()
        {
            var poId = _unitOfWork.PurchaseOrders.GetAll().ToArray()[2].Id;

            if (poId == 0)
                throw new AssertFailedException("No purchase orders in database");

            _purchaseOrder = _unitOfWork.PurchaseOrders.GetPurchaseOrderById(poId, true);
            _purchaseOrder.Lines = new List<PurchaseOrderLine>(_purchaseOrder.PurchaseOrderLines);

        }

        private IEnumerable<PurchaseOrderTransaction> GetPurchaseOrderTransactions()
        {
            var lines = _purchaseOrder.Lines;

            return lines.Select(line => new PurchaseOrderTransaction
                {
                    PurchaseOrderId = _purchaseOrder.Id,
                    RawMaterialId = line.RawMaterialId,
                    SupplierId = line.SupplierId,
                    QuantityReceived = line.Quantity - 2,
                    DateReceived = DateTime.MaxValue,
                    LocationId = "Room A",
                    DeliveredBy = "Johson Banner",
                    ReceivedBy = _user.Id
                })
                .ToList();
        }
        
    }
}