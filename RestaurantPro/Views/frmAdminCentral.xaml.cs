using DataObjects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace RestaurantPro
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
        /// <param name="user"></param>
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
