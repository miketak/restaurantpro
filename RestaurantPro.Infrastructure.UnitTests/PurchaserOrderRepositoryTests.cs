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
        private const string PurchaseOrderNumber = "105-K1";
        private const string PurchaseOrderNumber2 = "105-K2";



        public PurchaserOrderRepositoryTests()
        {
            _unitOfWork = new UnitOfWork(new RestProContext());
        }

        private PurchaseOrder GetPurchaseOrder(string purchaseOrderNumber, bool isActive)
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
                new PurchaseOrderLine { RawMaterialId = rms[0].Id, SupplierId = supps[0].Id, UnitPrice= 25, Quantity = 91, UnitOfMeasure = "crates"},
                new PurchaseOrderLine { RawMaterialId = rms[1].Id, SupplierId = supps[1].Id, UnitPrice= 26, Quantity = 92, UnitOfMeasure = "bags"},
                new PurchaseOrderLine { RawMaterialId = rms[2].Id, SupplierId = supps[2].Id, UnitPrice= 27, Quantity = 93, UnitOfMeasure = "olonkas"},
                new PurchaseOrderLine { RawMaterialId = rms[3].Id, SupplierId = supps[3].Id, UnitPrice= 28, Quantity = 94, UnitOfMeasure = "cups"},
                new PurchaseOrderLine { RawMaterialId = rms[4].Id, SupplierId = supps[4].Id, UnitPrice= 29, Quantity = 95, UnitOfMeasure = "container"},
                new PurchaseOrderLine { RawMaterialId = rms[1].Id, SupplierId = supps[2].Id, UnitPrice= 30, Quantity = 96, UnitOfMeasure = "kg"}
            };
            po.Lines = poLines;

            return po;
        }

        [TestMethod]
        public void AddPurchaseOrderTestWithTrueAndFalseActiveFlag()
        {
            var checkIfInDatabase = _unitOfWork
                .PurchaseOrders
                .SingleOrDefault(x => x.PurchaseOrderNumber == PurchaseOrderNumber);

            if (checkIfInDatabase != null)
            {
                Assert.IsNotNull(checkIfInDatabase);
                return;
            }

            //With active flag
            var purchaseOrder = GetPurchaseOrder(PurchaseOrderNumber, true);
            _unitOfWork.PurchaseOrders.AddPurchaseOrder(purchaseOrder);

            //Add with false active flag
            purchaseOrder = GetPurchaseOrder(PurchaseOrderNumber2, false);
            _unitOfWork.PurchaseOrders.AddPurchaseOrder(purchaseOrder);

            Assert.AreEqual(1,1);
        }

        [TestMethod]
        public void GetPurchaseOrderByPurchaseOrderNumberWithTrueActiveFlag()
        {
            var purchaseOrder = _unitOfWork.PurchaseOrders.GetPurchaseOrderByPurchaseOrderNumber(PurchaseOrderNumber, true);

            const int expectedLineCount = 6;

            Assert.AreEqual(PurchaseOrderNumber, purchaseOrder.PurchaseOrderNumber);

            Assert.AreEqual(expectedLineCount, purchaseOrder.PurchaseOrderLines.Count);
        }
        
        [TestMethod]
        public void GetPurchaseOrderByPurchaseOrderNumberWithTrueActiveFlagUsingFalseFlag()
        {
            var purchaseOrder = _unitOfWork.PurchaseOrders.GetPurchaseOrderByPurchaseOrderNumber(PurchaseOrderNumber, false);

            Assert.IsNull(purchaseOrder);
        }

        [TestMethod]
        public void GetPurchaseOrderByPurchaseOrderNumberWithFalseActiveFlag()
        {
            var purchaseOrder = _unitOfWork.PurchaseOrders.GetPurchaseOrderByPurchaseOrderNumber(PurchaseOrderNumber2, false);

            if (purchaseOrder == null)
                throw new AssertFailedException("Wrong active bit behaviour");

            const int expectedLineCount = 6;

            Assert.AreEqual(PurchaseOrderNumber2, purchaseOrder.PurchaseOrderNumber);

            Assert.AreEqual(expectedLineCount, purchaseOrder.PurchaseOrderLines.Count);
        }

        [TestMethod]
        public void GetPurchaseOrderByIdWithTrueActiveFlag()
        {
            var purchaseOrderInDb =
                _unitOfWork.PurchaseOrders.SingleOrDefault(c => c.PurchaseOrderNumber == PurchaseOrderNumber);

            const int expectedLineCount = 6;

            var purchaseOrder = _unitOfWork.PurchaseOrders.GetPurchaseOrderById(purchaseOrderInDb.Id, true);

            Assert.AreEqual(PurchaseOrderNumber, purchaseOrder.PurchaseOrderNumber);

            Assert.AreEqual(expectedLineCount, purchaseOrder.PurchaseOrderLines.Count);
        }

        [TestMethod]
        public void GetPurchaseOrderByIdWithFalseActiveFlag()
        {
            var purchaseOrderInDb =
                _unitOfWork.PurchaseOrders.SingleOrDefault(c => c.PurchaseOrderNumber == PurchaseOrderNumber2);

            const int expectedLineCount = 6;

            var purchaseOrder = _unitOfWork.PurchaseOrders.GetPurchaseOrderById(purchaseOrderInDb.Id, false);

            if (purchaseOrder == null)
                throw new AssertFailedException("Wrong active bit behaviour");

            Assert.AreEqual(PurchaseOrderNumber2, purchaseOrder.PurchaseOrderNumber);

            Assert.AreEqual(expectedLineCount, purchaseOrder.PurchaseOrderLines.Count);
        }

    }
}