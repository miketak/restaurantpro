namespace RestaurantPro.Models
{
    public class WpfSupplier : ValidatableBindableBase
    {
        public int Id { get; set; }

        private string _name;
        public string Name
        {
            get { return _name; }
            set { SetProperty(ref _name, value); }
        }

        private string _address;
        public string Address
        {
            get { return _address; }
            set { SetProperty(ref _address, value);}
        }

        private string _telephone;
        public string Telephone
        {
            get { return _telephone; }
            set { SetProperty(ref _telephone, value); }
        }

        private string _email;
        public string Email
        {
            get { return _email; }
            set { SetProperty(ref _email, value);}
        }

        public bool Active { get; set; }
    }
}