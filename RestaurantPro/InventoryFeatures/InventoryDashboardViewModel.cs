using System;
using RestaurantPro.Models;

namespace RestaurantPro.InventoryFeatures
{
    /// <summary>
    /// Inventory Dashboard View View Model
    /// </summary>
    public class InventoryDashboardViewModel : BindableBase
    {
        /// <summary>
        /// Initializes events
        /// </summary>
        public InventoryDashboardViewModel()
        {
            LogoutCommand = new RelayCommand(OnLogout);
            BackHomeCommand = new RelayCommand(OnHomeClick);
            ManageWorkCyclesCommand = new RelayCommand(OnManageCyclesClick);
            PurchaseOrdersListViewCommand = new RelayCommand(OnPurchaseOrdersListViewClick);
            SupplierListViewCommand = new RelayCommand(OnSupplierListViewClick);
            RawMaterialListViewCommand = new RelayCommand(OnRawMaterialListViewClick);
            RawMaterialCategoryListViewCommand = new RelayCommand(OnRawMaterialCategoryListViewClick);
            LocationListViewCommand = new RelayCommand(OnLocationListViewClick);
            InventorySettingViewCommand = new RelayCommand(OnInventorySettingViewClick);
            
        }

        #region Initialization Methods

        /// <summary>
        /// Sets Current User
        /// </summary>
        /// <param name="user">Current User</param>
        public void SetCurrentUser(WpfUser user)
        {
            CurrentUser = user;
        }


        #endregion

        #region Bindable Objects

        private WpfUser _CurrentUser;

        public WpfUser CurrentUser
        {
            get { return _CurrentUser; }
            set { SetProperty(ref _CurrentUser, value); }
        }

        #endregion

        #region Events

        public event Action<WpfUser> LogoutRequested = delegate { };

        public event Action<WpfUser> HomeDashboardRequested = delegate { };

        public event Action<WpfUser> ManageWorkCyclesRequsted = delegate { };

        public event Action<WpfUser> PurchaseOrdersListsViewRequested = delegate { };

        public event Action<WpfUser> SupplierListViewRequested = delegate { };

        public event Action<WpfUser> RawMaterialListViewRequested = delegate { };

        public event Action<WpfUser> RawMaterialCategoryListViewRequested = delegate { };

        public event Action<WpfUser> LocationListViewRequested = delegate { };

        public event Action<WpfUser> InventorySettingViewRequested = delegate { };

        #endregion

        #region Commands

        public RelayCommand LogoutCommand { get; private set; }

        public RelayCommand BackHomeCommand { get; private set; }

        public RelayCommand ManageWorkCyclesCommand { get; private set; }

        public RelayCommand PurchaseOrdersListViewCommand { get; private set; }

        public RelayCommand SupplierListViewCommand { get; private set; }

        public RelayCommand RawMaterialListViewCommand { get; private set; }

        public RelayCommand RawMaterialCategoryListViewCommand { get; private set; }

        public RelayCommand LocationListViewCommand { get; private set; }

        public RelayCommand InventorySettingViewCommand { get; private set; }


        #endregion

        #region Event Handling Implementations

        private void OnLogout()
        {
            CurrentUser = null;
            LogoutRequested(new WpfUser());
        }

        private void OnHomeClick()
        {
            HomeDashboardRequested(CurrentUser);
        }

        private void OnManageCyclesClick()
        {
            ManageWorkCyclesRequsted(CurrentUser);
        }

        private void OnPurchaseOrdersListViewClick()
        {
            PurchaseOrdersListsViewRequested(CurrentUser);
        }

        private void OnSupplierListViewClick()
        {
            SupplierListViewRequested(CurrentUser);
        }

        private void OnRawMaterialListViewClick()
        {
            RawMaterialListViewRequested(CurrentUser);
        }

        private void OnRawMaterialCategoryListViewClick()
        {
            RawMaterialCategoryListViewRequested(CurrentUser);
        }

        private void OnLocationListViewClick()
        {
            LocationListViewRequested(CurrentUser);
        }

        private void OnInventorySettingViewClick()
        {
            InventorySettingViewRequested(CurrentUser);
        }

    #endregion
    }
}