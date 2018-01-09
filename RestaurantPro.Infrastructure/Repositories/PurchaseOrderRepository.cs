using System;
using System.Data.Entity;
using System.Diagnostics;
using System.Linq;
using RestaurantPro.Core.Domain;
using RestaurantPro.Core.Repositories;

namespace RestaurantPro.Infrastructure.Repositories
{
    public class PurchaseOrderRepository : Repository<PurchaseOrder>, IPurchaseOrderRepository
    {
        private readonly RestProContext _context;

        public PurchaseOrderRepository(DbContext context) 
            : base(context)
        {
            _context = (RestProContext) context;
        }

        /// <summary>
        /// Adds new purchase order to database with
        /// purchase order lines
        /// </summary>
        /// <param name="purchaseOrder"></param>
        public void AddPurchaseOrder(PurchaseOrder purchaseOrder)
        {
            if (purchaseOrder.WorkCycleId == 0)
                purchaseOrder.WorkCycleId = null;

            var purchaseOrderInDb = _context.PurchaseOrders
                .SingleOrDefault(po => po.PurchaseOrderNumber == purchaseOrder.PurchaseOrderNumber);

            if ( purchaseOrderInDb != null)
                purchaseOrderInDb.StatusId = purchaseOrder.StatusId;

            _context.PurchaseOrders.Add(purchaseOrder);
            _context.SaveChanges();
            

            try
            {
                AddOrUpdateWorkingCycleLines(purchaseOrder);
            }
            catch (Exception e)
            {
                if (purchaseOrderInDb != null)
                {
                    _context.PurchaseOrders.Remove(purchaseOrderInDb);
                    _context.SaveChanges();
                }
                throw new ApplicationException("Database Error: " + e.Message);
            }
            
        }

        public void UpdatePurchaseOrder(PurchaseOrder purchaseOrder)
        {
            var purchaseOrderInDb = SingleOrDefault(x => x.PurchaseOrderNumber == purchaseOrder.PurchaseOrderNumber);
            purchaseOrderInDb.StatusId = purchaseOrder.StatusId;
            _context.SaveChanges();

            AddOrUpdateWorkingCycleLines(purchaseOrder);
        }

        /// <summary>
        /// Gets Purchase Order by purchase order number 
        /// based on active bit.
        /// </summary>
        /// <param name="isActive"></param>
        /// <param name="purchaseOrderNumber"></param>
        public PurchaseOrder GetPurchaseOrderByPurchaseOrderNumber(string purchaseOrderNumber, bool isActive)
        {
            return _context.PurchaseOrders
                .Include(po => po.PurchaseOrderLines)
                .Where(c => c.Active == isActive)
                .SingleOrDefault(c => c.PurchaseOrderNumber == purchaseOrderNumber);
        }

        /// <summary>
        /// Gets Purchase Order by purchase order id 
        /// based on active bit.
        /// </summary>
        /// <param name="isActive"></param>
        /// <param name="purchaseOrderId"></param>
        public PurchaseOrder GetPurchaseOrderById(int purchaseOrderId, bool isActive)
        {
                return _context.PurchaseOrders
                    .Include(po => po.PurchaseOrderLines)
                    .Where(c => c.Active == isActive)
                    .SingleOrDefault(c => c.Id == purchaseOrderId);
        }

        #region Private Helper Methods

        private void AddOrUpdateWorkingCycleLines(PurchaseOrder purchaseOrder)
        {
            if (!purchaseOrder.Lines.Any())
                return;

            if (purchaseOrder.WorkCycleId == 0)
                purchaseOrder.WorkCycleId = null;

            FlushWorkCycleLines(purchaseOrder);

            foreach (var line in purchaseOrder.Lines)
            {
                line.PurchaseOrderId = purchaseOrder.Id;
                bool isOldRawMaterialUsed = false;

                if (line.RawMaterialId == 0)
                {
                    line.RawMaterialId = CheckForOldRawMaterialsToActivate(line);
                    if (line.RawMaterialId != 0)
                        isOldRawMaterialUsed = true;
                }

                if (line.RawMaterialId == 0 && !isOldRawMaterialUsed)
                    line.RawMaterialId = AddNewRawMaterialToRawMaterialTable(line);

                if (line.SupplierId == 0)
                    line.SupplierId = AddNewSupplierToSupplierTable(line);

                _context.PurchaseOrderLines.Add(line);

                _context.SaveChanges();
            }
        }

        private int CheckForOldRawMaterialsToActivate(PurchaseOrderLine line)
        {
            var rawMaterialInDb = _context.RawMaterials.SingleOrDefault(r => r.Name == line.RawMaterialStringTemp);

            if (rawMaterialInDb == null)
                return 0;

            rawMaterialInDb.Active = true;
            _context.SaveChanges();

            return rawMaterialInDb.Id;
        }

        private int AddNewSupplierToSupplierTable(PurchaseOrderLine line)
        {
            _context.Suppliers.Add(new Supplier { Name = line.SupplierStringTemp, Active = true });
            _context.SaveChanges();
            return _context.Suppliers.SingleOrDefault(s => s.Name == line.SupplierStringTemp).Id;
        }

        private int AddNewRawMaterialToRawMaterialTable(PurchaseOrderLine line)
        {
            _context.RawMaterials.Add(new RawMaterial { Name = line.RawMaterialStringTemp, RawMaterialCategoryId = 1 });
            _context.SaveChanges();
            return _context.RawMaterials.SingleOrDefault(r => r.Name == line.RawMaterialStringTemp).Id;
        }

        private void FlushWorkCycleLines(PurchaseOrder purchaseOrder)
        {
            var linesToBeFlushed = _context.PurchaseOrderLines.Where(wc => wc.PurchaseOrderId == purchaseOrder.Id).ToList();
            _context.PurchaseOrderLines.RemoveRange(linesToBeFlushed);
            _context.SaveChanges();
        }



        #endregion
    }
}