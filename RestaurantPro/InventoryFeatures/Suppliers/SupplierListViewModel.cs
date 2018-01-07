using System;
using System.CodeDom;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using MahApps.Metro.Controls.Dialogs;
using RestaurantPro.Core;
using RestaurantPro.Models;

namespace RestaurantPro.InventoryFeatures.Suppliers
{
    /// <summary>
    /// Supplier List View Model Class
    /// </summary>
    public class SupplierListViewModel : BindableBase
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IDialogCoordinator dialogCoordinator;

        public SupplierListViewModel(IUnitOfWork unitOfWork, IDialogCoordinator instance)
        {
            _unitOfWork = unitOfWork;
            dialogCoordinator = instance;

            BackToInventoryCommand = new RelayCommand(OnBackToInventoryInventoryClick);
            LogoutCommand = new RelayCommand(OnLogout);
            BackHomeCommand = new RelayCommand(OnHomeClick);
            SaveCommand = new RelayCommand(OnSave, CanSave);
            DeleteSupplierCommand = new RelayCommand<WpfSupplier>(OnDeleteClick);
        }

        #region Object Bindings

        private WpfUser _currentUser;
        public WpfUser CurrentUser
        {
            get { return _currentUser; }
            set { SetProperty(ref _currentUser, value);}
        }

        private BindingList<WpfSupplier> _suppliers;
        public BindingList<WpfSupplier> Suppliers
        {
            get { return _suppliers; }
            set { SetProperty(ref _suppliers, value); }
        }

        #endregion

        #region Initialization Methods

        public void SetCurrentUser(WpfUser user)
        {
            CurrentUser = user;
        }

        public void LoadSuppliers()
        {
            var suppliers = _unitOfWork.Suppliers
                .GetAll()
                .Where(u => u.Active).ToList();

            var wpfSuppliers = RestproMapper.MapSupplierListToWpfSupplierList(suppliers);

            Suppliers = new BindingList<WpfSupplier>(wpfSuppliers);
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

        public RelayCommand<WpfSupplier> DeleteSupplierCommand { get; private set; }

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
            var suppliersToDb = RestproMapper
                .MapWpfSupplierListToSupplierList(Suppliers.ToList());
            string errorMessage = null;

            try
            {
                _unitOfWork.Suppliers.AddOrUpdateSuppliers(suppliersToDb);
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

        private async void OnDeleteClick(WpfSupplier supplier)
        {
            if (supplier == null)
                return;

            var supplierToDb = RestproMapper.MapWpfSupplierToSupplier(supplier);
            string errorMessage = null;

            try
            {
                _unitOfWork.Suppliers.FakeDeleteSupplier(supplierToDb);
                Suppliers.Remove(supplier);
                await dialogCoordinator.ShowMessageAsync(this, "Success", "Supplier Deleted Successfully. We're so done with them!");
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
            if (Suppliers == null)
                return true;

            foreach (var a in Suppliers)
            {
                if (a.HasErrors)
                    return false;
            }
            return true;
        }

        #endregion
    }
}