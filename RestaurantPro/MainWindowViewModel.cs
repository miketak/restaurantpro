using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
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

        private LoginViewModel _loginViewModel = new LoginViewModel();
        private HomeDashboardViewModel _homeDashboardViewModel = new HomeDashboardViewModel();

        /// <summary>
        /// Constructor to subscription of events for overall program navigation.
        /// </summary>
        public MainWindowViewModel()
        {
            NavCommand = new RelayCommand<string>(OnNav);

            //Event Subscriptions
            _loginViewModel.LoginRequested += NavToHomeDashboard;
        }

        /// <summary>
        /// Sets to Login Context on Start Up
        /// </summary>
        public void SetLoginContext()
        {
            _loginViewModel.CurrentUser = new MVUser();
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

        #region Navigation Events Fire Methods

        private void NavToHomeDashboard(MVUser currentUser)
        {
            _homeDashboardViewModel.SetCurrentUser(currentUser);
            CurrentViewModel = _homeDashboardViewModel;
        }

        #endregion

    }
}
