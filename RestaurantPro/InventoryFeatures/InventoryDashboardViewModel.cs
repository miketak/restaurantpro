using System;
using RestaurantPro.Models;

namespace RestaurantPro.InventoryFeatures
{
    public class InventoryDashboardViewModel : BindableBase
    {
        public void SetCurrentUser(WpfUser user)
        {
            CurrentUser = user;
            LogoutCommand = new RelayCommand(OnLogout);
            BackHomeCommand = new RelayCommand(OnHomeClick);
            ManageWorkCyclesCommand = new RelayCommand(OnManageCyclesClick);
        }


        #region Object Bindings

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

        #region Command Implementations

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