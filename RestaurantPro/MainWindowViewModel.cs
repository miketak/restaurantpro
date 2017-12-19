using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using RestaurantPro.Core.Services;
using RestaurantPro.HomeDashboard;
using RestaurantPro.Login;
using RestaurantPro.Models;

namespace RestaurantPro
{
    /// <summary>
    /// View Model for Generic Window Container
    /// </summary>
    public class MainWindowViewModel : BindableBase
    {
        private BindableBase _CurrentViewModel;

        private LoginViewModel _loginViewModel;
        private HomeDashboardViewModel _homeDashboardViewModel;


        /// <summary>
        /// Constructor to subscription of events for overall program navigation.
        /// </summary>
        public MainWindowViewModel(IUserAuthenticationService userAuthenticationService)
        {
            NavCommand = new RelayCommand<string>(OnNav);

            //DependencyResolution
            _loginViewModel = new LoginViewModel(userAuthenticationService);
            _homeDashboardViewModel = new HomeDashboardViewModel();


            //Set Login context
            SetLoginContext();

            //Event Subscriptions
            _loginViewModel.LoginRequested += NavToHomeDashboard;
            _homeDashboardViewModel.LogoutRequested += NavToLoginView;
        }

        /// <summary>
        /// Sets to Login Context on Start Up
        /// </summary>
        public void SetLoginContext()
        {
            _loginViewModel.CurrentUser = new WpfUser();
            CurrentViewModel = _loginViewModel;
        }

        /// <summary>
        /// Generic View Model Set By Current View Context
        /// </summary>
        public BindableBase CurrentViewModel
        {
            get { return _CurrentViewModel; }
            set { SetProperty(ref _CurrentViewModel, value);}
        }

        private void OnNav(string destination)
        {
            switch (destination)
            {
                case "home":
                    CurrentViewModel = _homeDashboardViewModel;
                    break;
                case "login":
                    CurrentViewModel = _loginViewModel;
                    break;
                default:
                    CurrentViewModel = _homeDashboardViewModel;
                    break;
            }
        }

        /// <summary>
        /// Command Relay
        /// </summary>
        public RelayCommand<string> NavCommand { get; private set; }

        #region Navigation Event Implementations

        private void NavToHomeDashboard(WpfUser currentUser)
        {
            _homeDashboardViewModel.SetCurrentUser(currentUser);
            CurrentViewModel = _homeDashboardViewModel;
        }

        private void NavToLoginView(WpfUser currentUser)
        {
            _loginViewModel.CurrentUser = currentUser;
            CurrentViewModel = _loginViewModel;
        }

        #endregion

    }
}
