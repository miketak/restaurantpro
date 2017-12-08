using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Security;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using BusinessLogic;
using DataObjects;
using RestaurantPro.Commands;
using RestaurantPro.Models;
using RestaurantPro.Views;

namespace RestaurantPro.ViewModels
{
    /// <summary>
    /// Initializes a new instance of the MVUserViewModel
    /// </summary>
    internal class MVUserViewModel
    {
        public MVUserViewModel()
        {
            _MVUser = new MVUser();//"rkpadi", "Richard", "Padi", "password");
            LoginCommand = new MVUserLoginCommand(this);
        }

        private MVUser _MVUser;

        public SecureString SecurePassword { private get; set; }

        /// <summary>
        /// Gets the MVUser instance
        /// </summary>
        public MVUser MVUser
        {
            get { return _MVUser; }
        }

        public ICommand LoginCommand
        {
            get;
            private set;
        }

        /// <summary>
        /// Gets or sets a System.Boolean value indicating wether the customer can be updated.
        /// </summary>
        public bool CanLogin
        {
            get
            {
                if (MVUser == null)
                {
                    return false;
                }
                return !String.IsNullOrWhiteSpace(MVUser.Username) && SecurePassword != null;
            }
        }

        /// <summary>
        /// Saves changes made to the MVUser instance
        /// </summary>
        public void SaveChanges()
        {
            var authenticate = new UserManager();

            try
            {
                var _user = authenticate.AuthenticateUser(_MVUser.Username, SecurePassword);

                //this is an evil evil violation of MVVM Mike!. Mike:Yes I know!.
                var frmCentralDashboard = new frmCentralDashboard(_user);
                frmCentralDashboard.ShowDialog();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
                // must close window someway.


                //lblPrompt.Content = failure;
                //txtUsername.BorderBrush = Brushes.Red;
                //txtPassword.BorderBrush = Brushes.Red;
                //txtUsername.Focus();
            }
            Debug.Assert(false, String.Format("{0} was updated.", _MVUser.Username));
        }
    }
}
