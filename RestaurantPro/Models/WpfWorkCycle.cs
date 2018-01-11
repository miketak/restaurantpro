using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using RestaurantPro.Core;
using RestaurantPro.Core.Domain;
using RestaurantPro.Infrastructure;

namespace RestaurantPro.Models
{
    public class WpfWorkCycle : ValidatableBindableBase
    {
        static IUnitOfWork _unitOfWork = new UnitOfWork(new RestProContext());

        public WpfWorkCycle()
        {
            DateBegin = DateTime.Today;
            DateEnd = DateTime.Today;
        }

        public int Id { get; set; }

        private string _name;
        [Required]
        public string Name
        {
            get { return _name; }
            set { SetProperty(ref _name, value); }
        }

        private DateTime _dateBegin;
        [Required]
        public DateTime DateBegin
        {
            get { return _dateBegin; }
            set { SetProperty(ref _dateBegin, value); }
        }

        private DateTime _dateEnd;
        [Required]
        public DateTime DateEnd
        {
            get { return _dateEnd; }
            set { SetProperty(ref _dateEnd, value); }
        }

        private bool _active;
        public bool Active
        {
            get { return _active; }
            set { SetProperty(ref _active, value); }
        }

        private int _userId;
        public int UserId
        {
            get { return _userId; }
            set { SetProperty(ref _userId, value); }
        }


        private string _statusId;
        public string StatusId
        {
            get { return _statusId; }
            set { SetProperty(ref _statusId, value); }
        }

        private BindingList<WpfWorkCycleLines> _workCycleLines;
        public BindingList<WpfWorkCycleLines> Lines
        {
            get { return _workCycleLines; }
            set
            {
                SetProperty(ref _workCycleLines, value);
                UpdateSubTotalSection();
            }
        }

        #region Properties for View

        public string FirstName { get; set; }

        public string LastName { get; set; }

        public string FullName
        {
            get { return FirstName + " " + LastName; }
        }

        public string DateBeginForView
        {
            get { return DateBegin.ToShortDateString(); }
        }

        public string DateEndForView
        {
            get { return DateEnd.ToShortDateString(); }
        }

        public string ActiveForView
        {
            get { return Active ? "Active" : "Inactive"; }
        }

        public static List<string> Statuses
        {
            get
            {
                if (_statuses == null)
                    Statuses = _unitOfWork
                        .WorkCycleStatuses
                        .GetAll().Select(a => a.Status).ToList();
                return _statuses;
            }
            set { _statuses = value;  }
        }

        private static List<string> _statuses;

        #endregion

        #region Subtotal Section Fields

        private double _subTotal;
        public double SubTotal
        {
            get { return _subTotal; }
            set { SetProperty(ref _subTotal, value); }
        }

        public double Tax {get { return (double)_unitOfWork.InventorySettings.GetTax(); }  } //to be abstracted to db later

        private double _total;

        public double Total
        {
            get { return _total; }
            set { SetProperty(ref _total, value); }
        }

        #endregion

        #region Private Calculation Methods

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