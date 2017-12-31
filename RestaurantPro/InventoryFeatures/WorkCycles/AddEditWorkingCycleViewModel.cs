using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Linq;
using System.Windows;
using RestaurantPro.Core;
using RestaurantPro.Core.Domain;
using RestaurantPro.Models;

namespace RestaurantPro.InventoryFeatures.WorkCycles
{
    public class AddEditWorkingCycleViewModel : BindableBase
    {
        private bool _editMode;
        private readonly IUnitOfWork _unitOfWork;

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
            get { return _editMode; }
            set { SetProperty(ref _editMode, value); }
        }

        public void SetWorkingCycle(WpfWorkCycle wCycle)
        {
            WorkCycle = wCycle;

            if (EditMode)
            {
                var workCycleWithLines = _unitOfWork
                    .WorkCycles
                    .GetWorkCycleByWorkCycleName(WorkCycle.Name, true);

                if (workCycleWithLines != null)
                    WorkCycle.Lines = new BindingList<WpfWorkCycleLines>(
                        RestproMapper.MapWorkCycleLinesToWpfWorkCycleList(workCycleWithLines
                        .WorkCycleLines.ToList()));
            }
            else
            {
                WorkCycle.Lines = new BindingList<WpfWorkCycleLines>();
            } 

            if (WorkCycle != null)
                WorkCycle.ErrorsChanged -= RaiseCanExecuteChanged;

            WorkCycle.ErrorsChanged += RaiseCanExecuteChanged;
            this.WorkCycle.Lines.ListChanged += this.OnListChanged;
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
            set
            {
                if (WorkCycle != null)
                    WorkCycle.UpdateSubTotalSection();
                SetProperty(ref _wpfWorkCycle, value);
            }
        }

        #endregion

        #region Events

        public event Action<WpfUser> LogoutRequested = delegate { };

        public event Action<WpfUser> ManageWorkCyclesRequsted = delegate { };

        public event Action<WpfUser> Done = delegate { };

        public event ListChangedEventHandler ListChanged;
        void OnListChanged(object sender, ListChangedEventArgs e)
        {
            WorkCycle.UpdateSubTotalSection();
        }

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

            if (_editMode)
            {
                _unitOfWork.WorkCycles.UpdateWorkCycle(workCycleEntity);
            }
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