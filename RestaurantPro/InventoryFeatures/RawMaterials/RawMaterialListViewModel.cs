using System;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using MahApps.Metro.Controls.Dialogs;
using RestaurantPro.Core;
using RestaurantPro.Models;

namespace RestaurantPro.InventoryFeatures.RawMaterials
{
    /// <summary>
    /// Raw Material List View Model
    /// </summary>
    public class RawMaterialListViewModel : BindableBase
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IDialogCoordinator dialogCoordinator;

        public RawMaterialListViewModel(IUnitOfWork unitOfWork, IDialogCoordinator instance)
        {
            _unitOfWork = unitOfWork;
            dialogCoordinator = instance;

            BackToInventoryCommand = new RelayCommand(OnBackToInventoryInventoryClick);
            LogoutCommand = new RelayCommand(OnLogout);
            BackHomeCommand = new RelayCommand(OnHomeClick);
            SaveCommand = new RelayCommand(OnSave, CanSave);
            DeleteCommand = new RelayCommand<WpfRawMaterial>(OnDeleteClick);

        }

        #region Object Bindings

        private WpfUser _currentUser;
        public WpfUser CurrentUser
        {
            get { return _currentUser; }
            set { SetProperty(ref _currentUser, value); }
        }

        private BindingList<WpfRawMaterial> _rawMaterials;
        public BindingList<WpfRawMaterial> RawMaterials
        {
            get { return _rawMaterials; }
            set { SetProperty(ref _rawMaterials, value); }
        }

        #endregion

        #region Initialization Methods

        public void SetCurrentUser(WpfUser user)
        {
            CurrentUser = user;
        }

        public void LoadRawMaterials()
        {
            var rawMaterialsInDb = _unitOfWork.RawMaterials.GetRawMaterials().ToList();

            var rawMaterialsForView = RestproMapper.MapRawMaterialListToWpfRawMaterialList(rawMaterialsInDb);

            RawMaterials = new BindingList<WpfRawMaterial>(rawMaterialsForView);
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

        public RelayCommand<WpfRawMaterial> DeleteCommand { get; private set; }

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
            var rawMaterialsToDb = RestproMapper
                .MapWpfRawMaterialLIstToRawMaterialList(RawMaterials.ToList());
            string errorMessage = null;

            try
            {
                _unitOfWork.RawMaterials.AddOrUpdateRawMaterials(rawMaterialsToDb);
                LoadRawMaterials();
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

        private async void OnDeleteClick(WpfRawMaterial wpfRawMaterial)
        {
            if (wpfRawMaterial == null)
                return;

            var rawMaterialToDb = RestproMapper.MapWpfRawMaterialToRawMaterial(wpfRawMaterial);
            string errorMessage = null;

            try
            {
                RawMaterials.Remove(wpfRawMaterial);
                if ( wpfRawMaterial.Id != 0 )
                    _unitOfWork.RawMaterials.FakeDelete(rawMaterialToDb);
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
            return RawMaterials == null || RawMaterials.All(a => !a.HasErrors);
        }

        #endregion

    }
}