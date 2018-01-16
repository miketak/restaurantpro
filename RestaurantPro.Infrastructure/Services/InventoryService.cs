using System;
using System.CodeDom;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Data.Entity;
using System.Linq;
using RestaurantPro.Core;
using RestaurantPro.Core.Domain;
using RestaurantPro.Core.Services;
using RestaurantPro.Infrastructure.Migrations;

namespace RestaurantPro.Infrastructure.Services
{
    public class InventoryService : RestProServiceBase, IInventoryService
    {
        private readonly IUnitOfWork _unitOfWork;
        private User _user;
        private int _currentTrackingNumber;

        public InventoryService(IUnitOfWork unitOfWork) 
            : base(unitOfWork)
        {
            _unitOfWork = unitOfWork;
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

        /// <summary>
        /// Updates Purchase Order Transactiosn and Work Cycle Transactionsw
        /// </summary>
        /// <param name="oldPurchaseOrder"></param>
        /// <param name="purchaseOrderTransactions"></param>
        public void ProcurePurchaseOrder(PurchaseOrder oldPurchaseOrder, IEnumerable<PurchaseOrderTransaction> purchaseOrderTransactions, User user)
        {
            ValidateParameters(oldPurchaseOrder, purchaseOrderTransactions, user);

            _user = user;

            ConcurrencyCheckOnDatabase(oldPurchaseOrder);

            ChangePurchaseOrderStatusToInProgress(oldPurchaseOrder);

            AddPurchaseOrderTransactions(purchaseOrderTransactions.ToList());

            AddWorkCycleTransactions(oldPurchaseOrder.WorkCycleId);
        }


        #region Confirm Cycle Helper Methods

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

        #region Procure Purchase Order Helper Methods

        private static void ValidateParameters(PurchaseOrder oldPurchaseOrder, IEnumerable<PurchaseOrderTransaction> newPurchaseOrder, User user)
        {
            if (oldPurchaseOrder == null) throw new ArgumentNullException("oldPurchaseOrder");
            if (newPurchaseOrder == null) throw new ArgumentNullException("newPurchaseOrder");
            if (user == null) throw new ArgumentNullException("user");
        }

        private void ConcurrencyCheckOnDatabase(PurchaseOrder oldPurchaseOrder)
        {
            if (oldPurchaseOrder == null) throw new ArgumentNullException("oldPurchaseOrder");

            var poId = _unitOfWork.PurchaseOrders
                .SingleOrDefault(x => x.Id == oldPurchaseOrder.Id).Id;
            var purchaseOrder = _unitOfWork.PurchaseOrders.GetPurchaseOrderById(poId, true);

            purchaseOrder.Lines = purchaseOrder.PurchaseOrderLines.ToList();

            if (!oldPurchaseOrder.Equals(purchaseOrder))
                throw new ApplicationException("Concurrency Error");
        }

         private void ChangePurchaseOrderStatusToInProgress(PurchaseOrder oldPurchaseOrder)
        {
            var purchaseOrder = _unitOfWork.PurchaseOrders.SingleOrDefault(x => x.Id == oldPurchaseOrder.Id);
            purchaseOrder.StatusId = "In Progress";
            _unitOfWork.Complete();
        }

        private void AddPurchaseOrderTransactions(List<PurchaseOrderTransaction> purchaseOrderTransactions)
        {
            _currentTrackingNumber = GenerateTrackingNumber();
            foreach (var pt in purchaseOrderTransactions)
            {
                pt.TrackingNumber = _currentTrackingNumber;
                pt.TransactionDate = DateTime.Now;
                _unitOfWork.PurchaseOrderTransactions.Add(pt);
                _unitOfWork.Complete();
            }
        }

        private void AddWorkCycleTransactions(int? workCycleId )
        {
            var workCycleTransaction = new WorkCycleTransaction();

            var purchaseOrderTransactions =
                _unitOfWork.PurchaseOrderTransactions.GetAll()
                .Where(t => t.TrackingNumber == _currentTrackingNumber)
                    .ToList();

            foreach (var pt in purchaseOrderTransactions)
            {
                workCycleTransaction = new WorkCycleTransaction
                {
                    WorkCycleId = (int) workCycleId,
                    RawMaterialId = pt.RawMaterialId,
                    TrackingNumber = _currentTrackingNumber,
                    UsedQuantity = pt.QuantityReceived,
                    DateUsed = pt.DateReceived,
                    CreatedBy = _user.Id,
                    TransactionDate = pt.TransactionDate
                };
                _unitOfWork.WorkCycleTransactions.Add(workCycleTransaction);
                _unitOfWork.Complete();
            }
        }

        #endregion


    }
}