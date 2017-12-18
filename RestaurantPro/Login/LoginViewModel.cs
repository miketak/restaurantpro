using System;
using System.Security;
using System.Windows;
using AutoMapper;
using RestaurantPro.Core.Domain;
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
        public LoginViewModel(IUserAuthenticationService userAuthenticationService)
        {
            _userAuthenticationService = userAuthenticationService;
            LoginCommand = new RelayCommand(OnLogin, CanLogin);
        }

        #region Object Binding
        private WpfUser _CurrentUser;
        /// <summary>
        /// Current User
        /// </summary>
        public WpfUser CurrentUser
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
        public event Action<WpfUser> LoginRequested = delegate { };
        #endregion

        #region Command Implementations
        private void OnLogin()
        {
            try
            {
                var user = _userAuthenticationService.AuthenticateUser(CurrentUser.Username, SecurePassword);
                var config = new MapperConfiguration(cfg =>
                {
                    cfg.CreateMap<User, WpfUser>();
                });
                IMapper iMapper = config.CreateMapper();
                CurrentUser = iMapper.Map<User, WpfUser>(user);
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
