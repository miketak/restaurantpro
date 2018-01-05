using System;
using MahApps.Metro.Controls.Dialogs;
using RestaurantPro.Core;
using RestaurantPro.Models;

namespace RestaurantPro.InventoryFeatures.PurchaseOrders
{
    /// <summary>
    /// View Model for Add/Edit Purchase Order View
    /// </summary>
    public class AddEditPurchaseOrderViewModel : BindableBase
    {
        private bool _editMode;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IDialogCoordinator dialogCoordinator;

        public AddEditPurchaseOrderViewModel(IUnitOfWork unitOfWork, IDialogCoordinator instance)
        {
            _unitOfWork = unitOfWork;
            dialogCoordinator = instance;

            BackToInventoryCommand = new RelayCommand(OnBackToInventoryInventoryClick);
            LogoutCommand = new RelayCommand(OnLogout);
            BackHomeCommand = new RelayCommand(OnHomeClick);
            BackToPurchaseOrderListCommand = new RelayCommand(OnPurchaseOrderListClick);
        }

        #region Initialization methods

        public void SetCurrentUser(WpfUser user)
        {
            CurrentUser = user;
        }

        #endregion

        #region Object Bindings

        private WpfUser _CurrentUser;
        public WpfUser CurrentUser
        {
            get { return _CurrentUser; }
            set { SetProperty(ref _CurrentUser, value); }
        }

        public bool EditMode
        {
            get { return _editMode; }
            set { SetProperty(ref _editMode, value); }
        }

        #endregion

        #region Events

        public event Action<WpfUser> LogoutRequested = delegate { };

        public event Action<WpfUser> InventoryDashboardRequested = delegate { };

        public event Action<WpfUser> HomeViewRequested = delegate { };

        public event Action<WpfUser> PurchaseOrderListRequested = delegate { };
        
        #endregion

        #region Commands

        public RelayCommand LogoutCommand { get; private set; }

        public RelayCommand BackToInventoryCommand { get; private set; }

        public RelayCommand BackHomeCommand { get; private set; }

        public RelayCommand BackToPurchaseOrderListCommand { get; private set; }

        #endregion

        #region Event Handlings

        private void OnBackToInventoryInventoryClick()
        {
            InventoryDashboardRequested(CurrentUser);
        }

        private void OnLogout()
        {
            CurrentUser = null;
            LogoutRequested(new WpfUser());
        }

        private void OnHomeClick()
        {
            HomeViewRequested(CurrentUser);
        }

        private void OnPurchaseOrderListClick()
        {
            PurchaseOrderListRequested(CurrentUser);
        }

        #endregion

        public void SetPurchaseOrder(WpfPurchaseOrder wpfPurchaseOrder)
        {
            throw new NotImplementedException();
        }
    }
}