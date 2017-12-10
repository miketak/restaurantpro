using System;
using System.Windows;
using DataObjects;
using MahApps.Metro.Controls.Dialogs;

namespace RestaurantPro.Views
{
    /// <summary>
    /// Interaction logic for frmCentralDashboard.xaml
    /// </summary>
    public partial class frmCentralDashboard 
    {
        /// <summary>
        /// Current User
        /// </summary>
        User _user;

        /// <summary>
        /// Central Dashboard Constructor
        /// </summary>
        /// <param name="user"></param>
        public frmCentralDashboard()
        {
            //_user = user;
            InitializeComponent();
            SetupWindow();
        }

        private void SetupWindow()
        {
            //Set first and last name
            //txtName.Text = _user.FirstName + " " + _user.LastName;

            //Set status message
            //txtStatusMessage.Content += "Welcome back " + _user.FirstName + " " + _user.LastName;           
        }

        private async void tryMe_Click(object sender, RoutedEventArgs e)
        {
             var mySettings = new MetroDialogSettings()
            {
                AffirmativeButtonText = "OK",
                ColorScheme = MetroDialogColorScheme.Theme
            };

            await this.ShowMessageAsync("Hello", "This is information",
                MessageDialogStyle.Affirmative, mySettings);
        }

        /// <summary>
        /// Toggles fly out open and closed.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ToggleButton_Checked(object sender, RoutedEventArgs e)
        {
            mySettings.IsOpen = true;
        }

        /// <summary>
        /// Toggles fly out open and closed.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ToggleButton_Unchecked(object sender, RoutedEventArgs e)
        {
            mySettings.IsOpen = false;
        }

        /// <summary>
        /// Opens Admin Central App
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnAdminCentral(object sender, RoutedEventArgs e)
        {
            //this.Hide();
            var frmAdminCentral = new frmAdminCentral();
            frmAdminCentral.Show();
        }

        /// <summary>
        /// Update Password function
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnChangePassword(object sender, RoutedEventArgs e)
        {
            
            var mySettings = new MetroDialogSettings()
            {
                ColorScheme = MetroDialogColorScheme.Theme
            };

            var subfrmChangePassword = new subfrmChangePassword(_user);
            subfrmChangePassword.ShowDialog();

            
        }

        private void MetroWindow_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            Environment.Exit(0);
        }



 
    }
}
