using System;
using System.Security;
using System.Windows;
using RestaurantPro.Core.Services;
using RestaurantPro.Infrastructure.Services;
using RestaurantPro.Models;

namespace RestaurantPro.Login
{
    /// <summary>
    /// View model for Login View
    /// </summary>
    public class LoginViewModel : BindableBase
    {

        private IUserAuthenticationService _userAuthenticationService;

        public SecureString SecurePassword { private get; set; }

        /// <summary>
        /// Login View Model Constructor
        /// </summary>
        public LoginViewModel()
        {
            _userAuthenticationService = new UserAuthenticationService();
            LoginCommand = new RelayCommand(OnLogin, CanLogin);
        }

        #region Object Binding
        private MVUser _CurrentUser;
        /// <summary>
        /// Current User
        /// </summary>
        public MVUser CurrentUser
        {
            get { return _CurrentUser; }
            set { SetProperty(ref _CurrentUser, value); }
        }
        #endregion
        

        #region Commands

        public RelayCommand LoginCommand { get; private set; }

        #endregion

        #region Events
        /// <summary>
        /// Action Event to navigate to Home Dashboard
        /// </summary>
        public event Action<MVUser> LoginRequested = delegate { };
        #endregion

        #region Command Implementations
        private void OnLogin()
        {
            try
            {
                var _user = _userAuthenticationService.AuthenticateUser(CurrentUser.Username, SecurePassword);
            }
            catch (Exception e)
            {
                MessageBox.Show("Check username and password");
                return;
            }

            LoginRequested(CurrentUser);
        }

        private bool CanLogin()
        {
            return !CurrentUser.HasErrors;
        }

        #endregion

        }


    
}
