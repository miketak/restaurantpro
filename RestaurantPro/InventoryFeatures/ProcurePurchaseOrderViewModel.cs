using System;
using System.Collections.Generic;
using System.Linq;
using MahApps.Metro.Controls.Dialogs;
using RestaurantPro.Core;
using RestaurantPro.Core.Domain;
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

            BackToInventoryCommand = new RelayCommand(OnBackToInventoryInventoryClick);
            LogoutCommand = new RelayCommand(OnLogout);
            BackHomeCommand = new RelayCommand(OnHomeClick);

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
            set { _dateReceived = value; }
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

        public RelayCommand SaveCommand { get; private set; }

        public RelayCommand<WpfRawMaterialCategory> DeleteCommand { get; private set; }

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

        #endregion

    }
}