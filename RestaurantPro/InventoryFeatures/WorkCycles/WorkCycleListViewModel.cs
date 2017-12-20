using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data;
using System.Linq;
using AutoMapper;
using Microsoft.Windows.Controls;
using RestaurantPro.Core;
using RestaurantPro.Core.Domain;
using RestaurantPro.Models;

namespace RestaurantPro.InventoryFeatures.WorkCycles
{
    public class WorkCycleListViewModel : BindableBase
    {
        private IUnitOfWork _unitOfWork;
        

        public WorkCycleListViewModel(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }


        /// <summary>
        /// Initialize View Model Commands for View and sets
        /// the current user.
        /// </summary>
        /// <param name="user"></param>
        public void SetCurrentUserAndInitializeCommands(WpfUser user)
        {
            CurrentUser = user;
            BackToInventoryCommand = new RelayCommand(OnBackToInventoryInventoryClick);
            LogoutCommand = new RelayCommand(OnLogout);
            DeactivateWorkCycleCommand = new RelayCommand<WpfWorkCycle>(DeactivateWorkCycle);
            DeleteWorkingCycleCommand = new RelayCommand<WpfWorkCycle>(DeleteWorkCycle);
        }

        #region DataGrid Functionality

        private List<WpfWorkCycle> _allWorkCycles;

        public void LoadWorkCycles()
        {
            var workCycleEntity = _unitOfWork.WorkCycles.GetAll().ToList();

            var wpfWorkCycles = MapWorkCycleListToWpfWorkCycleList(workCycleEntity);

            wpfWorkCycles = AppendCreatedByUsers(wpfWorkCycles);

            WorkCycles = new ObservableCollection<WpfWorkCycle>(wpfWorkCycles);
        }

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

        #region  Automapper

        private List<WpfWorkCycle> MapWorkCycleListToWpfWorkCycleList(List<WorkCycle> workCyclesEntity)
        {
            var config = new MapperConfiguration(cfg =>
            {
                cfg.CreateMap<WorkCycle, WpfWorkCycle>();
            });

            IMapper iMapper = config.CreateMapper();

            _allWorkCycles = iMapper.Map<List<WorkCycle>, List<WpfWorkCycle>>(workCyclesEntity);

            return _allWorkCycles;
        }

        private WorkCycle MapWorkCycleToWpfWorkCycle( WpfWorkCycle wpfWorkCyclesEntity)
        {
            var config = new MapperConfiguration(cfg =>
            {
                cfg.CreateMap<WpfWorkCycle, WorkCycle>();
            });

            IMapper iMapper = config.CreateMapper();

            return  iMapper.Map<WpfWorkCycle, WorkCycle>(wpfWorkCyclesEntity);
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

        #endregion

        #region Command

        public RelayCommand LogoutCommand { get; private set; }

        public RelayCommand BackToInventoryCommand { get; private set; }

        public RelayCommand<WpfWorkCycle> DeactivateWorkCycleCommand { get; private set; }

        public RelayCommand<WpfWorkCycle> DeleteWorkingCycleCommand { get; private set; }

        #endregion

        #region Command Implementations

        private void OnBackToInventoryInventoryClick()
        {
            InventoryDashboardRequested(CurrentUser);
        }

        private void OnLogout()
        {
            CurrentUser = null;
            LogoutRequested(new WpfUser());
        }

        private void DeactivateWorkCycle(WpfWorkCycle wpfWorkCycle)
        {
            _unitOfWork.WorkCycles.DeactivateWorkCycle(wpfWorkCycle.Id);
            LoadWorkCycles();
        }

        private void DeleteWorkCycle(WpfWorkCycle wpfWorkCycle)
        {
            var workCycleEntity = MapWorkCycleToWpfWorkCycle(wpfWorkCycle);

            workCycleEntity = _unitOfWork.WorkCycles.SingleOrDefault(
                w => w.Id == workCycleEntity.Id);

            _unitOfWork.WorkCycles.Remove(workCycleEntity);

            _unitOfWork.Complete();

            LoadWorkCycles();
        }

        #endregion
    }
}