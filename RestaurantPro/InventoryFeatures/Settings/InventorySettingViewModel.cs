using System;
using System.Diagnostics;
using MahApps.Metro.Controls.Dialogs;
using RestaurantPro.Core;
using RestaurantPro.Models;

namespace RestaurantPro.InventoryFeatures.Settings
{
    public class InventorySettingViewModel : BindableBase
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IDialogCoordinator dialogCoordinator;

        public InventorySettingViewModel(IUnitOfWork unitOfWork, IDialogCoordinator instance)
        {
            _unitOfWork = unitOfWork;
            dialogCoordinator = instance;

            BackToInventoryCommand = new RelayCommand(OnBackToInventoryInventoryClick);
            LogoutCommand = new RelayCommand(OnLogout);
            BackHomeCommand = new RelayCommand(OnHomeClick);
            TaxSaveCommand = new RelayCommand(OnTaxSave, CanSaveTax);
        }

        #region Object Binding

        private WpfUser _currentUser;
        private InventorySettings _inventorySettings;

        public WpfUser CurrentUser
        {
            get { return _currentUser; }
            set { SetProperty(ref _currentUser, value); }
        }

        public InventorySettings InventorySettings
        {
            get { return _inventorySettings; }
            set { SetProperty(ref _inventorySettings, value); }
        }

        #endregion

        #region Initialization

        public void SetCurrentUser(WpfUser currentUser)
        {
            CurrentUser = currentUser;
        }

        public void LoadSettings()
        {
            var tax = _unitOfWork.InventorySettings.GetTax();
            if (tax != null)
                InventorySettings = new InventorySettings
                {
                    Tax = (decimal) tax
                };
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

        public RelayCommand TaxSaveCommand { get; private set; }

        #endregion

        #region Navigation Event Handling

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

        #region Tax Event Handling

        private async void OnTaxSave()
        {
            _unitOfWork.InventorySettings.SetTax(InventorySettings.Tax);
            string errorMessage = null;

            try
            {
                LoadSettings();
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

        private bool CanSaveTax()
        {
            if( InventorySettings != null)
                return !InventorySettings.HasErrors;
            return true;
        }

        #endregion
    }
}