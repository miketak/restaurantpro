using System;
using RestaurantPro.Core;
using RestaurantPro.Models;

namespace RestaurantPro.InventoryFeatures.WorkCycles
{
    public class AddEditWorkingCycleViewModel : BindableBase
    {
        private IUnitOfWork _unitOfWork;

        public AddEditWorkingCycleViewModel(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public void SetCurrentUserAndInitializeCommands(WpfUser user)
        {
            CurrentUser = user;
            LogoutCommand = new RelayCommand(OnLogout);
            BackToWorkCycleListCommand = new RelayCommand(OnManageCyclesListClick);
            CancelCommand = new RelayCommand(OnManageCyclesListClick);
        }


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

        public event Action<WpfUser> ManageWorkCyclesRequsted = delegate { };

        #endregion

        #region Commands

        public RelayCommand LogoutCommand { get; private set; }

        public RelayCommand BackToWorkCycleListCommand { get; private set; }

        public RelayCommand CancelCommand { get; private set; }

        #endregion

        #region Command Implementations

        private void OnLogout()
        {
            CurrentUser = null;
            LogoutRequested(new WpfUser());
        }

        private void OnManageCyclesListClick()
        {
            ManageWorkCyclesRequsted(CurrentUser);
        }

        #endregion

    }
}