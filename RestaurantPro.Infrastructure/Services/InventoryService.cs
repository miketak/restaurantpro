using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using RestaurantPro.Core;
using RestaurantPro.Core.Domain;
using RestaurantPro.Core.Services;

namespace RestaurantPro.Infrastructure.Services
{
    public class InventoryService : RestProService, IInventoryService
    {
        private User _user;

        public InventoryService(IUnitOfWork unitOfWork)
            : base(unitOfWork)
        {
        }

        /// <summary>
        /// Transfers WorkCycle with Items to New Purchase Order
        /// Part of Workflow: WorkCycle -> PurchaseOrder
        /// </summary>
        /// <param name="workCycleId"></param>
        /// <param name="user"></param>
        public void ConfirmWorkCycle(int workCycleId, User user)
        {
            ValidateParameters(workCycleId, user);

            _user = user;

            var fullWorkCycleFromDb = ActivateWorkCycle(workCycleId);

            var purchaseOrderToDb = CreateNewPurchaseOrderForInsert(fullWorkCycleFromDb);

            _unitOfWork.PurchaseOrders.AddPurchaseOrder(purchaseOrderToDb);
        }

        #region Private Methods

        private static void ValidateParameters(int workCycleId, User user)
        {
            if (user == null) throw new ArgumentNullException("user");
            if (workCycleId <= 0) throw new ArgumentOutOfRangeException("workCycleId");
        }

        private WorkCycle ActivateWorkCycle(int workCycleId)
        {
            var fullWorkCycleFromDb = _unitOfWork.WorkCycles.GetWorkCycleById(workCycleId, true);
            fullWorkCycleFromDb.StatusId = WorkCycleStatus.Active.ToString();
            _unitOfWork.Complete();
            return fullWorkCycleFromDb;
        }

        private PurchaseOrder CreateNewPurchaseOrderForInsert(WorkCycle fullWorkCycleFromDb)
        {
            var purchaseOrderToDb = new PurchaseOrder
            {
                PurchaseOrderNumber = GenerateNewPurchaseOrder(),
                DateCreated = DateTime.Now,
                CreatedBy = _user.Id,
                StatusId = PurchaseOrderStatus.New.ToString(),
                WorkCycleId = fullWorkCycleFromDb.Id,
                Active = true,
                Lines = fullWorkCycleFromDb.WorkCycleLines.Select(wcLine => new PurchaseOrderLine
                    {
                        SupplierId = wcLine.WorkCycleId,
                        RawMaterialId = wcLine.RawMaterialId,
                        UnitPrice = wcLine.UnitPrice,
                        Quantity = wcLine.PlannedQuantity,
                        UnitOfMeasure = wcLine.UnitOfMeasure
                    })
                    .ToList()
            };
            return purchaseOrderToDb;
        }

        #endregion


    }
}