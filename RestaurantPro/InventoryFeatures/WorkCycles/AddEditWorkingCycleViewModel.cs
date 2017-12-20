using System;
using System.ComponentModel;
using RestaurantPro.Core;
using RestaurantPro.Models;

namespace RestaurantPro.InventoryFeatures.WorkCycles
{
    public class AddEditWorkingCycleViewModel : BindableBase
    {
        private IUnitOfWork _unitOfWork;
        private bool _EditMode;

        public AddEditWorkingCycleViewModel(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
            LogoutCommand = new RelayCommand(OnLogout);
            BackToWorkCycleListCommand = new RelayCommand(OnManageCyclesListClick);
            CancelCommand = new RelayCommand(OnManageCyclesListClick);
            SaveCommand = new RelayCommand(OnSave, CanSave);

        }

        #region Initial Setting

        public void SetCurrentUser(WpfUser user)
        {
            CurrentUser = user;
        }

        public bool EditMode
        {
            get { return _EditMode; }
            set { SetProperty(ref _EditMode, value); }
        }


        //private WpfWorkCycle _editingWorkCycle = null;
        public void SetWorkingCycle(WpfWorkCycle wCycle)
        {
            WorkCycle = wCycle;
            if (WorkCycle != null)
                WorkCycle.ErrorsChanged -= RaiseCanExecuteChanged;

            WorkCycle.ErrorsChanged += RaiseCanExecuteChanged;

        }

        private void RaiseCanExecuteChanged(object sender, DataErrorsChangedEventArgs e)
        {
            SaveCommand.RaiseCanExecuteChanged();
        }

        #endregion

        


        #region Bindable Objects

        private WpfUser _CurrentUser;

        public WpfUser CurrentUser
        {
            get { return _CurrentUser; }
            set { SetProperty(ref _CurrentUser, value); }
        }

        private WpfWorkCycle _wpfWorkCycle;

        public WpfWorkCycle WorkCycle
        {
            get { return _wpfWorkCycle; }
            set { SetProperty(ref _wpfWorkCycle, value);}
        }

        #endregion

        #region Events

        public event Action<WpfUser> LogoutRequested = delegate { };

        public event Action<WpfUser> ManageWorkCyclesRequsted = delegate { };

        public event Action<WpfUser> Done = delegate { };

        #endregion

        #region Commands

        public RelayCommand LogoutCommand { get; private set; }

        public RelayCommand BackToWorkCycleListCommand { get; private set; }

        public RelayCommand CancelCommand { get; private set; }

        public RelayCommand SaveCommand { get; private set; }

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

        private void OnSave()
        {
            WorkCycle = AppendCurrentUser(WorkCycle);
            var workCycleEntity = RestproMapper.MapWpfWorkCycleToWorkCycle(WorkCycle);

            if (_EditMode)
                _unitOfWork.WorkCycles.UpdateWorkCycle(workCycleEntity);
            else
            {
                _unitOfWork.WorkCycles.Add(workCycleEntity);
                _unitOfWork.Complete();
            }
            Done(CurrentUser);
        }

        private WpfWorkCycle AppendCurrentUser(WpfWorkCycle w)
        {
            w.UserId = CurrentUser.Id;
            return w;
        }

        private bool CanSave()
        {
            return !WorkCycle.HasErrors;
        }

        #endregion

    }
}