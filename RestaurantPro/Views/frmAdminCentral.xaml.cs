using System.Windows;
using DataObjects;

namespace RestaurantPro.Views
{
    /// <summary>
    /// Interaction logic for frmAdminCentral.xaml
    /// </summary>
    public partial class frmAdminCentral
    {
        /// <summary>
        /// Current User
        /// </summary>
        User _user = null;

        /// <summary>
        /// Admin Central Constructor
        /// </summary>
        public frmAdminCentral()//User user)
        {
            InitializeComponent();
            //_user = user;
            //txtName.Text = _user.FirstName + " " + _user.LastName;
        }

        private void btnManageDepartment(object sender, RoutedEventArgs e)
        {

        }

        private void btnManageEmployees(object sender, RoutedEventArgs e)
        {
            var subfrmManageEmployee = new subfrmManageEmployee(_user);
            subfrmManageEmployee.Show();

        }

        private void btnLeaveSettings(object sender, RoutedEventArgs e)
        {

        }
    }
}
