using System;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using MahApps.Metro.Controls.Dialogs;
using RestaurantPro.Core;
using RestaurantPro.Models;

namespace RestaurantPro.InventoryFeatures.Locations
{
    /// <summary>
    /// Location List View Model
    /// </summary>
    public class LocationListViewModel : BindableBase
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IDialogCoordinator dialogCoordinator;

        public LocationListViewModel(IUnitOfWork unitOfWork, IDialogCoordinator instance)
        {
            _unitOfWork = unitOfWork;
            dialogCoordinator = instance;

            BackToInventoryCommand = new RelayCommand(OnBackToInventoryInventoryClick);
            LogoutCommand = new RelayCommand(OnLogout);
            BackHomeCommand = new RelayCommand(OnHomeClick);
            SaveCommand = new RelayCommand(OnSave, CanSave);
            DeleteCommand = new RelayCommand<WpfLocation>(OnDeleteClick);
        }

        #region Object Bindings

        private WpfUser _currentUser;
        public WpfUser CurrentUser
        {
            get { return _currentUser; }
            set { SetProperty(ref _currentUser, value); }
        }

        private BindingList<WpfLocation> _locations;
        public BindingList<WpfLocation> Locations
        {
            get { return _locations; }
            set { SetProperty(ref _locations, value); }
        }

        #endregion

        #region Initialization Methods

        public void SetCurrentUser(WpfUser user)
        {
            CurrentUser = user;
        }

        public void LoadLocations()
        {
            var locationsInDb = _unitOfWork
                .Locations
                .GetLocations().ToList();

            var locationsForView = RestproMapper.MapLocationListToWpfLocationList(locationsInDb);

            Locations = new BindingList<WpfLocation>(locationsForView);
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

        public RelayCommand<WpfLocation> DeleteCommand { get; private set; }

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

        private async void OnSave()
        {
            var locationsToDb = RestproMapper
                .MapWpfLocationListToLocationList(Locations.ToList());
            string errorMessage = null;

            try
            {
                _unitOfWork.Locations.AddOrUpdate(locationsToDb);
                LoadLocations();
                await dialogCoordinator.ShowMessageAsync(this, "Success", "Items Saved Successfully. You Rock!");
            }
            catch (Exception e)
            {
                Debug.WriteLine(e.Message);
                errorMessage = e.Message;
            }

            if (errorMessage == null)
                return;

            await dialogCoordinator
                .ShowMessageAsync(this, "Error"
                    , "Fatal Error Occured. You're Screwed!\n" +
                      errorMessage);
        }

        private async void OnDeleteClick(WpfLocation location)
        {
            if (location == null)
                return;

            var locationToDb = RestproMapper.MapWpfLocationToLocation(location);
            string errorMessage = null;

            try
            {
                Locations.Remove(location);
                if (location.LocationId != null)
                    _unitOfWork.Locations.FakeDelete(locationToDb);
                await dialogCoordinator.ShowMessageAsync(this, "Success", "Raw Material Deleted Successfully. Good Bye :(");
            }
            catch (Exception e)
            {
                Debug.WriteLine(e);
                errorMessage = e.Message;
            }

            if (errorMessage == null)
                return;

            await dialogCoordinator
                .ShowMessageAsync(this, "Error"
                    , "Fatal Error Occured. You're Screwed!\n" +
                      errorMessage);
        }

        private bool CanSave()
        {
            return Locations == null || Locations.All(a => !a.HasErrors);
        }

        #endregion
        
    }
}