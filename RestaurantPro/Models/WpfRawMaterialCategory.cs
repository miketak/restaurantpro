namespace RestaurantPro.Models
{
    public class WpfRawMaterialCategory : ValidatableBindableBase
    {
        public int Id { get; set; }

        private string _name;

        public string Name
        {
            get { return _name; }
            set { SetProperty(ref _name, value);}
        }


        private string _description;
        public string Description
        {
            get { return _description; }
            set { SetProperty(ref _description, value); }
        }
    }
}