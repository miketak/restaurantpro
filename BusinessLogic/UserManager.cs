using DataAccessLayer;
using DataObjects;
using DataObjects.ProgramDataObjects;
using System;
using System.Collections.Generic;
using System.Linq;
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
        /// Hash Encryption for Password
        /// </summary>
        /// <param name="source">Source string</param>
        /// <returns>Encrypted String</returns>
        internal string HashSHA256(string source)   //change back to internal
        {
            var result = "";

            // this logic is always the same for our purposes
            // create a byte array (8 bit unsigned int)
            byte[] data;

            // Hash providers are all created with factory methods.
            using (SHA256 sha256hash = SHA256.Create())
            {
                // hash the input
                data = sha256hash.ComputeHash(Encoding.UTF8.GetBytes(source));
            }

            // use a stringbuilder to conserve memory
            var s = new StringBuilder();

            // loop through the bytes creating characts
            for (int i = 0; i < data.Length; i++)
            {
                s.Append(data[i].ToString("x2"));
            }

            result = s.ToString();
            return result;
        }
        

        /// <summary>
        /// Authenticates employee
        /// </summary>
        /// <param name="username">Entered username</param>
        /// <param name="password">Entered password</param>
        /// <returns>Returns User if Authentication Passes</returns>
        public User AuthenticateUser(string username, string password)
        {
            User user = null;
            
            //Username & Password pre-validation
            if (username.Length < 5 || username.Length > 20)
            {
                throw new ApplicationException("Invalid Username");
            }
            if (password.Length < 3) // we really need a better method
            {                        // possibly a regex for complexity
                throw new ApplicationException("Invalid Password");
            }

            try
            {
                if ( UserAccessor.VerifyUsernameAndPassword( username, password)) //HashSHA256(password) ) )
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
            var result = false;
            try
            {
                if (1 == UserAccessor.UpdatePasswordHash(userID, HashSHA256(oldPassword), HashSHA256(newPassword)))
                {
                    result = true;
                }
                else
                {
                    result = false;
                }
            }
            catch (Exception)
            {
                throw;
            }
            return result;
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
