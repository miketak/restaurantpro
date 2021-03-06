﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using RestaurantPro.Core;
using RestaurantPro.Core.Domain;
using RestaurantPro.Core.Services;

namespace RestaurantPro.Infrastructure.Services
{
    public class InventoryService : RestProServiceBase, IInventoryService
    {
        private readonly IUnitOfWork _unitOfWork;
        private User _user;

        public InventoryService(IUnitOfWork unitOfWork)
            : base(unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        /// <summary>
        ///     Transfers WorkCycle with Items to New Purchase Order
        ///     Part of Workflow: WorkCycle -> PurchaseOrder
        /// </summary>
        /// <param name="workCycleId"></param>
        /// <param name="user"></param>
        public void ConfirmWorkCycle(int workCycleId, User user)
        {
            ValidateParameters(workCycleId, user);

            if (IsOtherWorkCycleActive())
                throw new ApplicationException("Pending Work Cycle Active");

            _user = user;

            var activeWorkCycleFromDb = ActivateWorkCycle(workCycleId);

            InventoryTransactionsService.AddWorkCycleTransactions(activeWorkCycleFromDb);

            var purchaseOrderToDb = CreateNewPurchaseOrderForInsert(activeWorkCycleFromDb);

            _unitOfWork.PurchaseOrders.AddPurchaseOrder(purchaseOrderToDb);
        }

        private bool IsOtherWorkCycleActive()
        {
            return _unitOfWork.WorkCycles.CheckForActiveStatusWorkCycles();
        }

        /// <summary>
        ///     Updates Purchase Order Transactiosn and Work Cycle Transactionsw
        /// </summary>
        /// <param name="oldPurchaseOrder"></param>
        /// <param name="purchaseOrderTransactions"></param>
        public void ProcurePurchaseOrder(PurchaseOrder oldPurchaseOrder,
            IEnumerable<PurchaseOrderTransaction> purchaseOrderTransactions, User user)
        {
            ValidateParameters(oldPurchaseOrder, purchaseOrderTransactions, user);

            var newPurchaseOrderTransactions = purchaseOrderTransactions.ToList();

            _user = user;

            ConcurrencyCheckOnDatabase(oldPurchaseOrder);

            ChangePurchaseOrderStatusToInProgress(oldPurchaseOrder);

            InventoryTransactionsService.AddPurchaseOrderTransactions(newPurchaseOrderTransactions.ToList());

            InventoryTransactionsService.AddWorkCycleTransactions(oldPurchaseOrder.WorkCycleId, user);

            AssignLocationInWorkCycles(oldPurchaseOrder.WorkCycleId, newPurchaseOrderTransactions.ToList());

            AddOrUpdateItemsToStock(newPurchaseOrderTransactions);

            InventoryTransactionsService.AddInventoryTransactions(TransformToITL(newPurchaseOrderTransactions.ToList()), _user);
        }

        public IEnumerable<PurchaseOrderInformation> GetPurchaseOrderInformation()
        {
            var poNumbers = _unitOfWork.PurchaseOrders.GetAll()
                .Where(a => a.Active)
                .Where(p => p.StatusId != PurchaseOrderStatus.Closed.ToString())
                .Select(p => p.PurchaseOrderNumber)
                .ToList();

            var purchaseOrders = poNumbers
                .Select(poNumber => _unitOfWork.PurchaseOrders.GetPurchaseOrderByPurchaseOrderNumber(poNumber, true)).ToList();

            var purchaseOrderInformationEntities = new List<PurchaseOrderInformation>();

            foreach (var purchaseOrder in purchaseOrders)
            {
                purchaseOrderInformationEntities.AddRange(purchaseOrder.PurchaseOrderLines
                    .Select(line => new PurchaseOrderInformation
                {
                    RawMaterialId = line.RawMaterialId,
                    SupplierId = line.SupplierId,
                    PurchaseOrderId = line.PurchaseOrderId,
                    WorkCycleId = purchaseOrder.WorkCycleId ?? 0,
                    OrderedQuantity = line.Quantity,
                    PendingQuantity = CalculatePurchaseOrderItemPendingQuantity(line),
                    TotalValue = (decimal) line.UnitPrice * (decimal) line.Quantity
                }));
            }

            return purchaseOrderInformationEntities;
        }

        private double CalculatePurchaseOrderItemPendingQuantity(PurchaseOrderLine line)
        {
            var quantityReceivedAlready = _unitOfWork.PurchaseOrderTransactions.GetAll()
                .Where(p => p.PurchaseOrderId == line.PurchaseOrderId &&
                            p.RawMaterialId == line.RawMaterialId &&
                            p.SupplierId == line.SupplierId)
                .Select(p => p.QuantityReceived)
                .Sum();

            var pendingQuantity = line.Quantity - quantityReceivedAlready;

            return pendingQuantity;
        }

        private void AddOrUpdateItemsToStock(IEnumerable<PurchaseOrderTransaction> newPurchaseOrderTransactions)
        {
            if (newPurchaseOrderTransactions == null) throw new ArgumentNullException("newPurchaseOrderTransactions");

            foreach (var transaction in newPurchaseOrderTransactions.ToList())
            {
                var rawMaterialInDb =
                    _unitOfWork.Inventory.SingleOrDefault(r => r.RawMaterialId == transaction.RawMaterialId);

                if (rawMaterialInDb == null)
                {
                    _unitOfWork.Inventory.Add(new Inventory
                    {
                        RawMaterialId = transaction.RawMaterialId,
                        InitialQuantity = transaction.QuantityReceived,
                        CurrentQuantity = transaction.QuantityReceived
                    });
                    _unitOfWork.Complete();
                }
                else
                {
                    rawMaterialInDb.InitialQuantity += transaction.QuantityReceived;
                    rawMaterialInDb.CurrentQuantity += transaction.QuantityReceived;
                    _unitOfWork.Complete();
                }
            }
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
                PurchaseOrderNumber = GenerateNewPurchaseOrderNumber(),
                DateCreated = DateTime.Now,
                CreatedBy = _user.Id,
                StatusId = PurchaseOrderStatus.New.ToString(),
                WorkCycleId = fullWorkCycleFromDb.Id,
                Active = true,
                Lines = fullWorkCycleFromDb.WorkCycleLines.Select(wcLine => new PurchaseOrderLine
                    {
                        SupplierId = wcLine.SupplierId,
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

        private static void ValidateParameters(PurchaseOrder oldPurchaseOrder,
            IEnumerable<PurchaseOrderTransaction> newPurchaseOrderTransactions, User user)
        {
            if (oldPurchaseOrder == null) throw new ArgumentNullException("oldPurchaseOrder");
            if (newPurchaseOrderTransactions == null) throw new ArgumentNullException("newPurchaseOrderTransactions");
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

        private void AssignLocationInWorkCycles(int? workCycleId, List<PurchaseOrderTransaction> poTransactions)
        {
            var workCycleLinesToChange = _unitOfWork.WorkCyclesLines
                .GetAll().Where(x => x.WorkCycleId == workCycleId).ToList();

            foreach (var wcLine in workCycleLinesToChange)
            foreach (var transaction in poTransactions)
                wcLine.LocationId = transaction.LocationId;

            _unitOfWork.Complete();
        }

        private List<InventoryTransaction> TransformToITL(IEnumerable<PurchaseOrderTransaction> newPurchaseOrderTransactions)
        {
            var invItems = newPurchaseOrderTransactions.ToList()
                .Select(item => new InventoryTransaction
                {
                    InventoryId = _unitOfWork.Inventory.SingleOrDefault(x => x.RawMaterialId == item.RawMaterialId).Id,
                    Quantity = item.QuantityReceived,
                    CreatedBy = _user.Id
                })
                .ToList();
            return invItems;
        }

        #endregion

    }
}