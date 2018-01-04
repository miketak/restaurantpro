using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using MahApps.Metro.Controls.Dialogs;
using System.Data;
using System.Linq;
using System.Windows;
using AutoMapper;
using Microsoft.Windows.Controls;
using RestaurantPro.Core;
using RestaurantPro.Core.Domain;
using RestaurantPro.Models;
using Unity.ObjectBuilder.BuildPlan.Selection;

namespace RestaurantPro.InventoryFeatures.WorkCycles
{
    public class WorkCycleListViewModel : BindableBase
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IDialogCoordinator dialogCoordinator;
        
        /// <summary>
        /// Initialized events and commands
        /// </summary>
        /// <param name="unitOfWork">Unit Of Work</param>
        public WorkCycleListViewModel(IUnitOfWork unitOfWork, IDialogCoordinator instance)
        {
            _unitOfWork = unitOfWork;
            dialogCoordinator = instance;

            BackToInventoryCommand = new RelayCommand(OnBackToInventoryInventoryClick);
            LogoutCommand = new RelayCommand(OnLogout);
            DeactivateWorkCycleCommand = new RelayCommand<WpfWorkCycle>(DeactivateWorkCycle);
            DeleteWorkingCycleCommand = new RelayCommand<WpfWorkCycle>(DeleteWorkCycle);
            AddWorkingCycleCommand = new RelayCommand(OnAddWorkCycle);
            EditWorkCycleCommand = new RelayCommand<WpfWorkCycle>(OnEditWorkCycle);
        }

        #region Initialization Methods

        /// <summary>
        /// Initialize View Model Commands for View and sets
        /// the current user.
        /// </summary>
        /// <param name="user"></param>
        public void SetCurrentUser(WpfUser user)
        {
            CurrentUser = user;
        }

        /// <summary>
        /// Loads Work Cycles with true flag from database
        /// </summary>
        public void LoadWorkCycles()
        {
            var workCyclesInDb = _unitOfWork.WorkCycles
                .GetAll()
                .Where(t => t.Active).ToList();

            var wpfWorkCycles = RestproMapper.MapWorkCycleListToWpfWorkCycleList(workCyclesInDb);

            wpfWorkCycles = AppendCreatedByUsers(wpfWorkCycles);

            WorkCycles = new ObservableCollection<WpfWorkCycle>(wpfWorkCycles);
        }

 
        #endregion

        #region Datagrid Event Handling

        private void DeactivateWorkCycle(WpfWorkCycle wpfWorkCycle)
        {
            _unitOfWork.WorkCycles.DeactivateWorkCycle(wpfWorkCycle.Id);
            LoadWorkCycles();
        }

        private async void DeleteWorkCycle(WpfWorkCycle wpfWorkCycle)
        {
            var workCycleEntity = RestproMapper.MapWpfWorkCycleToWorkCycle(wpfWorkCycle);

            if (workCycleEntity == null)
            {
                await dialogCoordinator.ShowMessageAsync(this, "WARNING", "KINDLY SELECT A WORK CYCLE");
                return;
            }

            workCycleEntity = _unitOfWork.WorkCycles.SingleOrDefault(
                w => w.Id == workCycleEntity.Id);

            _unitOfWork.WorkCycles.Remove(workCycleEntity);
            _unitOfWork.Complete();

            WorkCycles.Remove(wpfWorkCycle);
        }

        #endregion

        #region Object Bindings

        private WpfUser _CurrentUser;

        public WpfUser CurrentUser
        {
            get { return _CurrentUser; }
            set { SetProperty(ref _CurrentUser, value); }
        }

        private ObservableCollection<WpfWorkCycle> _workCycles;

        public ObservableCollection<WpfWorkCycle> WorkCycles
        {
            get { return _workCycles; }
            set { SetProperty(ref _workCycles, value);}
        }

        #endregion

        #region Events

        public event Action<WpfUser> LogoutRequested = delegate { };

        public event Action<WpfUser> InventoryDashboardRequested = delegate { };

        public event Action<WpfWorkCycle, WpfUser> AddWorkCycleRequested = delegate { };

        public event Action<WpfWorkCycle, WpfUser> EditWorkCycleRequested = delegate { };

        #endregion

        #region Command

        public RelayCommand LogoutCommand { get; private set; }

        public RelayCommand BackToInventoryCommand { get; private set; }

        public RelayCommand<WpfWorkCycle> DeactivateWorkCycleCommand { get; private set; }

        public RelayCommand<WpfWorkCycle> DeleteWorkingCycleCommand { get; private set; }

        public RelayCommand AddWorkingCycleCommand { get; private set; }

        public RelayCommand<WpfWorkCycle> EditWorkCycleCommand { get; private set; }

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

        private void OnAddWorkCycle()
        {
            AddWorkCycleRequested(new WpfWorkCycle(), CurrentUser);
        }

        private void OnEditWorkCycle(WpfWorkCycle wpfWorkCycle)
        {
            EditWorkCycleRequested(wpfWorkCycle, CurrentUser);
        }

        #endregion

        #region Private Helper Methods

        private List<WpfWorkCycle> AppendCreatedByUsers(List<WpfWorkCycle> wpfWorkCycles)
        {
            foreach (var workCycle in wpfWorkCycles)
            {
                workCycle.FirstName = _unitOfWork.Users
                    .SingleOrDefault(user => user.Id == workCycle.UserId)
                    .FirstName;

                workCycle.LastName = _unitOfWork.Users
                    .SingleOrDefault(user => user.Id == workCycle.UserId)
                    .LastName;
            }

            return wpfWorkCycles;
        }

        #endregion
    }
}