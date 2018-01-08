using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using MahApps.Metro.Controls.Dialogs;
using RestaurantPro.Core;
using RestaurantPro.Core.Services;
using RestaurantPro.HomeDashboard;
using RestaurantPro.InventoryFeatures;
using RestaurantPro.InventoryFeatures.PurchaseOrders;
using RestaurantPro.InventoryFeatures.RawMaterials;
using RestaurantPro.InventoryFeatures.Suppliers;
using RestaurantPro.InventoryFeatures.WorkCycles;
using RestaurantPro.Login;
using RestaurantPro.Models;

namespace RestaurantPro
{
    /// <inheritdoc />
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
        private PurchaseOrderListViewModel _purchaseOrderListViewModel;
        private AddEditPurchaseOrderViewModel _addEditPurchaseOrderListViewModel;
        private SupplierListViewModel _supplierListViewModel;
        private RawMaterialListViewModel _rawMaterialListViewModel;
        private RawMaterialCategoryListViewModel _rawMaterialCategoryListViewModel;

        /// <summary>
        /// Constructor to subscription of events for overall program navigation.
        /// </summary>
        public MainWindowViewModel(IUnitOfWork unitOfWork)
        {
            NavCommand = new RelayCommand<string>(OnNav);

            //View Model Initializations
            _loginViewModel = new LoginViewModel(unitOfWork, DialogCoordinator.Instance);
            _homeDashboardViewModel = new HomeDashboardViewModel();
            _inventoryDashboardViewModel = new InventoryDashboardViewModel();
            _workCycleListViewModel = new WorkCycleListViewModel(unitOfWork, DialogCoordinator.Instance);
            _addEditWorkingCycleViewModel = new AddEditWorkingCycleViewModel(unitOfWork, DialogCoordinator.Instance);
            _purchaseOrderListViewModel = new PurchaseOrderListViewModel(unitOfWork, DialogCoordinator.Instance);
            _addEditPurchaseOrderListViewModel = new AddEditPurchaseOrderViewModel(unitOfWork, DialogCoordinator.Instance);
            _supplierListViewModel = new SupplierListViewModel(unitOfWork, DialogCoordinator.Instance);
            _rawMaterialListViewModel = new RawMaterialListViewModel(unitOfWork, DialogCoordinator.Instance);
            _rawMaterialCategoryListViewModel = new RawMaterialCategoryListViewModel(unitOfWork, DialogCoordinator.Instance);

            //Set Login context
            SetLoginContext();

            //Event Subscriptions
            _loginViewModel.LoginRequested += NavToHomeDashboard;

            _homeDashboardViewModel.LogoutRequested += NavToLoginView;
            _homeDashboardViewModel.InventoryDashboardRequested += NavigateToInventoryDashboard;
            _inventoryDashboardViewModel.HomeDashboardRequested += NavToHomeDashboard;

            _inventoryDashboardViewModel.LogoutRequested += NavToLoginView;
            _inventoryDashboardViewModel.ManageWorkCyclesRequsted += NavToManageWorkCycles;
            _inventoryDashboardViewModel.PurchaseOrdersListsViewRequested += NavToPurchaseOrdersListView;
            _inventoryDashboardViewModel.SupplierListViewRequested += NavToSupplierListView;
            _inventoryDashboardViewModel.RawMaterialListViewRequested += NavToRawMaterialsListView;
            _inventoryDashboardViewModel.RawMaterialCategoryListViewRequested += NavToRawMaterialCategoryListView;

            _workCycleListViewModel.InventoryDashboardRequested += NavigateToInventoryDashboard;
            _workCycleListViewModel.LogoutRequested += NavToLoginView;
            _workCycleListViewModel.HomeViewRequested += NavToHomeDashboard;
            _workCycleListViewModel.AddWorkCycleRequested += NavToAddWorkCycleView;
            _workCycleListViewModel.EditWorkCycleRequested += NavToEditWorkCycleView;

            _addEditWorkingCycleViewModel.LogoutRequested += NavToLoginView;
            _addEditWorkingCycleViewModel.ManageWorkCyclesRequsted += NavToManageWorkCycles;
            _addEditWorkingCycleViewModel.Done += NavToManageWorkCycles;

            _purchaseOrderListViewModel.LogoutRequested += NavToLoginView;
            _purchaseOrderListViewModel.InventoryDashboardRequested += NavigateToInventoryDashboard;
            _purchaseOrderListViewModel.HomeViewRequested += NavToHomeDashboard;
            _purchaseOrderListViewModel.AddPurchaseOrderRequested += NavToAddPurchaseOrder;
            _purchaseOrderListViewModel.EditPurchaseOrderRequested += NavToEditPurchaseOrder;

            _addEditPurchaseOrderListViewModel.LogoutRequested += NavToLoginView;
            _addEditPurchaseOrderListViewModel.HomeViewRequested += NavToHomeDashboard;
            _addEditPurchaseOrderListViewModel.InventoryDashboardRequested += NavigateToInventoryDashboard;
            _addEditPurchaseOrderListViewModel.PurchaseOrderListRequested += NavToPurchaseOrdersListView;
            _addEditPurchaseOrderListViewModel.Done += NavToPurchaseOrdersListView;

            _supplierListViewModel.LogoutRequested += NavToLoginView;
            _supplierListViewModel.HomeViewRequested += NavToHomeDashboard;
            _supplierListViewModel.InventoryDashboardRequested += NavigateToInventoryDashboard;

            _rawMaterialListViewModel.LogoutRequested += NavToLoginView;
            _rawMaterialListViewModel.HomeViewRequested += NavToHomeDashboard;
            _rawMaterialListViewModel.InventoryDashboardRequested += NavigateToInventoryDashboard;

            _rawMaterialCategoryListViewModel.LogoutRequested += NavToLoginView;
            _rawMaterialCategoryListViewModel.HomeViewRequested += NavToHomeDashboard;
            _rawMaterialCategoryListViewModel.InventoryDashboardRequested += NavigateToInventoryDashboard;
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
            _inventoryDashboardViewModel.SetCurrentUser(currentUser);
            CurrentViewModel = _inventoryDashboardViewModel;
        }

        private void NavToManageWorkCycles(WpfUser currentUser)
        {
            _workCycleListViewModel.SetCurrentUser(currentUser);
            CurrentViewModel = _workCycleListViewModel;
        }

        public void NavToAddWorkCycleView(WpfWorkCycle workCycle, WpfUser currentUser)
        {
            _addEditWorkingCycleViewModel.EditMode = false;
            _addEditWorkingCycleViewModel.SetCurrentUser(currentUser);
            _addEditWorkingCycleViewModel.SetWorkingCycle(workCycle);
            CurrentViewModel = _addEditWorkingCycleViewModel;
        }

        private void NavToEditWorkCycleView(WpfWorkCycle workCycle, WpfUser currentUser)
        {
            _addEditWorkingCycleViewModel.EditMode = true;
            _addEditWorkingCycleViewModel.SetCurrentUser(currentUser);
            _addEditWorkingCycleViewModel.SetWorkingCycle(workCycle);
            CurrentViewModel = _addEditWorkingCycleViewModel;
        }

        private void NavToPurchaseOrdersListView(WpfUser currentUser)
        {
            _purchaseOrderListViewModel.SetCurrentUser(currentUser);
            CurrentViewModel = _purchaseOrderListViewModel;
        }

        private void NavToAddPurchaseOrder(WpfPurchaseOrder wpfPurchaseOrder, WpfUser currentUser)
        {
            _addEditPurchaseOrderListViewModel.EditMode = false;
            _addEditPurchaseOrderListViewModel.SetCurrentUser(currentUser);
            _addEditPurchaseOrderListViewModel.SetPurchaseOrder(wpfPurchaseOrder);
            CurrentViewModel = _addEditPurchaseOrderListViewModel;
        }

        private void NavToEditPurchaseOrder(WpfPurchaseOrder wpfPurchaseOrder, WpfUser currentUser)
        {
            _addEditPurchaseOrderListViewModel.EditMode = true;
            _addEditPurchaseOrderListViewModel.SetCurrentUser(currentUser);
            _addEditPurchaseOrderListViewModel.SetPurchaseOrder(wpfPurchaseOrder);
            CurrentViewModel = _addEditPurchaseOrderListViewModel;
        }

        private void NavToSupplierListView(WpfUser currentUser)
        {
            _supplierListViewModel.SetCurrentUser(currentUser);
            CurrentViewModel = _supplierListViewModel;
        }

        private void NavToRawMaterialsListView(WpfUser currentUser)
        {
            _rawMaterialListViewModel.SetCurrentUser(currentUser);
            CurrentViewModel = _rawMaterialListViewModel;
        }

        private void NavToRawMaterialCategoryListView(WpfUser currentUser)
        {
            _rawMaterialCategoryListViewModel.SetCurrentUser(currentUser);
            CurrentViewModel = _rawMaterialCategoryListViewModel;
        }

        #endregion

    }
}
