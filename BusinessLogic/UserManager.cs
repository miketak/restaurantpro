using DataAccessLayer;
using DataObjects;
using DataObjects.ProgramDataObjects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLogic
{

    /// <summary>
    /// Manages all User module level activities
    /// </summary>
    public class UserManager
    {
        /// <summary>
        /// Authenticates employee
        /// </summary>
        /// <param name="username">Entered username</param>
        /// <param name="password">Entered password</param>
        /// <returns>Returns User if Authentication Passes</returns>
        public User AuthenticateUser(string username, SecureString password)
        {
            User user = null;
            
            //Username & Password pre-validation
            if (username.Length < 2 || username.Length > 20)
            {
                throw new ApplicationException("Invalid Username");
            }
            if (password.Length < 3) // we really need a better method
            {                        // possibly a regex for complexity
                throw new ApplicationException("Invalid Password");
            }

            try
            {
                if ( UserAccessor.VerifyUsernameAndPassword( username, password))
                {
                    password = null;
                    // need to create a employee object to use as an access token

                    // get a employee object
                    user = UserAccessor.RetrieveUserByUsername(username);
                }
                else
                {
                    throw new ApplicationException("Authentication Failed!");
                }

            }
            catch (Exception)
            {
                throw;
            }
            return user;
        }


        /// <summary>
        /// Updates Password 
        /// </summary>
        /// <param name="userID">Users UserID</param>
        /// <param name="oldPassword">User's Old Password</param>
        /// <param name="newPassword">User's New Password</param>
        /// <returns></returns>
        public bool UpdatePassword(int userID, string oldPassword, string newPassword)
        {
           throw new NotImplementedException();
        }


        private ClearanceAccess RetrieveClearanceAccess( int clearanceLevelID, int functionsID )
        {
            var clearanceAccess = new ClearanceAccess();

            clearanceAccess = UserAccessor.RetrieveClearanceAccess( clearanceLevelID, functionsID );

            return clearanceAccess;
        }
        
        /// <summary>
        /// To Retrieve User Access Data
        /// </summary>
        /// <param name="user">Current User</param>
        /// <returns>ClearanceAccess list DTO</returns>
        public List<ClearanceAccess> RetrieveUserAccess ( User user )
        {
            var userAccess = new List<ClearanceAccess>();
            var clearanceAccess = new List<ClearanceAccess>();

            //Retrieve Functions available to Specific UserRole
            userAccess = UserAccessor.RetrieveUserAccess( user.UserRolesId );

            foreach ( var k in userAccess )
            { 
                //Use User Clearance Level to retrieve access properties
                clearanceAccess.Add( RetrieveClearanceAccess(user.ClearanceLevelId, k.FeatureID) );
            }

            return clearanceAccess;
        }
    }
}
