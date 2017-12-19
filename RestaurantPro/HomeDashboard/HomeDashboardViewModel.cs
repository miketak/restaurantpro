﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RestaurantPro.Models;

namespace RestaurantPro.HomeDashboard
{
    public class HomeDashboardViewModel : BindableBase
    {
     
        /// <summary>
        /// Sets Current User in the system.
        /// </summary>
        /// <param name="user"></param>
        public void SetCurrentUser(WpfUser user)
        {
            CurrentUser = user;
            LogoutCommand = new RelayCommand(OnLogout);
            InventoryDashboardCommand = new RelayCommand(OnInventoryMgtClick);
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

        #region Relay Events
        public event Action<WpfUser> LogoutRequested = delegate { };

        public event Action<WpfUser> InventoryDashboardRequested = delegate { };

        

        #endregion

        #region Command Declarations

        public RelayCommand LogoutCommand { get; private set; }

        public RelayCommand InventoryDashboardCommand { get; private set; }

        public RelayCommand ManageWorkCyclesCommand { get; private set; }
        
        #endregion

        #region Command Implementations

        private void OnLogout()
        {
            CurrentUser = null;
            LogoutRequested(new WpfUser());
        }

        private void OnInventoryMgtClick()
        {
            InventoryDashboardRequested(CurrentUser);
        }


        #endregion
    }


}
