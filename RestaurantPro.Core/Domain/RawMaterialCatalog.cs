namespace RestaurantPro.Core.Domain
{
    public class RawMaterialCatalog
    {
        public int Id { get; set; }

        public int RawMaterialId { get; set; }

        public int SupplierId { get; set; }

        public double Price { get; set; }

        public string UnitOfMeasure { get; set; }


        #region Navigation Properties

        public virtual RawMaterial RawMaterial { get; set; }

        public virtual Supplier Supplier { get; set; }

        #endregion
    }
}