using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using RestaurantPro.Core;
using RestaurantPro.Core.Domain;

namespace RestaurantPro.Infrastructure.UnitTests
{
    [TestClass]
    public class PurchaserOrderRepositoryTests
    {
        private readonly IUnitOfWork _unitOfWork;
        private const string FirstTestPurchaseOrderNumber = "100";
        private const string SecondTestPurchaseOrderNumber = "101";
        private const int ExpectedLineCount = 6;

        public PurchaserOrderRepositoryTests()
        {
            _unitOfWork = new UnitOfWork(new RestProContext());
        }

        [TestMethod]
        public void AddPurchaseOrderTestWithTrueAndFalseActiveFlag()
        {
            //With true active flag
            AddPurchaseOrderWithLines(FirstTestPurchaseOrderNumber, true);

            //Add with false active flag
            AddPurchaseOrderWithLines(SecondTestPurchaseOrderNumber, false);

            var firstPurchaseOrderInDb = _unitOfWork.PurchaseOrders.GetPurchaseOrderByPurchaseOrderNumber(FirstTestPurchaseOrderNumber, true);
            Assert.AreEqual(FirstTestPurchaseOrderNumber, firstPurchaseOrderInDb.PurchaseOrderNumber);
            Assert.AreEqual(ExpectedLineCount, firstPurchaseOrderInDb.PurchaseOrderLines.Count);

            var secondPurchaseOrderInDb = _unitOfWork.PurchaseOrders.GetPurchaseOrderByPurchaseOrderNumber(SecondTestPurchaseOrderNumber, false);
            Assert.AreEqual(SecondTestPurchaseOrderNumber, secondPurchaseOrderInDb.PurchaseOrderNumber);
            Assert.AreEqual(ExpectedLineCount, secondPurchaseOrderInDb.PurchaseOrderLines.Count);

            RemovePurchaseOrder(FirstTestPurchaseOrderNumber);
            RemovePurchaseOrder(SecondTestPurchaseOrderNumber);
        }

        [TestMethod]
        public void GetPurchaseOrderByPurchaseOrderNumberWithTrueActiveFlag()
        {
            AddPurchaseOrderWithLines(FirstTestPurchaseOrderNumber, true);
            var purchaseOrder = _unitOfWork.PurchaseOrders.GetPurchaseOrderByPurchaseOrderNumber(FirstTestPurchaseOrderNumber, true);

            Assert.AreEqual(FirstTestPurchaseOrderNumber, purchaseOrder.PurchaseOrderNumber);
            Assert.AreEqual(ExpectedLineCount, purchaseOrder.PurchaseOrderLines.Count);

            RemovePurchaseOrder(FirstTestPurchaseOrderNumber);
        }
        
        [TestMethod]
        public void GetPurchaseOrderByPurchaseOrderNumberWithTrueActiveFlagUsingFalseFlag()
        {
            AddPurchaseOrderWithLines(FirstTestPurchaseOrderNumber, true);
            var purchaseOrder = _unitOfWork.PurchaseOrders.GetPurchaseOrderByPurchaseOrderNumber(FirstTestPurchaseOrderNumber, false);

            Assert.IsNull(purchaseOrder);

            RemovePurchaseOrder(FirstTestPurchaseOrderNumber);
        }

 

        [TestMethod]
        public void GetPurchaseOrderByPurchaseOrderNumberWithTrueActiveFlagUsingTrueFlag()
        {
            AddPurchaseOrderWithLines(FirstTestPurchaseOrderNumber, true);
            var purchaseOrder = _unitOfWork.PurchaseOrders.GetPurchaseOrderByPurchaseOrderNumber(FirstTestPurchaseOrderNumber, true);

            Assert.IsNotNull(purchaseOrder);

            RemovePurchaseOrder(FirstTestPurchaseOrderNumber);
        }




        [TestMethod]
        public void GetPurchaseOrderByIdWithTrueActiveFlag()
        {
            AddPurchaseOrderWithLines(FirstTestPurchaseOrderNumber, true);
            var purchaseOrderInDb = _unitOfWork.PurchaseOrders.SingleOrDefault(c => c.PurchaseOrderNumber == FirstTestPurchaseOrderNumber);

            var purchaseOrder = _unitOfWork.PurchaseOrders.GetPurchaseOrderById(purchaseOrderInDb.Id, true);

            Assert.AreEqual(FirstTestPurchaseOrderNumber, purchaseOrder.PurchaseOrderNumber);
            Assert.AreEqual(ExpectedLineCount, purchaseOrder.PurchaseOrderLines.Count);

            RemovePurchaseOrder(FirstTestPurchaseOrderNumber);
        }

        private void AddPurchaseOrderWithLines(string poNumber, bool isActive)
        {
            var purchaseOrder = GetPurchaseOrderWithLines(poNumber, isActive);
            _unitOfWork.PurchaseOrders.AddPurchaseOrder(purchaseOrder);
        }

        private void RemovePurchaseOrder(string purchaseOrderNumber)
        {
            var purchaseOrder =
                _unitOfWork.PurchaseOrders.SingleOrDefault(x => x.PurchaseOrderNumber == purchaseOrderNumber);
            _unitOfWork.PurchaseOrders.Remove(purchaseOrder);
            _unitOfWork.Complete();
        }

        private PurchaseOrder GetPurchaseOrderWithLines(string purchaseOrderNumber, bool isActive)
        {
            var wcs = _unitOfWork.WorkCycles.GetAll().ToArray();
            var supps = _unitOfWork.Suppliers.GetAll().ToArray();
            var rms = _unitOfWork.RawMaterials.GetAll().ToArray();
            var statuses = _unitOfWork.Statuses.GetAll().ToArray();

            if (supps.Length == 0)
                throw new AssertFailedException("No suppliers in Database");
            if (wcs.Length == 0)
                throw new AssertFailedException("No Working Cycles in Database");
            if (rms.Length == 0)
                throw new AssertFailedException("No Raw Materials in Database");
            if (statuses.Length == 0)
                throw new AssertFailedException("No Statuses in Database");

            var po = new PurchaseOrder
            {
                PurchaseOrderNumber = purchaseOrderNumber,
                DateCreated = DateTime.Now,
                CreatedBy = 1,
                StatusId = statuses[1].Status,
                Active = isActive,
                WorkCycleId = wcs[0].Id
            };

            var poLines = new List<PurchaseOrderLine>
            {
                new PurchaseOrderLine { RawMaterialId = rms[0].Id, SupplierId = supps[0].Id, UnitPrice= 25, Quantity = 91, UnitOfMeasure = "crates", LeadTime = 5},
                new PurchaseOrderLine { RawMaterialId = rms[1].Id, SupplierId = supps[1].Id, UnitPrice= 26, Quantity = 92, UnitOfMeasure = "bags"},
                new PurchaseOrderLine { RawMaterialId = rms[2].Id, SupplierId = supps[2].Id, UnitPrice= 27, Quantity = 93, UnitOfMeasure = "olonkas", LeadTime = 3},
                new PurchaseOrderLine { RawMaterialId = rms[3].Id, SupplierId = supps[3].Id, UnitPrice= 28, Quantity = 94, UnitOfMeasure = "cups", LeadTime = 2},
                new PurchaseOrderLine { RawMaterialId = rms[4].Id, SupplierId = supps[4].Id, UnitPrice= 29, Quantity = 95, UnitOfMeasure = "container", LeadTime = 2},
                new PurchaseOrderLine { RawMaterialId = rms[1].Id, SupplierId = supps[2].Id, UnitPrice= 30, Quantity = 96, UnitOfMeasure = "kg", LeadTime = 1}
            };
            po.Lines = poLines;

            return po;
        }

    }
}