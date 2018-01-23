using System;
using RestaurantPro.Core;
using RestaurantPro.Infrastructure;

namespace RestaurantPro.Models
{
    /// <summary>
    /// Pending Items for Procure Purchase Order View
    /// </summary>
    public class ProcurementItem : ValidatableBindableBase
    {
        private readonly IUnitOfWork _unitOfWork = new UnitOfWork(new RestProContext());
        public event Action<ProcurementItem> ReceivedQuantityChanged = delegate { };

        private double _receivedQuantity;
        private double _pendingQuantity;
        private string _deliveredBy;
        private double _receivedQuantityAdjustment;

        public int RawMaterialId { get; set; }
        public int SupplierId { get; set; }
        public int WorkCycleId { get; set; }
        public int PurchaseOrderId { get; set; }
        public double OrderedQuantity { get; set; }

        public DateTime DateReceived { get; set; }

        public double PendingQuantity
        {
            get { return _pendingQuantity; }
            set { SetProperty(ref _pendingQuantity, value); }
        }

        public double ReceivedQuantityAdjustment
        {
            get { return _receivedQuantityAdjustment; }
            set
            {
                SetProperty(ref _receivedQuantityAdjustment, value);
                CalculatePendingQuantity();
            }
        }

        public double TotalValue { get; set; }

        public double ReceivedQuantity
        {
            get { return _receivedQuantity; }
            set
            {
                SetProperty(ref _receivedQuantity, value);
                CalculatePendingQuantity();
                ReceivedQuantityChanged(this);
            }
        }

        public string DeliveredBy
        {
            get { return _deliveredBy; }
            set { SetProperty(ref _deliveredBy, value); }
        }

        #region Display Properties

        public string RawMaterial
        {
            get
            {
                try
                {
                    return _unitOfWork.RawMaterials.SingleOrDefault(r => r.Id == RawMaterialId).Name ?? "Error: Not found";
                }
                catch (Exception e)
                {
                    Console.WriteLine(e);
                    return "Error: Not found\n" + e.Message;
                }
            }
        }

        public string Supplier
        {
            get
            {
                try
                {
                    return _unitOfWork.Suppliers.SingleOrDefault(s => s.Id == SupplierId).Name ?? "Error: Not found";
                }
                catch (Exception e)
                {
                    Console.WriteLine(e);
                    return "Error: Not found\n" + e.Message;
                }
            }
        }

        public string WorkCycle
        {
            get
            {
                try
                {
                    return _unitOfWork.WorkCycles.SingleOrDefault(s => s.Id == WorkCycleId).Name ?? "Error: Not found";
                }
                catch (Exception e)
                {
                    Console.WriteLine(e);
                    return "Error: Not found\n" + e.Message;
                }
            }
        }

        public string PurchaseOrderNumber
        {
            get
            {
                try
                {
                    return _unitOfWork.PurchaseOrders.SingleOrDefault(s => s.Id == PurchaseOrderId).PurchaseOrderNumber ?? "Error: Not found";
                }
                catch (Exception e)
                {
                    Console.WriteLine(e);
                    return "Error: Not found\n" + e.Message;
                }
            }
        }

        #endregion

        #region Calculations

        private void CalculatePendingQuantity()
        {
            var tryPendingQuantity = OrderedQuantity - (ReceivedQuantity + ReceivedQuantityAdjustment);

            if (tryPendingQuantity < 0)
                ReceivedQuantity = 0;
            else
                PendingQuantity = tryPendingQuantity;
        }

        #endregion
    }
}