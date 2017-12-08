using DataObjects;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using RestaurantPro.ViewModels;

namespace RestaurantPro.Views
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow 
    {
        
        internal readonly string failure = "Wrong username or password.";

        /// <summary>
        /// Current User
        /// </summary>
        User _user = null;

        /// <summary>
        /// Start Up Window for Program for User Authentication
        /// </summary>
        public MainWindow()
        {
            InitializeComponent();
            DataContext = new MVUserViewModel();
            txtUsername.Focus();
        }

        /// <summary>
        /// Close function
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnClose_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        /// <summary>
        /// Login Event
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnLogin_Click(object sender, RoutedEventArgs e)
        {
            //var authenticate = new UserManager();

            //try
            //{
            //    _user = authenticate.AuthenticateUser(txtUsername.Text, txtPassword.Password);
            //    this.Hide();
            //    var frmCentralDashboard = new frmCentralDashboard(_user);
            //    frmCentralDashboard.ShowDialog();
            //}
            //catch (Exception ex)
            //{
            //    Console.WriteLine(ex);
            //    lblPrompt.Content = failure;
            //    txtUsername.BorderBrush = Brushes.Red;
            //    txtPassword.BorderBrush = Brushes.Red;
            //    txtUsername.Focus();
            //}
 
        }

        /// <summary>
        /// Closes window
        /// </summary>
        public void closeWindow()
        {
            this.Close();
        }

        private void txtUsername_TextChanged(object sender, TextChangedEventArgs e)
        {
            txtUsername.BorderBrush = Brushes.Blue;
            lblPrompt.Content = "";

        }

        private void txtPassword_PasswordChanged(object sender, RoutedEventArgs e)
        {
            if (this.DataContext != null)
            {
                ((dynamic) this.DataContext).SecurePassword = ((PasswordBox) sender).SecurePassword;
            }
            //txtPassword.BorderBrush = Brushes.Blue;
            //lblPrompt.Content = "";
        }


    }
}
