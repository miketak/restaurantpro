using System;
using RestaurantPro.Core.Domain;

namespace RestaurantPro.Models
{
    public class WpfWorkCycle
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public DateTime DateBegin { get; set; }

        public DateTime DateEnd { get; set; }

        public bool Active { get; set; }

        public virtual User User { get; set; }

        public int UserId { get; set; }


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