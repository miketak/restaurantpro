using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Windows;
using MahApps.Metro.Controls.Dialogs;
using RestaurantPro.Core;
using RestaurantPro.Core.Domain;
using RestaurantPro.Infrastructure;
using RestaurantPro.Models;

namespace RestaurantPro.InventoryFeatures
{
    public class ProcurePurchaseOrderViewModel : BindableBase
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IDialogCoordinator dialogCoordinator;

        public ProcurePurchaseOrderViewModel(IUnitOfWork unitOfWork, IDialogCoordinator instance)
        {
            _unitOfWork = unitOfWork;
            dialogCoordinator = instance;
            SetLocations();
            SetFilterCategories();
            SetDateReceived();
            LoadPendingItems();

            BackToInventoryCommand = new RelayCommand(OnBackToInventoryInventoryClick);
            LogoutCommand = new RelayCommand(OnLogout);
            BackHomeCommand = new RelayCommand(OnHomeClick);
            ProcureCommand = new RelayCommand<ProcurementItem>(OnProcureClick);
        }

        #region Object Bindings

        private WpfUser _currentUser;
        public WpfUser CurrentUser
        {
            get { return _currentUser; }
            set { SetProperty(ref _currentUser, value); }
        }

        private List<string> _locations;
        public List<string> Locations
        {
            get { return _locations; }
            set { _locations = value; }
        }

        private List<string> _filterCategories;
        public List<string> FilterCategories
        {
            get { return _filterCategories; }
            set { _filterCategories = value; }
        }

        private DateTime _dateReceived;
        public DateTime DateReceived
        {
            get { return _dateReceived; }
            set { SetProperty( ref _dateReceived, value); }
        }

        private BindingList<ProcurementItem> _pendingItems;
        public BindingList<ProcurementItem> PendingItems
        {
            get { return _pendingItems; }
            set { SetProperty(ref _pendingItems, value); }
        }

        private BindingList<ProcurementItem> _procurementSummary;
        public BindingList<ProcurementItem> ProcurementSummary
        {
            get { return _procurementSummary; }
            set { SetProperty(ref _procurementSummary, value); }
        }

        #endregion

        #region Initialization Methods

        public void SetCurrentUser(WpfUser user)
        {
            CurrentUser = user;
        }

        private void SetLocations()
        {
            var locationsInDb = _unitOfWork.Locations.GetAll().ToList();
            var locs = locationsInDb.Select(a => a.LocationId).ToList();
            Locations = locs;
        }

        private void SetFilterCategories()
        {
            var filterCategories = new List<string>
            {
                "Work Cycle",
                "Purchase Order",
                "Supplier"
            };
            FilterCategories = filterCategories;
        }

        private void SetDateReceived()
        {
            DateReceived = DateTime.Now;
        }

        private void LoadPendingItems()
        {
            var pds = new BindingList<ProcurementItem>
            {
                new ProcurementItem
                {
                    RawMaterialId = 1,
                    SupplierId = 1,
                    PurchaseOrderId = 1,
                    WorkCycleId = 1,
                    OrderedQuantity = 20,
                    PendingQuantity = 20,
                    TotalValue = 100,

                },                
                new ProcurementItem
                {
                    RawMaterialId = 2,
                    SupplierId = 2,
                    PurchaseOrderId = 1,
                    WorkCycleId = 1,
                    OrderedQuantity = 25,
                    PendingQuantity = 25,
                    TotalValue = 50

                }
            };
            PendingItems = pds;
        }
        #endregion

        #region Events

        public event Action<WpfUser> LogoutRequested = delegate { };

        public event Action<WpfUser> InventoryDashboardRequested = delegate { };

        public event Action<WpfUser> HomeViewRequested = delegate { };

        #endregion

        #region Commands

        
        public RelayCommand LogoutCommand { get; private set; }

        public RelayCommand BackToInventoryCommand { get; private set; }

        public RelayCommand BackHomeCommand { get; private set; }

        public RelayCommand<ProcurementItem> ProcureCommand { get; private set; }

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

        private void OnProcureClick(ProcurementItem procurementItemFromView)
        {
            if (ProcurementSummary == null)
                ProcurementSummary = new BindingList<ProcurementItem>();

            var isExist = false;
            foreach (var a in ProcurementSummary)
            {
                if (isProcurementItemandPendingItemSame(procurementItemFromView, a))
                {
                    a.ReceivedQuantity += procurementItemFromView.ReceivedQuantity;
                    isExist = true;
                }
            }

            if (!isExist)
            {
                ProcurementSummary.Add(new ProcurementItem
                {
                    RawMaterialId = procurementItemFromView.RawMaterialId,
                    SupplierId = procurementItemFromView.SupplierId,
                    WorkCycleId = procurementItemFromView.WorkCycleId,
                    OrderedQuantity = procurementItemFromView.OrderedQuantity,
                    ReceivedQuantity = procurementItemFromView.ReceivedQuantity,
                    PurchaseOrderId = procurementItemFromView.PurchaseOrderId,
                    DateReceived = procurementItemFromView.DateReceived
                });

                foreach (var a in ProcurementSummary)
                    a.ReceivedQuantityChanged += AdjustPendingItemsPendingQuantity;
            }

            foreach (var a in PendingItems)
            {
                if (!isProcurementItemandPendingItemSame(a, procurementItemFromView)) continue;
                a.ReceivedQuantityAdjustment = a.ReceivedQuantity;
                a.ReceivedQuantity = 0;
            }
        }     
        
        private void AdjustPendingItemsPendingQuantity(ProcurementItem summaryItem)
        {
                foreach (var b in PendingItems)
                {
                    if (!isProcurementItemandPendingItemSame(summaryItem, b)) continue;
                    b.ReceivedQuantityAdjustment = summaryItem.ReceivedQuantity;
                    return;
                }
        }

        private static bool isProcurementItemandPendingItemSame(ProcurementItem a, ProcurementItem b)
        {
            return a.RawMaterialId == b.RawMaterialId && a.SupplierId == b.SupplierId && a.WorkCycleId == b.WorkCycleId && a.PurchaseOrderId == b.PurchaseOrderId;
        }

        #endregion

    }
}