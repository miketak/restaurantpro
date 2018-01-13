using System;

namespace RestaurantPro.Core.Domain
{
    public class WorkCycleTransaction
    {
        public int Id { get; set; }

        public int WorkCycleId { get; set; }

        public int RawMaterialId { get; set; }

        public int TrackingNumber { get; set; }

        public double UsedQuantity { get; set; }

        public DateTime DateUsed { get; set; }

        public int CreatedBy { get; set; }

        public DateTime TransactionDate { get; set; }

        #region Navigation Properties

        public virtual WorkCycle WorkCycle { get; set; }

        public virtual RawMaterial RawMaterial { get; set; }

        public virtual User User { get; set; }

        #endregion
    }
}