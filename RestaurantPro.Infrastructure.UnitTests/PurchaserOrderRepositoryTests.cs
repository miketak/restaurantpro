using System;
using System.Collections.Generic;
using System.Linq;
using NUnit.Framework;
using RestaurantPro.Core;
using RestaurantPro.Core.Domain;

namespace RestaurantPro.Infrastructure.UnitTests
{
    [TestFixture]
    public class PurchaserOrderRepositoryTests
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly string _firstTestPurchaseOrderNumber = "100";
        private const int ExpectedLineCount = 6;

        public PurchaserOrderRepositoryTests()
        {
            _unitOfWork = new UnitOfWork(new RestProContext());
        }

        [Test]
        public void GetPurchaseOrderByPurchaseOrderNumber_WhenCalledWithTrueFlag_ReturnsTrueActiveBitPurchaseOrder()
        {
            AddPurchaseOrderWithLines(_firstTestPurchaseOrderNumber, true);

            var purchaseOrder = _unitOfWork.PurchaseOrders.GetPurchaseOrderByPurchaseOrderNumber(_firstTestPurchaseOrderNumber, true);

            Assert.That(purchaseOrder.PurchaseOrderNumber, Is.EqualTo(_firstTestPurchaseOrderNumber));
        }        
        
        [Test]
        public void GetPurchaseOrderByPurchaseOrderNumber_WhenCalled_ReturnsPurchaseOrderWithLines()
        {
            AddPurchaseOrderWithLines(_firstTestPurchaseOrderNumber, true);

            var purchaseOrder = _unitOfWork.PurchaseOrders.GetPurchaseOrderByPurchaseOrderNumber(_firstTestPurchaseOrderNumber, true);

            Assert.That(purchaseOrder.PurchaseOrderLines.Count, Is.EqualTo(ExpectedLineCount));
        }
        
        [Test]
        public void GetPurchaseOrderByPurchaseOrderNumber_WhenCalledWithFalseFlag_ReturnsNoPurchaseOrderWithTrueFlag()
        {
            AddPurchaseOrderWithLines(_firstTestPurchaseOrderNumber, true);

            var purchaseOrder = _unitOfWork.PurchaseOrders.GetPurchaseOrderByPurchaseOrderNumber(_firstTestPurchaseOrderNumber, false);

            Assert.IsNull(purchaseOrder);
        }        
        
        [Test]
        public void GetPurchaseOrderByPurchaseOrderNumber_WhenCalledWithFalseFlag_ReturnsFalseActiveBitPurchaseOrder()
        {
            AddPurchaseOrderWithLines(_firstTestPurchaseOrderNumber, false);

            var purchaseOrder = _unitOfWork.PurchaseOrders.GetPurchaseOrderByPurchaseOrderNumber(_firstTestPurchaseOrderNumber, false);

            Assert.IsNotNull(purchaseOrder);
        }

        [TearDown]
        public void TearDown()
        {
            RemovePurchaseOrder(_firstTestPurchaseOrderNumber);
        }

        #region Helper Methods

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
                throw new AssertionException("No suppliers in Database");
            if (wcs.Length == 0)
                throw new AssertionException("No Working Cycles in Database");
            if (rms.Length == 0)
                throw new AssertionException("No Raw Materials in Database");
            if (statuses.Length == 0)
                throw new AssertionException("No Statuses in Database");

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

        #endregion

       

    }
}