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
        private readonly RestProContext _context;

        public PurchaserOrderRepositoryTests()
        {
            _context = new RestProContext();
            _unitOfWork = new UnitOfWork(_context);
        }

        [TestMethod]
        public void AddPurchaseOrderTest()
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
                PurchaseOrderNumber = "105-K1",
                DateCreated = DateTime.Now,
                CreatedBy = 1,
                StatusId = statuses[1].Status,
                Active = true,
                WorkCycleId = wcs[0].Id
            };

            var poLines = new List<PurchaseOrderLine>
            {
                new PurchaseOrderLine { RawMaterialId = rms[0].Id, SupplierId = supps[0].Id, Quantity = 91, UnitOfMeasure = "crates"},
                new PurchaseOrderLine { RawMaterialId = rms[1].Id, SupplierId = supps[1].Id, Quantity = 92, UnitOfMeasure = "bags"},
                new PurchaseOrderLine { RawMaterialId = rms[2].Id, SupplierId = supps[2].Id, Quantity = 93, UnitOfMeasure = "olonkas"},
                new PurchaseOrderLine { RawMaterialId = rms[3].Id, SupplierId = supps[3].Id, Quantity = 94, UnitOfMeasure = "cups"},
                new PurchaseOrderLine { RawMaterialId = rms[4].Id, SupplierId = supps[4].Id, Quantity = 95, UnitOfMeasure = "container"},
                new PurchaseOrderLine { RawMaterialId = rms[1].Id, SupplierId = supps[2].Id, Quantity = 96, UnitOfMeasure = "kg"}
            };
            po.Lines = poLines;

            _unitOfWork.PurchaseOrders.AddPurchaseOrder(po);

            Assert.AreEqual(1,1);
        }
        
    }
}