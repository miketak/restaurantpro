using System;
using System.Collections.Generic;
using System.Linq;
using RestaurantPro.Core;
using RestaurantPro.Core.Domain;

namespace RestaurantPro.Infrastructure.Services
{
    public class InventoryTransactions
    {
        private readonly IUnitOfWork _unitOfWork;
        private int _currentTrackingNumber;
        private static Random random = new Random();

        public InventoryTransactions(IUnitOfWork unitOfWork)
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