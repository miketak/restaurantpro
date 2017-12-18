using System;
using System.Collections.Generic;
using System.Linq;
using System.Security;
using System.Text;
using System.Threading.Tasks;
using RestaurantPro.Core.Domain;

namespace RestaurantPro.Core.Services
{
    public interface IUserAuthenticationService
    {
        User AuthenticateUser(string username, SecureString password);

        //bool VerifyUsernameAndPassword(string username, SecureString password);

    }
}
