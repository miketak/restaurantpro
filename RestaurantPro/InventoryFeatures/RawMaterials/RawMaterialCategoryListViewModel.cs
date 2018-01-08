using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using MahApps.Metro.Controls.Dialogs;
using RestaurantPro.Core;
using RestaurantPro.Models;

namespace RestaurantPro.InventoryFeatures.RawMaterials
{
    /// <summary>
    /// Raw Materials Category View Model
    /// </summary>
    public class RawMaterialCategoryListViewModel : BindableBase
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IDialogCoordinator dialogCoordinator;

        public RawMaterialCategoryListViewModel(IUnitOfWork unitOfWork, IDialogCoordinator instance)
        {
            _unitOfWork = unitOfWork;
            dialogCoordinator = instance;

            BackToInventoryCommand = new RelayCommand(OnBackToInventoryInventoryClick);
            LogoutCommand = new RelayCommand(OnLogout);
            BackHomeCommand = new RelayCommand(OnHomeClick);
            SaveCommand = new RelayCommand(OnSave, CanSave);
            DeleteCommand = new RelayCommand<WpfRawMaterialCategory>(OnDeleteClick);

        }

        #region Object Bindings

        private WpfUser _currentUser;
        public WpfUser CurrentUser
        {
            get { return _currentUser; }
            set { SetProperty(ref _currentUser, value); }
        }

        private BindingList<WpfRawMaterialCategory> _categories;

        public BindingList<WpfRawMaterialCategory> Categories
        {
            get { return _categories; }
            set { SetProperty(ref _categories, value); }
        }

        #endregion


        #region Initialization Methods

        public void SetCurrentUser(WpfUser user)
        {
            CurrentUser = user;
        }

        public void LoadCategories()
        {
            var categoriesInDb = _unitOfWork.RawMaterialCategories
                .GetAll().ToList();

            var categoriesForView =
                RestproMapper.MapRawMaterialCategoryListToWpfRawMaterialCategoryList(categoriesInDb);

            Categories = new BindingList<WpfRawMaterialCategory>(categoriesForView);
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

        public RelayCommand<WpfRawMaterialCategory> DeleteCommand { get; private set; }       

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
            var categoriesToDb = RestproMapper
                .MapWpfRawMaterialCategoryListToRawMaterialCategoryList(Categories.ToList());
            string errorMessage = null;

            try
            {
                _unitOfWork.RawMaterialCategories.AddOrUpdateRawMaterials(categoriesToDb);
                LoadCategories();
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

        private async void OnDeleteClick(WpfRawMaterialCategory wpfCategory)
        {
            if (wpfCategory == null)
                return;

            var rawMaterialInDb = RestproMapper.MapWpfRawMaterialCategoryToRawMaterialCategory(wpfCategory);
            string errorMessage = null;

            try
            {
                Categories.Remove(wpfCategory);
                if (wpfCategory.Id != 0)
                    _unitOfWork.RawMaterialCategories.FakeDeleteSupplier(rawMaterialInDb);
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
            return Categories == null || Categories.All(a => !a.HasErrors);
        }

        #endregion

    }
}