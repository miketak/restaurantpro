using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using RestaurantPro.Core;
using RestaurantPro.Core.Domain;
using RestaurantPro.Core.Services;
using RestaurantPro.Infrastructure.Migrations;
using RestaurantPro.Infrastructure.Services;

namespace RestaurantPro.Infrastructure.UnitTests
{
    [TestClass]
    public class InventoryServiceTests
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IRestProService _services;

        private string TestPurchaseOrderNumber = "100";
        private string TestWorkCycleNumber = "Test Cycle 1";
        private User _user = new User {Id = 1};
        private int WorkCycleLineCount = 2;
        private string TestLocationId = "Home Warehouse";

        public InventoryServiceTests()
        {
            _unitOfWork = new UnitOfWork(new RestProContext());
            _services = new RestProService(_unitOfWork);
        }

        [TestMethod]
        public void ConfirmWorkCycle()
        {
            AddTestWorkCycle();
            var workCycle = GetTestWorkCycle();
                
            if (workCycle == null)
                throw new AssertFailedException();

            _services.InventoryService.ConfirmWorkCycle(workCycle.Id, _user);
            var transactions = _unitOfWork.WorkCycleTransactions.GetAll().Where(p => p.WorkCycleId == workCycle.Id).ToArray();
            var purchaseOrderId = _unitOfWork.PurchaseOrders.SingleOrDefault(x => x.WorkCycleId == workCycle.Id).Id;
            var purchaseOrder = _unitOfWork.PurchaseOrders.GetPurchaseOrderById(purchaseOrderId, true);

            Assert.AreEqual(WorkCycleStatus.Active.ToString(), workCycle.StatusId);
            Assert.AreEqual(WorkCycleLineCount, transactions.Length);

            Assert.IsTrue(transactions[0].UsedQuantity < 0);

            Assert.AreEqual(workCycle.Id, purchaseOrder.WorkCycleId);
            Assert.AreEqual(PurchaseOrderStatus.New.ToString(), purchaseOrder.StatusId);
            Assert.AreEqual(workCycle.WorkCycleLines.Count, purchaseOrder.PurchaseOrderLines.Count);

            RemoveTestWorkCycle();
            _unitOfWork.PurchaseOrders.Remove(purchaseOrder);
            _unitOfWork.Complete();
        }

        [TestMethod]
        public void ProcurePurchaseOrderTest()
        {
            AddTestWorkCycle();
            var workCycle = GetTestWorkCycle();

            if (workCycle == null)
                throw new AssertFailedException();

            _services.InventoryService.ConfirmWorkCycle(workCycle.Id, _user);
            var purchaseOrderId = _unitOfWork.PurchaseOrders.SingleOrDefault(x => x.WorkCycleId == workCycle.Id).Id;
            var userTransactions = GetFakePurchaseOrderTransactions(purchaseOrderId).ToList();
            var purchaseOrder = _unitOfWork.PurchaseOrders.GetPurchaseOrderById(purchaseOrderId, true);

            _services.InventoryService.ProcurePurchaseOrder(purchaseOrder, userTransactions, _user);
            var purchaseOrderTransactionsInDb = _unitOfWork.PurchaseOrderTransactions
                .GetAll().Where(p => p.PurchaseOrderId == purchaseOrder.Id).ToArray();
            var wcTransactionsFromPurchaseOrder = _unitOfWork.WorkCycleTransactions
                .GetAll().Where(p => p.TrackingNumber == purchaseOrderTransactionsInDb[0].TrackingNumber);

            Assert.AreEqual("In Progress", purchaseOrder.StatusId);
            Assert.AreEqual(userTransactions.ToArray()[0].QuantityReceived, purchaseOrderTransactionsInDb[0].QuantityReceived);
            Assert.AreEqual(userTransactions.ToArray()[1].QuantityReceived, purchaseOrderTransactionsInDb[1].QuantityReceived);
            Assert.AreEqual(wcTransactionsFromPurchaseOrder.ToArray()[0].UsedQuantity, purchaseOrderTransactionsInDb[0].QuantityReceived);
            Assert.AreEqual(wcTransactionsFromPurchaseOrder.ToArray()[1].UsedQuantity, purchaseOrderTransactionsInDb[1].QuantityReceived);


            var workCycleTransactionsNetDifference = _unitOfWork.WorkCycleTransactions.GetAll()
                .Where(w => w.WorkCycleId == workCycle.Id && w.RawMaterialId == workCycle.WorkCycleLines.ToArray()[0].RawMaterialId)
                .Select(p => p.UsedQuantity)
                .Sum();

            Assert.AreEqual(-30, workCycleTransactionsNetDifference);
            Assert.AreEqual(TestLocationId, workCycle.WorkCycleLines.ToArray()[0].LocationId);
            Assert.AreEqual(TestLocationId, workCycle.WorkCycleLines.ToArray()[1].LocationId);

            RemoveTestWorkCycle();
            _unitOfWork.PurchaseOrders.Remove(purchaseOrder);
            _unitOfWork.Complete();

        }

        [TestMethod]
        public void GetPurchaseOrderInformationTest()
        {
            var pois = _services.InventoryService.GetPurchaseOrderInformation();
        }


        private void AddTestWorkCycle()
        {
            var workCycleToInsert = new WorkCycle
            {
                Name = TestWorkCycleNumber,
                DateBegin = new DateTime(2017, 09, 01),
                DateEnd = new DateTime(2017, 09, 11),
                Active = true,
                UserId = 1,
                StatusId = WorkCycleStatus.Draft.ToString(),
                Lines = new List<WorkCycleLines>{
                    new WorkCycleLines
                    {
                        RawMaterialId = 1,
                        SupplierId = 1,
                        UnitPrice = 50,
                        PlannedQuantity = 45,
                        UnitOfMeasure = "crates"
                    },
                    new WorkCycleLines
                    {
                        RawMaterialId = 2,
                        SupplierId = 2,
                        UnitPrice = 50,
                        PlannedQuantity = 45,
                        UnitOfMeasure = "crates"
                    }
                }
            };
            _unitOfWork.WorkCycles.AddWorkingCycle(workCycleToInsert);
        }

        private void RemoveTestWorkCycle()
        {
            var workCycle = _unitOfWork.WorkCycles.SingleOrDefault(x => x.Name == TestWorkCycleNumber);
            _unitOfWork.WorkCycles.Remove(workCycle);
            _unitOfWork.Complete();
        }

        private WorkCycle GetTestWorkCycle()
        {
            return _unitOfWork.WorkCycles.GetWorkCycleByWorkCycleName(TestWorkCycleNumber, true);
        }

        private IEnumerable<PurchaseOrderTransaction> GetFakePurchaseOrderTransactions(int purchaseOrderId)
        {
            var timeStamp = DateTime.Now;
            var transactions = new List<PurchaseOrderTransaction>
            {
                new PurchaseOrderTransaction
                {
                    RawMaterialId = 1,
                    SupplierId = 1,
                    PurchaseOrderId = purchaseOrderId,
                    QuantityReceived = 15,
                    DeliveredBy = "Michael Johnson",
                    DateReceived = timeStamp,
                    TransactionDate = timeStamp,
                    ReceivedBy = _user.Id,
                    LocationId = TestLocationId
                    
                },
                new PurchaseOrderTransaction
                {
                    RawMaterialId = 2,
                    SupplierId = 2,
                    PurchaseOrderId = purchaseOrderId,
                    QuantityReceived = 15,
                    DeliveredBy = "Michael Dwayne",
                    DateReceived = timeStamp,
                    TransactionDate = timeStamp,
                    ReceivedBy = _user.Id,
                    LocationId = TestLocationId
                }
            };
            return transactions;
        }


        
    }
}