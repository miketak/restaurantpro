using System;
using System.Collections.Generic;
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

        public DateTime DateCreated { get; set; }

        public int CreatedBy { get; set; }

        public string StatusId { get; set; }

        public int WorkCycleId { get; set; }

        public bool Active { get; set; }

        public IEnumerable<PurchaseOrderLine> Lines { get; set; }

        #region Purchase Order List View Properties

        public string DateCreatedForView
        {
            get { return DateCreated .ToShortDateString(); }
        }

        public string Status
        {
            get { return StatusId;  }
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

                return workCycle == null ? "Not found" : workCycle.Name;
            }
        }

        #endregion
    }
}