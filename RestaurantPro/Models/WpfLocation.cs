namespace RestaurantPro.Models
{
    public class WpfLocation : ValidatableBindableBase
    {
        private string _locationId;

        public string LocationId
        {
            get { return _locationId; }
            set { SetProperty(ref _locationId, value); }
        }
    }
}