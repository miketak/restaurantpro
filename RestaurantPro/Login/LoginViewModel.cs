using System;
using System.Diagnostics;
using System.Security;
using System.Threading.Tasks;
using System.Windows;
using AutoMapper;
using MahApps.Metro.Controls.Dialogs;
using RestaurantPro.Core;
using RestaurantPro.Core.Domain;
using RestaurantPro.Infrastructure.Services;
using RestaurantPro.Models;

namespace RestaurantPro.Login
{
    /// <inheritdoc />
    /// <summary>
    /// View model for Login View
    /// </summary>
    public class LoginViewModel : BindableBase
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IDialogCoordinator dialogCoordinator;

        public SecureString SecurePassword { private get; set; }

        /// <summary>
        /// Login View Model Constructor
        /// </summary>
        public LoginViewModel(IUnitOfWork unitOfWork, IDialogCoordinator instance)
        {
            _unitOfWork = unitOfWork;
            LoginCommand = new RelayCommand(OnLogin, CanLogin);
            dialogCoordinator = instance;
        }

        #region Object Binding

        private WpfUser _CurrentUser;

        /// <summary>
        /// Current User
        /// </summary>
        public WpfUser CurrentUser
        {
            get { return _CurrentUser; }
            set { SetProperty(ref _CurrentUser, value); }
        }

        #endregion
        
        #region Commands

        public RelayCommand LoginCommand { get; private set; }

        #endregion

        #region Events

        /// <summary>
        /// Action Event to navigate to Home Dashboard
        /// </summary>
        public event Action<WpfUser> LoginRequested = delegate { };

        #endregion

        #region Event Handling Implementations

        private async void OnLogin()
        {
            var controller = await dialogCoordinator.ShowProgressAsync(this, "Stay Tuned", "Checking if you're not a thief", false, null);
            User user = null;
           string errorMessage = null;
            try
            {
                controller.SetIndeterminate();
                user = await _unitOfWork.UserAuthenticationService.AuthenticateUser(CurrentUser.Username, SecurePassword);
                await Task.Delay(2000);
                await controller.CloseAsync();

                CurrentUser = MapUserToWpfUser(user);
            }
            catch (Exception e)
            {
                Debug.WriteLine(e.Message);
                errorMessage = e.Message;
            }

            if (user == null)
            {
                controller.SetTitle("Go seek the kingdom of God, Thief");
                controller.SetMessage("Invalid Username or password\n" + errorMessage);
                await Task.Delay(2000);
                await controller.CloseAsync();
                return;
            }

            LoginRequested(CurrentUser);
        }

        private bool CanLogin()
        {
            return !CurrentUser.HasErrors;
        }

        #endregion

        #region Private Helper Methods

        private WpfUser MapUserToWpfUser(User source)
        {
            var config = new MapperConfiguration(cfg => { cfg.CreateMap<User, WpfUser>(); });

            IMapper iMapper = config.CreateMapper();

            return iMapper.Map<User, WpfUser>(source);
        }

        #endregion
    }


    
}
