using System;
using System.Diagnostics;
using System.Security;
using System.Windows.Input;
using BusinessLogic;
using PresentationLayer.ViewModels;
using RestaurantPro.Commands;
using RestaurantPro.Models;

namespace RestaurantPro.ViewModels
{
    /// <summary>
    /// Initializes a new instance of the MVUserViewModel
    /// </summary>
    internal class MVUserViewModel
    {

        private MVUser mVUser;
        private CentralDashboardViewModel childViewModel_CentralDashboard;
        public SecureString SecurePassword { private get; set; }
        
        /// <summary>
        /// Constructor to initialize MVUser and MVUserLoginCommand
        /// </summary>
        public MVUserViewModel()
        {
            mVUser = new MVUser();
            childViewModel_CentralDashboard = new CentralDashboardViewModel();
            LoginCommand = new MVUserLoginCommand(this);
        }

        /// <summary>
        /// Gets the MVUser instance
        /// </summary>
        public MVUser MVUser
        {
            get { return mVUser; }
        }

        /// <summary>
        /// Gets or sets a System.Boolean value indicating wether the customer can be updated.
        /// </summary>
        //public bool CanLogin
        //{
        //    get
        //    {
        //        if (MVUser == null)
        //        {
        //            return false;
        //        }
        //        return !String.IsNullOrWhiteSpace(MVUser.Username) && SecurePassword != null;
        //    }
        //}

        public ICommand LoginCommand
        {
            get;
            private set;
        }

        /// <summary>
        /// Saves changes made to the MVUser instance
        /// </summary>
        public void SaveChanges() 
        {
            var authenticate = new UserManager();

            try
            {
                var _user = authenticate.AuthenticateUser(mVUser.Username, SecurePassword);
                var view = new frmCentralDashboard();

                //this is an evil evil violation of MVVM Mike!. Mike:Yes I know!.
                view.DataContext = childViewModel_CentralDashboard;
                childViewModel_CentralDashboard.FirstName = _user.FirstName;
                childViewModel_CentralDashboard.LastName = _user.LastName;
                view.ShowDialog();
               
                
                
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);;
            }
            Debug.Assert(false, String.Format("{0} was updated.", mVUser.Username));
        }
    }
}
