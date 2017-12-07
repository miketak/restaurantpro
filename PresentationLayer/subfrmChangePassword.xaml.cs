using BusinessLogic;
using DataObjects;
using MahApps.Metro.Controls.Dialogs;
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

namespace PresentationLayer
{
    /// <summary>
    /// Interaction logic for subfrmChangePassword.xaml
    /// </summary>
    public partial class subfrmChangePassword
    {
        User _user = null;


        /// <summary>
        /// Retrieves and initializaes page with User information
        /// </summary>
        /// <param name="user"></param>
        public subfrmChangePassword(User user)
        {
            _user = user;
            InitializeComponent();
        }

        private void btnChangePassword(object sender, RoutedEventArgs e)
        {
            var oldPassword = txtOldPassword.Password;
            var newPassword = txtNewPassword.Password;
            var confirmPassword = txtConfirmPassword.Password;

            if (newPassword == oldPassword)  //make sure user is choosing a new password
            {
                this.customMessageBox("Alert!", "You need to choose a new password");
                txtNewPassword.Clear();
                txtConfirmPassword.Clear();
                txtNewPassword.Focus();
                return;
            }

            if (newPassword != confirmPassword) //make sure user knows what password was chosen
            {
                    this.customMessageBox("Alert", "New Password and Confirm must match");
                    txtNewPassword.Clear();
                    txtConfirmPassword.Clear();
                    txtNewPassword.Focus();
                    return;
            }

            try
            {
                var usrMgr = new UserManager();
                if (usrMgr.UpdatePassword(_user.UserId, oldPassword, newPassword))
                {
                    this.customMessageBox("Success!", "Password updated.");
                    //this.Close();
                }
                else // user supplied a bad oldPassword 
                {
                    throw new ApplicationException("Update Failed. Recheck password and try again.");
                }
            }
            catch (Exception ex)
            {
                this.customMessageBox("Failure", "Password not updated.\n" + ex.Message);
                txtNewPassword.Clear();
                txtOldPassword.Clear();
                txtConfirmPassword.Clear();
                txtOldPassword.Focus();
            }
        }

        private async void customMessageBox(String title, String message)
        {
            var mySettings = new MetroDialogSettings()
            {
                AffirmativeButtonText = "OK",
                ColorScheme = MetroDialogColorScheme.Theme
            };

            await this.ShowMessageAsync(title, message, MessageDialogStyle.Affirmative, mySettings);

            if (title.Equals("Success!"))
            {
                this.Close();
            }
        }
    }
}
