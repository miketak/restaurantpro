namespace RestaurantPro.Models
{
    public class InventorySettings : ValidatableBindableBase
    {
        private decimal _tax;
        public decimal Tax
        {
            get { return _tax; }
            set { SetProperty(ref _tax, value); }
        }
    }
}