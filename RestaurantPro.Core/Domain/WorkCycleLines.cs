using System;

namespace RestaurantPro.Core.Domain
{
    public class WorkCycleLines
    {
        public int WorkCycleId { get; set; }

        public int RawMaterialId { get; set; }

        public int SupplierId { get; set; }

        public float Quantity { get; set; }

        public float CurrentQuanity { get; set; }

        public string UnitOfMeasure { get; set; }

        public bool WasMoved { get; set; }

        public DateTime MoveDate { get; set; }

        public string LocationId { get; set; }


        #region Navigation Properties

        public virtual WorkCycle WorkCycle { get; set; }

        public virtual RawMaterial RawMaterial { get; set; }

        public virtual Supplier Supplier { get; set; }

        public virtual Location Location { get; set; }

        #endregion
    }
}