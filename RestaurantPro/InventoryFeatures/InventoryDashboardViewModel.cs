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

        #endregion

        #region Commands

        public RelayCommand LogoutCommand { get; private set; }

        public RelayCommand BackHomeCommand { get; private set; }

        public RelayCommand ManageWorkCyclesCommand { get; private set; }


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

        #endregion

    }
}