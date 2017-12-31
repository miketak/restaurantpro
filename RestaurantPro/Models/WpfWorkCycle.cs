using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using RestaurantPro.Core;
using RestaurantPro.Core.Domain;
using RestaurantPro.Infrastructure;

namespace RestaurantPro.Models
{
    public class WpfWorkCycle : ValidatableBindableBase
    {

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

        public virtual User User { get; set; }


        private int _userId;
        public int UserId
        {
            get { return _userId; }
            set { SetProperty(ref _userId, value); }
        }

        private List<WpfWorkCycleLines> _workCycleLines;
        public List<WpfWorkCycleLines> Lines
        {
            get { return _workCycleLines; }
            set {  SetProperty(ref _workCycleLines, value);}
        }

        #region Fields for Working Cycle Datagrid View 

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

        #endregion

        #region Subtotal Section Fields

        private double _subTotal;
        public double SubTotal
        {
            get
            {
                CalculateSubTotal();
                return _subTotal;
            }
            set { SetProperty(ref _subTotal, value); }
        }

        public double Tax{ get { return 0.175; }  } //to be abstracted to db later

        private double _total;
        public double Total
        {
            get
            {
                CalculateTotal();
                return _total;
            }
            set { SetProperty(ref _total, value); }
        }

        #endregion

        #region Private Calculation Methods

        public void CalculateSubTotal()
        {
            SubTotal = Lines.Sum(a => a.GetLineTotal());
        }

        public void CalculateTotal()
        {
            Total = SubTotal + SubTotal * Tax;
        }

        #endregion
    }
}