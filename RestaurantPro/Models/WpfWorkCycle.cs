using System;
using System.ComponentModel.DataAnnotations;
using RestaurantPro.Core.Domain;

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

       
    }
}