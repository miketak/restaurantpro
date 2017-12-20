using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using RestaurantPro.Core;
using RestaurantPro.Core.Services;
using RestaurantPro.HomeDashboard;
using RestaurantPro.InventoryFeatures;
using RestaurantPro.InventoryFeatures.WorkCycles;
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
        private InventoryDashboardViewModel _inventoryDashboardViewModel;
        private WorkCycleListViewModel _workCycleListViewModel;
        private AddEditWorkingCycleViewModel _addEditWorkingCycleViewModel;


        /// <summary>
        /// Constructor to subscription of events for overall program navigation.
        /// </summary>
        public MainWindowViewModel(IUserAuthenticationService userAuthenticationService, IUnitOfWork unitOfWork)
        {
            NavCommand = new RelayCommand<string>(OnNav);

            //View Model Initializations
            _loginViewModel = new LoginViewModel(userAuthenticationService);
            _homeDashboardViewModel = new HomeDashboardViewModel();
            _inventoryDashboardViewModel = new InventoryDashboardViewModel();
            _workCycleListViewModel = new WorkCycleListViewModel(unitOfWork);
            _addEditWorkingCycleViewModel = new AddEditWorkingCycleViewModel(unitOfWork);

            //Set Login context
            SetLoginContext();

            //Event Subscriptions
            _loginViewModel.LoginRequested += NavToHomeDashboard;

            _homeDashboardViewModel.LogoutRequested += NavToLoginView;
            _homeDashboardViewModel.InventoryDashboardRequested += NavigateToInventoryDashboard;
            _inventoryDashboardViewModel.HomeDashboardRequested += NavToHomeDashboard;

            _inventoryDashboardViewModel.LogoutRequested += NavToLoginView;
            _inventoryDashboardViewModel.ManageWorkCyclesRequsted += NavToManageWorkCycles;

            _workCycleListViewModel.InventoryDashboardRequested += NavigateToInventoryDashboard;
            _workCycleListViewModel.LogoutRequested += NavToLoginView;
            _workCycleListViewModel.AddWorkCycleRequested += NavToAddWorkCycleView;

            _addEditWorkingCycleViewModel.LogoutRequested += NavToLoginView;
            _addEditWorkingCycleViewModel.ManageWorkCyclesRequsted += NavToManageWorkCycles;
            

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

        private void NavigateToInventoryDashboard(WpfUser currentUser)
        {
            _inventoryDashboardViewModel.SetCurrentUserAndInitializeCommands(currentUser);
            CurrentViewModel = _inventoryDashboardViewModel;
        }

        private void NavToManageWorkCycles(WpfUser currentUser)
        {
            _workCycleListViewModel.SetCurrentUserAndInitializeCommands(currentUser);
            CurrentViewModel = _workCycleListViewModel;
        }

        public void NavToAddWorkCycleView(WpfWorkCycle workCycle, WpfUser currentUser)
        {
            _addEditWorkingCycleViewModel.SetCurrentUserAndInitializeCommands(currentUser);
            CurrentViewModel = _addEditWorkingCycleViewModel;
        }



        #endregion

    }
}
