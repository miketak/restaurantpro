using DataObjects;
using DataObjects.ProgramDataObjects;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccessLayer
{

    /// <summary>
    /// Data Access for User Related Functionality
    /// </summary>
    public class UserAccessor
    {

        /// <summary>
        /// Verifies username and password
        /// </summary>
        /// <param name="username">Username for verification</param>
        /// <param name="passwordHash">Password for verification</param>
        /// <returns></returns>
        public static bool VerifyUsernameAndPassword(string username, string passwordHash)
        {
            return username == "rkpadi" && passwordHash == "password" ||
                   username == "linda" && passwordHash == "password";

        }

        /// <summary>
        /// Retrieves User by Username
        /// </summary>
        /// <param name="username"></param>
        /// <returns></returns>
        public static User RetrieveUserByUsername(string username)
        {
            switch (username)
            {
                case "rkpadi":
                    return new User {Username = "rkpadi", FirstName = "Richard", LastName = "Padi"};
                case "linda":
                    return new User {Username = "linda", FirstName = "Linda", LastName = "Ocloo"};
            }

            return null;
        }

        /// <summary>
        /// Updates Password for a User
        /// </summary>
        /// <param name="userID"></param>
        /// <param name="oldPasswordHash"></param>
        /// <param name="newPasswordHash"></param>
        /// <returns></returns>
        public static int UpdatePasswordHash(int userID, string oldPasswordHash, string newPasswordHash)
        {
            var count = 0;

            var conn = DBConnection.GetConnection();
            var cmdText = @"sp_update_passwordHash";
            var cmd = new SqlCommand(cmdText, conn);

            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.Add("@UserID", SqlDbType.Int);
            cmd.Parameters.Add("@OldPasswordHash", SqlDbType.VarChar, 100);
            cmd.Parameters.Add("@NewPasswordHash", SqlDbType.VarChar, 100);

            cmd.Parameters["@UserID"].Value = userID;
            cmd.Parameters["@OldPasswordHash"].Value = oldPasswordHash;
            cmd.Parameters["@NewPasswordHash"].Value = newPasswordHash;

            try
            {
                conn.Open();
                count = cmd.ExecuteNonQuery();
            }
            catch (Exception)
            {
                throw;
            }
            finally
            {
                conn.Close();
            }
            return count;
        }

        /// <summary>
        /// For Role Based Stuff
        /// </summary>
        /// <param name="clearanceLevelID"></param>
        /// <param name="functionsID"></param>
        /// <returns></returns>
        public static ClearanceAccess RetrieveClearanceAccess(int clearanceLevelID, int functionsID)
        {
            ClearanceAccess clearanceAccessInDB = null;

            //Getting connection
            var conn = DBConnection.GetConnection();

            //Using stored procedure
            var cmdText = @"sp_retrieve_user_clearances_by_functionsID";
            var cmd = new SqlCommand(cmdText, conn);
            cmd.CommandType = CommandType.StoredProcedure;

            // add a parameter to the command
            cmd.Parameters.Add("@ClearanceLevelID", SqlDbType.Int);
            cmd.Parameters["@ClearanceLevelID"].Value = clearanceLevelID;
            cmd.Parameters.Add("@FunctionsID", SqlDbType.Int);
            cmd.Parameters["@FunctionsID"].Value = functionsID;

            try
            {
                // you have to open a connection before using it
                conn.Open();

                // create a data reader with our command
                var reader = cmd.ExecuteReader();

                // check to make sure the reader has data
                if (reader.HasRows)
                {
                    reader.Read();
                    clearanceAccessInDB = new ClearanceAccess()
                    {
                        FeatureID = reader.GetInt32(0),
                        FeatureName = reader.GetString(1),
                        hasAccess = reader.GetBoolean(2),
                        isEditable = reader.GetBoolean(3)
                    };
                    // close the reader
                    reader.Close();
                }
            }
            catch (Exception ex)
            {
                throw new ApplicationException("There was a problem retrieving clearance data.", ex);
            }
            finally
            {
                // final housekeeping
                conn.Close();
            }

            return clearanceAccessInDB;
        }

        /// <summary>
        /// Retrieves User Access Data
        /// </summary>
        /// <param name="userRolesID">UserRole of User</param>
        /// <returns>List<CrearanceAccess) DTO</returns>
        public static List<ClearanceAccess> RetrieveUserAccess(string userRolesID)
        {
            var userAccessInDB = new List<ClearanceAccess>();

            //Getting connection
            var conn = DBConnection.GetConnection();

            //Using stored procedure
            var cmdText = @"sp_retrieve_user_access";
            var cmd = new SqlCommand(cmdText, conn);
            cmd.CommandType = CommandType.StoredProcedure;

            // add a parameter to the command
            cmd.Parameters.Add("@UserRolesID", SqlDbType.VarChar, 6);
            cmd.Parameters["@UserRolesID"].Value = userRolesID;

            try
            {
                // you have to open a connection before using it
                conn.Open();

                // create a data reader with our command
                var reader = cmd.ExecuteReader();

                // check to make sure the reader has data
                if (reader.HasRows)
                {
                    while ( reader.Read() )
                    {
                        // create an employee object
                        var userAccess = new ClearanceAccess()
                        {
                            FeatureID = reader.GetInt32(0),
                            UserRolesID = reader.GetString(1)
                        };
                        userAccessInDB.Add(userAccess);
                    }
                    
                    // close the reader
                    reader.Close();
                }
            }
            catch (Exception ex)
            {
                throw new ApplicationException("There was a problem retrieving user access data.", ex);
            }
            finally
            {
                // final housekeeping
                conn.Close();
            }

            return userAccessInDB;
        }
    }
}
