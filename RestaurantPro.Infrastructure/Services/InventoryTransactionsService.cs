using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using RestaurantPro.Core;
using RestaurantPro.Core.Domain;

namespace RestaurantPro.Infrastructure.Services
{
    public class InventoryTransactionsService
    {
        private readonly IUnitOfWork _unitOfWork;

        private int _currentTrackingNumber;
        private static Random random = new Random();

        public InventoryTransactionsService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }


        internal void AddPurchaseOrderTransactions(IEnumerable<PurchaseOrderTransaction> purchaseOrderTransactions)
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

        internal void AddWorkCycleTransactions(int? workCycleId, User user)
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
                    WorkCycleId = (int)workCycleId,
                    RawMaterialId = pt.RawMaterialId,
                    TrackingNumber = _currentTrackingNumber,
                    UsedQuantity = pt.QuantityReceived,
                    DateUsed = pt.DateReceived,
                    CreatedBy = user.Id,
                    TransactionDate = pt.TransactionDate
                };
                _unitOfWork.WorkCycleTransactions.Add(workCycleTransaction);
                _unitOfWork.Complete();
            }
        }

        internal void AddWorkCycleTransactions(WorkCycle workCycle)
        {
            if (workCycle == null) throw new ArgumentNullException("workCycle");

            var workCycleInDb = _unitOfWork.WorkCycles
                .SingleOrDefault(wc => wc.Name == workCycle.Name);

            if (workCycle.Id != workCycleInDb.Id)
                throw new ApplicationException("Work Cycle Id mismatch");

            if ( workCycleInDb == null)
                throw new ApplicationException("Work Cycle not in database");

            var timeStamp = DateTime.Now;

            foreach (var line in workCycle.WorkCycleLines)
            {
                _unitOfWork.WorkCycleTransactions.Add(new WorkCycleTransaction
                {
                    WorkCycleId = workCycleInDb.Id,
                    RawMaterialId = line.RawMaterialId,
                    TrackingNumber = 0,
                    UsedQuantity = -1.0 * line.PlannedQuantity,
                    DateUsed = timeStamp,
                    CreatedBy = workCycle.UserId,
                    TransactionDate = timeStamp
                });
                _unitOfWork.Complete();
            }
        }

        internal void AddInventoryTransactions(IEnumerable<InventoryTransaction> inventoryItems, User user)
        {
            var transactionDate = DateTime.Now;
            foreach (var item in inventoryItems.ToList())
            {
                item.TransactionDate = transactionDate;
                _unitOfWork.InventoryTransactionRepository.Add(item);
                _unitOfWork.Complete();
            }
        }

        private int GenerateTrackingNumber()
        {
            var length = 6;
            const string chars = "0123456789";
            var val = new string(Enumerable.Repeat(chars, length)
                .Select(s => s[random.Next(s.Length)]).ToArray());

            var trackingNumbers = _unitOfWork.PurchaseOrderTransactions.GetAll()
                .Select(a => a.TrackingNumber).ToList();

            if (trackingNumbers.Contains(Int32.Parse(val)))
                GenerateTrackingNumber();

            return Int32.Parse(val);
        }
    }
}