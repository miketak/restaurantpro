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
        private MVUser currentUser;

        /// <summary>
        /// Sets Current User in the system.
        /// </summary>
        /// <param name="user"></param>
        public void SetCurrentUser(MVUser user)
        {
            currentUser = user;
        }



    }


}
