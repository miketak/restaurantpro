using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Linq;
using System.Windows;
using MahApps.Metro.Controls.Dialogs;
using RestaurantPro.Core;
using RestaurantPro.Core.Domain;
using RestaurantPro.Models;

namespace RestaurantPro.InventoryFeatures.WorkCycles
{
    /// <inheritdoc />
    /// <summary>
    /// AddEditView View Model
    /// </summary>
    public class AddEditWorkingCycleViewModel : BindableBase
    {
        private bool _editMode;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IDialogCoordinator dialogCoordinator;

        /// <summary>
        /// Constructor to initialize Events and Unit of Work
        /// </summary>
        /// <param name="unitOfWork"></param>
        public AddEditWorkingCycleViewModel(IUnitOfWork unitOfWork, IDialogCoordinator instance)
        {
            _unitOfWork = unitOfWork;
            dialogCoordinator = instance;

            LogoutCommand = new RelayCommand(OnLogout);
            BackToWorkCycleListCommand = new RelayCommand(OnManageCyclesListClick);
            CancelCommand = new RelayCommand(OnManageCyclesListClick);
            SaveCommand = new RelayCommand(OnSave, CanSave);
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

        /// <summary>
        /// Edit Mode Flag
        /// </summary>
        public bool EditMode
        {
            get { return _editMode; }
            set { SetProperty(ref _editMode, value); }
        }

        /// <summary>
        /// Sets Working Cycle
        /// </summary>
        /// <param name="wCycle">Work Cycle Passed</param>
        public void SetWorkingCycle(WpfWorkCycle wCycle)
        {
            WorkCycle = wCycle;

            wCycle.UserId = CurrentUser.Id;

            if (EditMode)
            {
                var workCycleWithLines = _unitOfWork
                    .WorkCycles
                    .GetWorkCycleByWorkCycleName(WorkCycle.Name, true);

                if(workCycleWithLines == null)
                    throw new ApplicationException("Invalid Work Cycle");

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

        /// <summary>
        /// Current User
        /// </summary>
        public WpfUser CurrentUser
        {
            get { return _CurrentUser; }
            set { SetProperty(ref _CurrentUser, value); }
        }

        private WpfWorkCycle _wpfWorkCycle;

        /// <summary>
        /// Current Work Cycle
        /// </summary>
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

        #endregion

        #region Commands

        public RelayCommand LogoutCommand { get; private set; }

        public RelayCommand BackToWorkCycleListCommand { get; private set; }

        public RelayCommand CancelCommand { get; private set; }

        public RelayCommand SaveCommand { get; private set; }

        #endregion

        #region Event Handling

        private void OnLogout()
        {
            CurrentUser = null;
            LogoutRequested(new WpfUser());
        }

        private void OnManageCyclesListClick()
        {
            ManageWorkCyclesRequsted(CurrentUser);
        }

        private async void OnSave()
        {
            WorkCycle = AppendCurrentUser(WorkCycle);

            var workCycleEntity = RestproMapper.MapWpfWorkCycleToWorkCycleAndHandleLines(WorkCycle);

            if (_editMode)
            {
                _unitOfWork.WorkCycles.UpdateWorkCycle(workCycleEntity);
            }
            else
            {
                workCycleEntity.Active = true;
                _unitOfWork.WorkCycles.AddWorkingCycle(workCycleEntity);
                _unitOfWork.Complete();
            }

            var controller = await dialogCoordinator.ShowMessageAsync(this, "Success", "Items Saved Successfully. You Rock!");

            if (controller == MessageDialogResult.Affirmative)
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

        private void OnListChanged(object sender, ListChangedEventArgs e)
        {
            WorkCycle.UpdateSubTotalSection();
        }

        #endregion

    }
}