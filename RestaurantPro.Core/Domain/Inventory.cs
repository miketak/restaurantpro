namespace RestaurantPro.Core.Domain
{
    public class Inventory
    {
        public int Id { get; set; }

        public int RawMaterialId { get; set; }

        public double InitialQuantity { get; set; }

        public double CurrentQuantity { get; set; }

        #region Navigation Properties

        public virtual RawMaterial RawMaterial { get; set; }

        #endregion
    }
}