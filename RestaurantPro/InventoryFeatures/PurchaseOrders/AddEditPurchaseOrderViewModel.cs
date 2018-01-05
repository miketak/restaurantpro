using System;
using System.ComponentModel;
using System.Linq;
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
            SaveCommand = new RelayCommand(OnSave, CanSave);
        }

        #region Initialization methods

        public void SetCurrentUser(WpfUser user)
        {
            CurrentUser = user;
        }

        public void SetPurchaseOrder(WpfPurchaseOrder wpfPurchaseOrder)
        {
            PurchaseOrder = wpfPurchaseOrder;

            if (EditMode)
            {
                var purchaseOrderWithLines = _unitOfWork
                    .PurchaseOrders
                    .GetPurchaseOrderByPurchaseOrderNumber(wpfPurchaseOrder.PurchaseOrderNumber, true);

                if (purchaseOrderWithLines == null)
                    throw new ApplicationException("Invalid Purchase Order");

                PurchaseOrder.Lines = new BindingList<WpfPurchaseOrderLine>(
                    RestproMapper.MapPurchaseOrderLineListToWpfPurchaseOrderLineList(purchaseOrderWithLines.PurchaseOrderLines.ToList()));
            }
            else
                PurchaseOrder.Lines = new BindingList<WpfPurchaseOrderLine>();

            if (PurchaseOrder != null)
                PurchaseOrder.ErrorsChanged -= RaiseCanExecuteChanged;

            PurchaseOrder.ErrorsChanged += RaiseCanExecuteChanged;
            this.PurchaseOrder.Lines.ListChanged += this.OnListChanged;

        }

        private void RaiseCanExecuteChanged(object sender, DataErrorsChangedEventArgs e)
        {
            SaveCommand.RaiseCanExecuteChanged();
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

        private WpfPurchaseOrder _purchaseOrder;

        public WpfPurchaseOrder PurchaseOrder
        {
            get { return _purchaseOrder; }
            set
            {
                if (PurchaseOrder != null)
                    PurchaseOrder.UpdateSubTotalSection();
                SetProperty(ref _purchaseOrder, value);
            }
        }

        #endregion

        #region Events

        public event Action<WpfUser> LogoutRequested = delegate { };

        public event Action<WpfUser> InventoryDashboardRequested = delegate { };

        public event Action<WpfUser> HomeViewRequested = delegate { };

        public event Action<WpfUser> PurchaseOrderListRequested = delegate { };

        public event Action<WpfUser> Done = delegate { };

        public event ListChangedEventHandler ListChanged;
        
        #endregion

        #region Commands

        public RelayCommand LogoutCommand { get; private set; }

        public RelayCommand BackToInventoryCommand { get; private set; }

        public RelayCommand BackHomeCommand { get; private set; }

        public RelayCommand BackToPurchaseOrderListCommand { get; private set; }

        public RelayCommand SaveCommand { get; private set; }

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

        private async void OnSave()
        {
            var purchaseOrderEntity = RestproMapper.MapWpfPurchaseOrderToPurchaseOrderWithLines(PurchaseOrder);

            if (_editMode)
            {
                _unitOfWork.PurchaseOrders.UpdatePurchaseOrder(purchaseOrderEntity);
            }
            else
            {
                purchaseOrderEntity.Active = true;
                _unitOfWork.PurchaseOrders.AddPurchaseOrder(purchaseOrderEntity);
                _unitOfWork.Complete();
            }

            var controller = await dialogCoordinator.ShowMessageAsync(this, "Success", "Items Saved Successfully. You Rock!");

            if (controller == MessageDialogResult.Affirmative)
                Done(CurrentUser);
        }

        private bool CanSave()
        {
            return !PurchaseOrder.HasErrors;
        }

        private void OnListChanged(object sender, ListChangedEventArgs e)
        {
            PurchaseOrder.UpdateSubTotalSection();
        }

        #endregion


    }
}