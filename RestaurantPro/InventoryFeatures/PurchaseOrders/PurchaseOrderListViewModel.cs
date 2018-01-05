using System;
using System.Collections.ObjectModel;
using System.Linq;
using MahApps.Metro.Controls.Dialogs;
using RestaurantPro.Core;
using RestaurantPro.Models;

namespace RestaurantPro.InventoryFeatures.PurchaseOrders
{
    /// <inheritdoc />
    /// <summary>
    /// Purchase Order View Model Class
    /// </summary>
    public class PurchaseOrderListViewModel : BindableBase
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IDialogCoordinator dialogCoordinator;

        public PurchaseOrderListViewModel(IUnitOfWork unitOfWork, IDialogCoordinator instance)
        {
            _unitOfWork = unitOfWork;
            dialogCoordinator = instance;

            BackToInventoryCommand = new RelayCommand(OnBackToInventoryInventoryClick);
            LogoutCommand = new RelayCommand(OnLogout);
            BackHomeCommand = new RelayCommand(OnHomeClick);
            AddPurchaseOrderCommand = new RelayCommand(OnAddPurchaseOrderClick);
            EditPurchaseOrderCommand = new RelayCommand<WpfPurchaseOrder>(OnEditPurchaseOrderClick);
        }

        #region Initialization methods

        public void SetCurrentUser(WpfUser user)
        {
            CurrentUser = user;
        }

        public void LoadPurchaseOrders()
        {
            var purchaseOrders = _unitOfWork.PurchaseOrders
                .GetAll()
                .Where(u => u.Active).ToList();

            var wpfPurchaseOrders = RestproMapper.MapPurchaseOrderListToWpfPurchaseOrderList(purchaseOrders);

            PurchaseOrders = new ObservableCollection<WpfPurchaseOrder>(wpfPurchaseOrders);
        }

        #endregion

        #region Object Bindings

        private WpfUser _CurrentUser;
        public WpfUser CurrentUser
        {
            get { return _CurrentUser; }
            set { SetProperty(ref _CurrentUser, value); }
        }

        private ObservableCollection<WpfPurchaseOrder> _purchaseOrders;

        public ObservableCollection<WpfPurchaseOrder> PurchaseOrders
        {
            get { return _purchaseOrders; }
            set { SetProperty(ref _purchaseOrders, value);}
        }
        #endregion

        #region Events

        public event Action<WpfUser> LogoutRequested = delegate { };

        public event Action<WpfUser> InventoryDashboardRequested = delegate { };

        public event Action<WpfUser> HomeViewRequested = delegate { };

        public event Action<WpfPurchaseOrder, WpfUser> AddPurchaseOrderRequested = delegate { };

        public event Action<WpfPurchaseOrder, WpfUser> EditPurchaseOrderRequested = delegate { };

        #endregion

        #region Commands

        public RelayCommand LogoutCommand { get; private set; }

        public RelayCommand BackToInventoryCommand { get; private set; }

        public RelayCommand BackHomeCommand { get; private set; }

        public RelayCommand<WpfPurchaseOrder> EditPurchaseOrderCommand { get; private set; }

        public RelayCommand AddPurchaseOrderCommand { get; private set; }

        #endregion

        #region Event Handling Implementations

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

        private void OnEditPurchaseOrderClick(WpfPurchaseOrder wpfPurchaseOrder)
        {
            EditPurchaseOrderRequested(wpfPurchaseOrder, CurrentUser);
        }

        private void OnAddPurchaseOrderClick()
        {
            AddPurchaseOrderRequested(new WpfPurchaseOrder(), CurrentUser);
        }

        #endregion

    }


}