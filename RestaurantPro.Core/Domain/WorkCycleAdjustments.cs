using System;

namespace RestaurantPro.Core.Domain
{
    public class WorkCycleAdjustments
    {
        public int Id { get; set; }

        public int WorkCycleId { get; set; }

        public double AdjustedPlanningQty { get; set; }

        public DateTime TransactionDate { get; set; }

        public int CreatedBy { get; set; }

        #region Navigation Properties

        public virtual WorkCycle WorkCycle { get; set; }

        public virtual User User { get; set; }

        #endregion
    }
}