using System;
using System.Collections.Generic;
using System.Linq;

namespace RestaurantPro.Core.Domain
{
    public class PurchaseOrder
    {
        public int Id { get; set; }

        public string PurchaseOrderNumber { get; set; }

        public DateTime DateCreated { get; set; }

        public int CreatedBy { get; set; }

        public string StatusId { get; set; }

        public int? WorkCycleId { get; set; }

        public bool Active { get; set; }

        public IEnumerable<PurchaseOrderLine> Lines { get; set; }


        #region Equality Methods

        protected bool Equals(PurchaseOrder other)
        {
            var result = Id == other.Id && string.Equals(PurchaseOrderNumber, other.PurchaseOrderNumber) &&
                   DateCreated.Equals(other.DateCreated) && CreatedBy == other.CreatedBy &&
                   string.Equals(StatusId, other.StatusId) && WorkCycleId == other.WorkCycleId &&
                   Active == other.Active;
            
            if (other.Lines.Select(lineInOther => Lines.All(lineInOther.Equals)).Any(isEqual => !isEqual))
                result = false;

            return result;
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            if (obj.GetType() != GetType()) return false;
            return Equals((PurchaseOrder) obj);
        }

        public override int GetHashCode()
        {
            unchecked
            {
                var hashCode = Id;
                hashCode = (hashCode * 397) ^ (PurchaseOrderNumber != null ? PurchaseOrderNumber.GetHashCode() : 0);
                hashCode = (hashCode * 397) ^ DateCreated.GetHashCode();
                hashCode = (hashCode * 397) ^ CreatedBy;
                hashCode = (hashCode * 397) ^ (StatusId != null ? StatusId.GetHashCode() : 0);
                hashCode = (hashCode * 397) ^ WorkCycleId.GetHashCode();
                hashCode = (hashCode * 397) ^ Active.GetHashCode();
                hashCode = (hashCode * 397) ^ (Lines != null ? Lines.GetHashCode() : 0);
                return hashCode;
            }
        }

        #endregion

       


        #region Navigation Properties

        public virtual User User { get; set; }

        public virtual PoStatus Status { get; set; }

        public virtual WorkCycle WorkCycle { get; set; }

        public virtual ICollection<PurchaseOrderLine> PurchaseOrderLines { get; set; }

        #endregion
    }
}