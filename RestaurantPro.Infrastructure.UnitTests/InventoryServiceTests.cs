using System;
using System.Collections.Generic;
using System.Linq;
using NUnit.Framework;
using NUnit.Framework.Interfaces;
using RestaurantPro.Core;
using RestaurantPro.Core.Domain;
using RestaurantPro.Core.Services;
using RestaurantPro.Infrastructure.Services;

namespace RestaurantPro.Infrastructure.UnitTests
{
    [TestFixture]
    public class InventoryServiceTests
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IRestProService _services;

        public InventoryServiceTests()
        {
            _unitOfWork = new UnitOfWork(new RestProContext());
            _services = new RestProService(_unitOfWork);
        }

        [Test]
        public void ConfirmWorkCycle_WorkCycleIdArgumentIsLessThanZero_ThrowsArgumentOutOfRangeException()
        {
            Assert.That(() => _services.InventoryService.ConfirmWorkCycle(-1, new User()),
                Throws.Exception.TypeOf<ArgumentOutOfRangeException>());
        }        
        
        [Test]
        public void ConfirmWorkCycle_UserArgumentIsNull_ThrowsArgumentOutOfRangeException()
        {
            Assert.That(() => _services.InventoryService.ConfirmWorkCycle(1, null),
                Throws.ArgumentNullException);
        }        
        
        [Test]
        public void ConfirmWorkCycle_WhenCalled_SetsWorkCycleToActive()
        {
            var workCycle = AddandGetTestWorkCycle();

            _services.InventoryService.ConfirmWorkCycle(workCycle.Id, _user);

            var result = _unitOfWork.WorkCycles.GetWorkCycleById(workCycle.Id, true).StatusId;

            Assert.That(result, Is.EqualTo(WorkCycleStatus.Active.ToString()));
        }

        [Test]
        public void ConfirmWorkCycle_WhenCalled_AddsWorkCycleToWorkCycleTransactions()
        {
            var workCycle = AddandGetTestWorkCycle();

            _services.InventoryService.ConfirmWorkCycle(workCycle.Id, _user);

            var result = _unitOfWork.WorkCycleTransactions.GetAll().Where(p => p.WorkCycleId == workCycle.Id).ToArray();

            Assert.That(result.Length, Is.EqualTo(workCycle.WorkCycleLines.Count));
        }

        [Test]
        public void ConfirmWorkCycle_WhenCalled_UsedQuantityInWorkCycleTransactionsIsNegative()
        {
            var workCycle = AddandGetTestWorkCycle();

            _services.InventoryService.ConfirmWorkCycle(workCycle.Id, _user);

            var result = _unitOfWork.WorkCycleTransactions.GetAll().Where(p => p.WorkCycleId == workCycle.Id)
                .All(t => t.UsedQuantity < 0);

            Assert.IsTrue(result);
        }

        [Test]
        public void ConfirmWorkCycle_WhenCalled_AddsWorkCycleToPurchaseOrderTable()
        {
            var workCycle = AddandGetTestWorkCycle();

            _services.InventoryService.ConfirmWorkCycle(workCycle.Id, _user);

            var purchaseOrderId = _unitOfWork.PurchaseOrders.SingleOrDefault(x => x.WorkCycleId == workCycle.Id).Id;
            var result = _unitOfWork.PurchaseOrders.GetPurchaseOrderById(purchaseOrderId, true);

            Assert.That(result.PurchaseOrderLines.Count, Is.EqualTo(workCycle.WorkCycleLines.Count));
        }        
        
        [Test]
        public void ConfirmWorkCycle_WhenCalled_PurchaseOrderStatusIsSetToNew()
        {
            var workCycle = AddandGetTestWorkCycle();

            _services.InventoryService.ConfirmWorkCycle(workCycle.Id, _user);

            var purchaseOrderId = _unitOfWork.PurchaseOrders.SingleOrDefault(x => x.WorkCycleId == workCycle.Id).Id;
            var result = _unitOfWork.PurchaseOrders.GetPurchaseOrderById(purchaseOrderId, true);

            Assert.That(result.StatusId, Is.EqualTo(PurchaseOrderStatus.New.ToString()));
        }

        [Test]
        public void ProcurePurchaseOrder_OldPurchaseOrderArgumentIsNull_ThrowsArgumentNullException()
        {
            Assert.That( () =>
                _services.InventoryService.ProcurePurchaseOrder(null, new List<PurchaseOrderTransaction>(), new User()), Throws.ArgumentNullException); 
        }

        [Test]
        public void ProcurePurchaseOrder_PurchaseOrderTransactionsArgumentIsNull_ThrowsArgumentNullException()
        {
            Assert.That(() =>
                _services.InventoryService.ProcurePurchaseOrder(new PurchaseOrder(), null, new User()), Throws.ArgumentNullException); 
        }

        [Test]
        public void ProcurePurchaseOrder_UserArgumentIsNull_ThrowsArgumentNullException()
        {
            Assert.That(() =>
                _services.InventoryService.ProcurePurchaseOrder(new PurchaseOrder(), new List<PurchaseOrderTransaction>(), null), Throws.ArgumentNullException); 
        }

        [Test]
        public void ProcurePurchaseOrder_WhenCalled_ChangesPurchaseOrderStatusToInProgress()
        {
            var workCycle = AddandGetTestWorkCycle();
            _services.InventoryService.ConfirmWorkCycle(workCycle.Id, _user);
            var oldPurchaseOrder = GetPurchaseOrderByWorkCycleId(workCycle);
            var userTransactions = GeneratePurchaseOrderTransactionsFromWorkCycle(workCycle).ToList();

            _services.InventoryService.ProcurePurchaseOrder(oldPurchaseOrder, userTransactions, _user);

            var result = _unitOfWork.PurchaseOrders.GetPurchaseOrderById(oldPurchaseOrder.Id, true).StatusId;

            Assert.That(result, Is.EqualTo("In Progress"));
        }

        [Test]
        public void ProcurePurchaseOrder_WhenCalled_AssignsProcuredQuantityToPurchaseOrderTransactionQuantityReceived()
        {
            var workCycle = AddandGetTestWorkCycle();
            _services.InventoryService.ConfirmWorkCycle(workCycle.Id, _user);
            var oldPurchaseOrder = GetPurchaseOrderByWorkCycleId(workCycle);
            var userTransactions = GeneratePurchaseOrderTransactionsFromWorkCycle(workCycle).ToList();

            _services.InventoryService.ProcurePurchaseOrder(oldPurchaseOrder, userTransactions, _user);

            var purchaseOrder = _unitOfWork.PurchaseOrders.GetPurchaseOrderById(oldPurchaseOrder.Id, true);

            var purchaseOrderTransactionsInDb = _unitOfWork.PurchaseOrderTransactions
                .GetAll().Where(p => p.PurchaseOrderId == purchaseOrder.Id).ToArray();

            Assert.That(userTransactions.ToArray()[0].QuantityReceived, Is.EqualTo(purchaseOrderTransactionsInDb[0].QuantityReceived));
            Assert.That(userTransactions.ToArray()[1].QuantityReceived, Is.EqualTo(purchaseOrderTransactionsInDb[1].QuantityReceived));
        }

        [Test]
        public void ProcurePurchaseOrder_WhenCalled_AssignsProcuredQuantityToWorkCycleTransactionUsedQuantity()
        {
            var workCycle = AddandGetTestWorkCycle();
            _services.InventoryService.ConfirmWorkCycle(workCycle.Id, _user);
            var oldPurchaseOrder = GetPurchaseOrderByWorkCycleId(workCycle);
            var userTransactions = GeneratePurchaseOrderTransactionsFromWorkCycle(workCycle).ToList();

            _services.InventoryService.ProcurePurchaseOrder(oldPurchaseOrder, userTransactions, _user);

            var purchaseOrderTransactionsInDb = _unitOfWork.PurchaseOrderTransactions
                .GetAll().Where(p => p.PurchaseOrderId == oldPurchaseOrder.Id).ToArray();

            var wcTransactionsFromPurchaseOrder = _unitOfWork.WorkCycleTransactions
                .GetAll().Where(p => p.TrackingNumber == purchaseOrderTransactionsInDb[0].TrackingNumber);

            Assert.That(wcTransactionsFromPurchaseOrder.ToArray()[0].UsedQuantity, Is.EqualTo(purchaseOrderTransactionsInDb[0].QuantityReceived));
            Assert.That(wcTransactionsFromPurchaseOrder.ToArray()[0].UsedQuantity, Is.EqualTo(purchaseOrderTransactionsInDb[1].QuantityReceived));
        }        
        
        [Test]
        public void ProcurePurchaseOrder_WhenCalled_CorrectlyReflectsOutstandingStockNeeded()
        {
            var workCycle = AddandGetTestWorkCycle();
            _services.InventoryService.ConfirmWorkCycle(workCycle.Id, _user);
            var oldPurchaseOrder = GetPurchaseOrderByWorkCycleId(workCycle);
            var userTransactions = GeneratePurchaseOrderTransactionsFromWorkCycle(workCycle).ToList();

            _services.InventoryService.ProcurePurchaseOrder(oldPurchaseOrder, userTransactions, _user);

            var workCycleTransactionsNetDifference = _unitOfWork.WorkCycleTransactions.GetAll()
                .Where(w => w.WorkCycleId == workCycle.Id && w.RawMaterialId == workCycle.WorkCycleLines.ToArray()[0].RawMaterialId)
                .Select(p => p.UsedQuantity)
                .Sum();

            Assert.That(workCycleTransactionsNetDifference, Is.EqualTo(-30));
        }

        [Test]
        public void ProcurePurchaseOrder_WhenCalled_SetsWorkCycleLocationIdCorrectly()
        {
            var workCycle = AddandGetTestWorkCycle();
            _services.InventoryService.ConfirmWorkCycle(workCycle.Id, _user);
            var oldPurchaseOrder = GetPurchaseOrderByWorkCycleId(workCycle);
            var userTransactions = GeneratePurchaseOrderTransactionsFromWorkCycle(workCycle).ToList();

            _services.InventoryService.ProcurePurchaseOrder(oldPurchaseOrder, userTransactions, _user);

            Assert.AreEqual(_testLocationId, workCycle.WorkCycleLines.ToArray()[0].LocationId);
            Assert.AreEqual(_testLocationId, workCycle.WorkCycleLines.ToArray()[1].LocationId);
        }

        [Test]
        public void GetPurchaseOrderInformationTest()
        {
            var pois = _services.InventoryService.GetPurchaseOrderInformation();
        }

        [TearDown]
        public void TearDown()
        {
            CleanupDatabase();

            RestoreWorkCycleActiveState();
        }

        private void CleanupDatabase()
        {
            var testWorkCycle = _unitOfWork.WorkCycles.GetWorkCycleByWorkCycleName(_testWorkCycleNumber, true);

            if (testWorkCycle == null)
                return;

            _unitOfWork.WorkCycles.Remove(testWorkCycle);

            _unitOfWork.Complete();
        }

        private PurchaseOrder GetPurchaseOrderByWorkCycleId(WorkCycle workCycle)
        {
            var purchaseOrderId = _unitOfWork.PurchaseOrders.SingleOrDefault(x => x.WorkCycleId == workCycle.Id).Id;
            return _unitOfWork.PurchaseOrders.GetPurchaseOrderById(purchaseOrderId, true);
        }

        private IEnumerable<PurchaseOrderTransaction> GeneratePurchaseOrderTransactionsFromWorkCycle(WorkCycle workCycle)
        {
            var purchaseOrderId = _unitOfWork.PurchaseOrders.SingleOrDefault(x => x.WorkCycleId == workCycle.Id).Id;

            var userTransactions = GetFakePurchaseOrderTransactions(purchaseOrderId).ToList();

            return userTransactions;
        }

        private void AddTestWorkCycle()
        {
            var workCycleToInsert = new WorkCycle
            {
                Name = _testWorkCycleNumber,
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

        private WorkCycle AddandGetTestWorkCycle()
        {
            SetAllWorkCyclesToDraft();

            AddTestWorkCycle();

            return _unitOfWork.WorkCycles.GetWorkCycleByWorkCycleName(_testWorkCycleNumber, true);
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
                    LocationId = _testLocationId
                    
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
                    LocationId = _testLocationId
                }
            };
            return transactions;
        }

        private void SetAllWorkCyclesToDraft()
        {
            var activeWorkCycle = _unitOfWork.WorkCycles
                .GetAll().FirstOrDefault(w => w.StatusId == WorkCycleStatus.Active.ToString());

            if (activeWorkCycle == null)
                return;

            _activeWorkCyleId = activeWorkCycle.Id;
            activeWorkCycle.StatusId = WorkCycleStatus.Draft.ToString();
            _unitOfWork.Complete();
        }

        private void RestoreWorkCycleActiveState()
        {
            if (_activeWorkCyleId == 0)
                return;

            var workCycleToSetActive = _unitOfWork.WorkCycles.Get(_activeWorkCyleId);


            workCycleToSetActive.StatusId = WorkCycleStatus.Active.ToString();

            _unitOfWork.Complete();
        }

        private int _activeWorkCyleId;
        private readonly string _testPurchaseOrderNumber = "100";
        private readonly string _testWorkCycleNumber = "Test Cycle 1";
        private readonly User _user = new User { Id = 1 };
        private readonly string _testLocationId = "Home Warehouse";
    }
}