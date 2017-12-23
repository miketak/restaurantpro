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
            _context.PurchaseOrders.Add(purchaseOrder);
            _context.SaveChanges();

            var purchaseOrderInDb = _context.PurchaseOrders
                .SingleOrDefault(po => po.PurchaseOrderNumber == purchaseOrder.PurchaseOrderNumber);

            if (purchaseOrderInDb != null)
            {
                purchaseOrder.Lines
                    .ToList()
                    .ForEach(p => p.PurchaseOrderId = purchaseOrderInDb.Id);
            }

            purchaseOrder.Lines.ToList().ForEach(p => _context.PurchaseOrderLines.Add(p));

            try
            {
                _context.SaveChanges();
            }
            catch (Exception e)
            {
                Debug.WriteLine(e.Message);

                //Remove Purchase Order
                if (purchaseOrderInDb != null)
                {
                    _context.PurchaseOrders.Remove(purchaseOrderInDb);
                    _context.SaveChanges();
                }
                throw e;
            }
            
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
    }
}