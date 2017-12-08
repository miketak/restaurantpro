using BusinessLogic;
using DataObjects;
using System;
using System.Collections.Generic;
using System.Data;
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
    /// Interaction logic for subfrmManageEmployee.xaml
    /// </summary>
    public partial class subfrmManageEmployee 
    {
        /// <summary>
        /// Current user logged into system.
        /// </summary>
        User _user = null;

        /// <summary>
        /// List of active employee objects
        /// </summary>
        private List<Employee> _activeEmployees;

        /// <summary>
        /// List of inactive employee objects
        /// </summary>
        private List<Employee> _inactiveEmployees;


        /// <summary>
        /// Initialize using current User settings
        /// </summary>
        /// <param name="user"></param>
        public subfrmManageEmployee(User user)
        {
            _user = user;
            InitializeComponent();
            

        }

        /// <summary>
        /// Toggle to Edit Mode on Edit Form
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void globalEmployeeList_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {

        }


        private void btnSearch(object sender, RoutedEventArgs e)
        {

        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            txtName.Text = _user.FirstName + " " + _user.LastName;
            RefreshEmployees();

        }

        /// <summary>
        /// Loads employees from database both active and inactive.
        /// </summary>
        private void RefreshEmployees()
        {
            var employeeManager = new EmployeeManager();
            _activeEmployees = employeeManager.RetrieveEmployees(true);
            _inactiveEmployees = employeeManager.RetrieveEmployees(false);
            globalEmployeeList.ItemsSource = _activeEmployees.Concat(_inactiveEmployees);

            //option for active employees

            //option for inactive employees
        }

        /// <summary>
        /// Opens Create Update Employee in Add mode
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnAdd(object sender, RoutedEventArgs e)
        {
            //Open in Add Mode
            var subfrmCreateUpdateDelete = new subfrmCreateUpdateEmployee(_user);
            subfrmCreateUpdateDelete.ShowDialog();
        }

        /// <summary>
        /// Action Event Handler for Edit
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnEdit(object sender, RoutedEventArgs e)
        {
            //Get username from datagrid
            var selectedEmployee = (Employee)globalEmployeeList.SelectedItem;
            var username = selectedEmployee.Username;
            
            //Open in Edit Mode
            var subfrmCreateUpdateDelete = new subfrmCreateUpdateEmployee(_user, username);
            subfrmCreateUpdateDelete.ShowDialog();

        }
 
    }
}
