using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using RestaurantPro.Core;
using RestaurantPro.Core.Domain;
using RestaurantPro.Infrastructure;

namespace RestaurantPro.Models
{
    /// <summary>
    /// Purchase Order DTOs
    /// </summary>
    public class WpfPurchaseOrder : ValidatableBindableBase
    {
        private readonly IUnitOfWork _unitOfWork = new UnitOfWork(new RestProContext());// will be removed
        
        public int Id { get; set; }

        private string _purchaseOrderNumber;
        public string PurchaseOrderNumber
        {
            get { return _purchaseOrderNumber; }
            set { SetProperty(ref _purchaseOrderNumber, value); }
        }


        private DateTime _dateCreated;
        public DateTime DateCreated
        {
            get { return _dateCreated; }
            set { SetProperty(ref _dateCreated, value); }
        }

        public int CreatedBy { get; set; }

        private string _statusId;
        public string StatusId
        {
            get { return _statusId; }
            set { SetProperty(ref _statusId, value); }
        }

        public int WorkCycleId { get; set; }

        public bool Active { get; set; }

        private BindingList<WpfPurchaseOrderLine> _purchaseOrderLine;
        public BindingList<WpfPurchaseOrderLine> Lines
        {
            get { return _purchaseOrderLine; }
            set
            {
                SetProperty(ref _purchaseOrderLine, value);
                UpdateSubTotalSection();
            }
        }

        #region Purchase Order View Properties

        public List<string> StatusForCombo
        {
            get
            {
                return _unitOfWork.Statuses.GetAll()
                    .Select(s => s.Status).ToList();
            }
        }

        public string DateCreatedForView
        {
            get { return DateCreated.ToShortDateString(); }
        }

        public string FullName
        {
            get
            {
                var createdByUser = _unitOfWork.Users.SingleOrDefault(u => u.Id == CreatedBy);

                if (createdByUser == null)
                    return "Unknown";

                return createdByUser.FirstName + " " + createdByUser.LastName;
            }
        }

        public string WorkCycleName
        {
            get
            {
                var workCycle =  _unitOfWork.WorkCycles.SingleOrDefault(wc => wc.Id == WorkCycleId);

                return workCycle == null ? "N/A" : workCycle.Name;
            }
        }

        #endregion

        #region SubTotal Section Fields

        private double _subTotal;
        public double SubTotal
        {
            get { return _subTotal; }
            set { SetProperty(ref _subTotal, value); }
        }

        public double Tax { get { return 0.175; } } //to be abstracted to db later

        private double _total;
        public double Total
        {
            get { return _total; }
            set { SetProperty(ref _total, value); }
        }

        #endregion

        #region Sub Totals Calculation Section

        public void UpdateSubTotalSection()
        {
            CalculateSubTotal();
            CalculateTotal();
        }

        private void CalculateSubTotal()
        {
            SubTotal = Lines.Sum(a => a.GetLineTotal());
        }

        private void CalculateTotal()
        {
            Total = SubTotal + SubTotal * Tax;
        }

        #endregion

    }
}