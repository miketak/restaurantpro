using System;
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



    }


}
