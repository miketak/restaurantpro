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
    /// Data Access for Employee Related Functionality
    /// </summary>
    public class EmployeeAccessor
    {
        //Employee CRUD
        /// <summary>
        /// Creates a new Employee
        /// </summary>
        /// <param name="employee"></param>
        /// <returns>Returns a boolean on success/failure</returns>
        public static int CreateEmployee(Employee em)
        {
            int userId = 0;

            var conn = DBConnection.GetConnection();
            var cmdText = @"sp_create_new_user";
            var cmd = new SqlCommand(cmdText, conn);

            //Birthdate Baricade
           // employee.DateOfBirth = employee.DateOfBirth != null ? employee.DateOfBirth : System.Data.SqlTypes.SqlDateTime.MinValue.Value;

            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.Add("@UserID", SqlDbType.Int);
            cmd.Parameters["@UserID"].Value = userId;
            cmd.Parameters.AddWithValue("@FirstName", em.FirstName );
            cmd.Parameters.AddWithValue("@LastName", em.LastName);
            cmd.Parameters.AddWithValue("@OtherNames", em.OtherNames);
            cmd.Parameters.AddWithValue("@PhoneNumber", em.PhoneNumber);
            cmd.Parameters.AddWithValue("@Email", em.Email);
            cmd.Parameters.AddWithValue("@ssnNo", em.SSNo);
            cmd.Parameters.AddWithValue("@picUrl", em.PicUrl);
            cmd.Parameters.AddWithValue("@isEmployed", em.isEmployed);
            cmd.Parameters.AddWithValue("@isBlocked", em.isBlocked);
            cmd.Parameters.AddWithValue("@UserRolesID", em.UserRolesId);
            cmd.Parameters.AddWithValue("@ClearanceLevelID", em.ClearanceLevelId);
            cmd.Parameters.AddWithValue("@Username", em.Username);
            cmd.Parameters.AddWithValue("@HireDate", em.HireDate); //set manually in BL temporarily
            cmd.Parameters.AddWithValue("@PasswordHash", em.PasswordHash);
            cmd.Parameters.AddWithValue("@Gender", em.Gender);
            cmd.Parameters.AddWithValue("@BirthDate", em.DateOfBirth.HasValue ? em.DateOfBirth : (object)DBNull.Value);
            cmd.Parameters.AddWithValue("@CountryID", em.CountryId.HasValue ? em.CountryId : (object)DBNull.Value);
            cmd.Parameters.AddWithValue("@MaritalStatus", em.MaritalStatus);
            cmd.Parameters.AddWithValue("@PersonalEmail", em.PersonalEmail);
            cmd.Parameters.AddWithValue("@PersonalPhoneNumber", em.PersonalPhoneNumber);
            cmd.Parameters.AddWithValue("@AdditionalInfo", em.AdditonalInfo);

            try
            {
                conn.Open();
                userId = (int)cmd.ExecuteScalar();
            }
            catch (Exception)
            {
                throw;
            }
            finally
            {
                conn.Close();
            }

            //Returns the User ID
            return userId;
        }

        /// <summary>
        /// Retrieves all Employee Data by Username
        /// </summary>
        /// <param name="username"></param>
        /// <returns>Returns Employee Object By Username</returns>
        public static Employee RetrieveEmployeeByUsername(string username)
        {
            //Retrieve all details
            Employee employee = null;

            var conn = DBConnection.GetConnection();
            var cmdText = @"sp_retrieve_all_employee_data_by_username";
            var cmd = new SqlCommand(cmdText, conn);
            cmd.CommandType = CommandType.StoredProcedure;

            cmd.Parameters.Add("@Username", SqlDbType.VarChar, 20);
            cmd.Parameters["@Username"].Value = username;

            try
            {
                conn.Open();
                var reader = cmd.ExecuteReader();

                if (reader.HasRows)
                {
                    reader.Read();
                    employee = new Employee()
                    {
                        UserId = reader.GetInt32(0),
                        Username = reader.GetString(1),
                        FirstName = reader.GetString(2),
                        LastName = reader.GetString(3),
                        OtherNames = reader.IsDBNull(4) ? null : reader.GetString(4),
                        DepartmentId = reader.IsDBNull(5) ? null : reader.GetString(5),
                        Department = reader.IsDBNull(6) ? null : reader.GetString(6),
                        PhoneNumber = reader.IsDBNull(7) ? null : reader.GetString(7),
                        Email = reader.IsDBNull(8) ? null : reader.GetString(8),
                        PicUrl = reader.IsDBNull(9) ? null : reader.GetString(9),
                        isEmployed = reader.IsDBNull(10) ? false : reader.GetBoolean(10),
                        isBlocked = reader.IsDBNull(11) ? false : reader.GetBoolean(11),
                        UserRolesId = reader.IsDBNull(12) ? null : reader.GetString(12),
                        JobDesignation = reader.IsDBNull(13) ? null : reader.GetString(13), //role name
                        ClearanceLevelId = reader.IsDBNull(14) ? -1 : reader.GetInt32(14),
                        ClearanceLevel = reader.IsDBNull(15) ? null : reader.GetString(15),
                        //PersonalInfoId = reader.IsDBNull(16) ? -1 : reader.GetInt32(16),
                        //Gender = reader.IsDBNull(17) ? null : reader.GetBoolean(17),
                        DateOfBirth = reader.IsDBNull(17) ? DateTime.MinValue : reader.GetDateTime(17),
                        CountryId = reader.IsDBNull(18) ? -1 : reader.GetInt32(18),
                        Nationality = reader.IsDBNull(19) ? null : reader.GetString(19),
                        AdditonalInfo = reader.IsDBNull(20) ? null : reader.GetString(20),
                        HireDate = reader.IsDBNull(21) ? DateTime.MinValue : reader.GetDateTime(21),
                        MaritalStatus = reader.IsDBNull(22) ? false : reader.GetBoolean(22),
                        PersonalEmail = reader.IsDBNull(23) ? null : reader.GetString(23),
                        PersonalPhoneNumber = reader.IsDBNull(24) ? null : reader.GetString(24)
                    };

                    //Null gender if not exists in database
                    if (reader.IsDBNull(16))
                        employee.Gender = null;
                    else
                        employee.Gender = reader.GetBoolean(16);

                }
            }
            catch (Exception e)
            {
                throw;
            }
            finally
            {
                conn.Close();
            }

            //Get List of Personal Addresses
            var addressList = new List<Address>();
            addressList = retrieveAddressesByUserID(employee.UserId);
            employee.Address = addressList;
            

            return employee;

        }

        /// <summary>
        /// Retrieve active or inactive employee based on paramater. Retrieves all details
        /// </summary>
        /// <param name="isEmployed">Employee Activity</param>
        /// <returns>Retruns Employee List</returns>
        public static List<Employee> RetrieveEmployees(bool isEmployed)
        {
            var employeesInDB = new List<Employee>();

           //Getting connection
            var conn = DBConnection.GetConnection();

            //Using stored procedure
            var cmdText = @"sp_retrieve_user_by_activity";
            var cmd = new SqlCommand(cmdText, conn);
            cmd.CommandType = CommandType.StoredProcedure;


            // add a parameter to the command
            int activeBit = isEmployed ? 1 : 0;
            cmd.Parameters.Add("@isEmployed", SqlDbType.Bit);
            cmd.Parameters["@isEmployed"].Value = activeBit;

            try
            {
                // you have to open a connection before using it
                conn.Open();

                // create a data reader with our command
                var reader = cmd.ExecuteReader();

                // check to make sure the reader has data
                if (reader.HasRows)
                {
                    // process the data reader
                    while (reader.Read())
                    {
                        // create an employee object
                        var emp = new Employee()
                        {
                            UserId = reader.GetInt32(0),
                            Username = reader.GetString(1),
                            FirstName = reader.GetString(2),
                            LastName = reader.GetString(3),
                            OtherNames = reader.GetString(4),
                            PhoneNumber = reader.GetString(5),
                            Email = reader.GetString(6),
                            DepartmentId = reader.GetString(7),
                            Department = reader.GetString(8),
                            UserRolesId = reader.GetString(9),
                            JobDesignation = reader.GetString(10),
                            ClearanceLevelId = reader.GetInt32(11),
                            ClearanceLevel = reader.GetString(12),
                            isEmployed = reader.GetBoolean(13),
                            HireDate = reader.GetDateTime(14)
                        };

                        // Save Employees into List
                        employeesInDB.Add(emp);
                    }
                    // close the reader
                    reader.Close();
                }
            }
            catch (Exception ex)
            {

                throw new ApplicationException("There was a problem retrieving your data.", ex);
            }
            finally
            {
                // final housekeeping
                conn.Close();
            }

            return employeesInDB;
        }

        /// <summary>
        /// Updates Employee By ID
        /// </summary>
        /// <param name="employee">Employee DTO for update</param>
        /// <returns></returns>
        public static bool UpdateEmployeeByID( Employee employee )
        {
            int count = 0;

            var conn = DBConnection.GetConnection();
            var cmdText = @"sp_update_user";
            var cmd = new SqlCommand(cmdText, conn);

            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@UserID", employee.UserId);
            cmd.Parameters.AddWithValue("@FirstName", employee.FirstName);
            cmd.Parameters.AddWithValue("@LastName", employee.LastName);
            cmd.Parameters.AddWithValue("@OtherNames", employee.OtherNames);
            cmd.Parameters.AddWithValue("@PhoneNumber", employee.PhoneNumber);
            cmd.Parameters.AddWithValue("@Email", employee.Email);
            cmd.Parameters.AddWithValue("@ssnNo", employee.SSNo);
            cmd.Parameters.AddWithValue("@picUrl", employee.PicUrl);
            cmd.Parameters.AddWithValue("@isEmployed", employee.isEmployed);
            cmd.Parameters.AddWithValue("@isBlocked", employee.isBlocked);
            cmd.Parameters.AddWithValue("@UserRolesID", employee.UserRolesId);
            cmd.Parameters.AddWithValue("@ClearanceLevelID", employee.ClearanceLevelId);
            cmd.Parameters.AddWithValue("@Username", employee.Username);
            cmd.Parameters.AddWithValue("@HireDate", employee.HireDate); //set manually in BL temporarily
            //cmd.Parameters.AddWithValue("@PasswordHash", employee.PasswordHash);
            cmd.Parameters.AddWithValue("@Gender", employee.Gender);
            cmd.Parameters.AddWithValue("@BirthDate", employee.DateOfBirth.HasValue ? employee.DateOfBirth : (object)DBNull.Value);
            cmd.Parameters.AddWithValue("@CountryID", employee.CountryId.HasValue && employee.CountryId != -1 ? employee.CountryId : (object)DBNull.Value);
            cmd.Parameters.AddWithValue("@MaritalStatus", employee.MaritalStatus);
            cmd.Parameters.AddWithValue("@PersonalEmail", employee.PersonalEmail);
            cmd.Parameters.AddWithValue("@PersonalPhoneNumber", employee.PersonalPhoneNumber);
            cmd.Parameters.AddWithValue("@AdditionalInfo", employee.AdditonalInfo);

            try
            {
                conn.Open();
                count = (int)cmd.ExecuteNonQuery();
            }
            catch (Exception)
            {
                throw;
            }
            finally
            {
                conn.Close();
            }

            //Insert into Users Table

            //Returns the User ID
            bool result = count == 1 ? true : false;
            return result;
        }

        /// <summary>
        /// Deletes employee records
        /// </summary>
        /// <param name="userID"></param>
        /// <returns></returns>
        public static bool DeleteEmployeeByID ( int userID)
        {
            return false;
        }


        // Department CRUD
        /// <summary>
        /// Creates a new departments
        /// </summary>
        /// <param name="depertment"></param>
        /// <returns></returns>
        public static bool CreateDepartment ( Department depertment)
        {
            return false;
        }

        /// <summary>
        /// Retrieve Departments based on visibility
        /// </summary>
        /// <param name="isVisible"></param>
        /// <returns>List of Department Objects</returns>
        public List<Department> RetrieveDepartments(bool isVisible)
        {
            var departmentsInDB = new List<Department>();

            //Getting connection
            var conn = DBConnection.GetConnection();

            //Using stored procedure
            var cmdText = @"sp_retrieve_departments";
            var cmd = new SqlCommand(cmdText, conn);
            cmd.CommandType = CommandType.StoredProcedure;


            // add a parameter to the command
            int visibilityBit = isVisible ? 1 : 0;
            cmd.Parameters.Add("@Visibility", SqlDbType.Bit);
            cmd.Parameters["@Visibility"].Value = visibilityBit;

            try
            {
                // you have to open a connection before using it
                conn.Open();

                // create a data reader with our command
                var reader = cmd.ExecuteReader();

                // check to make sure the reader has data
                if (reader.HasRows)
                {
                    // process the data reader
                    while (reader.Read())
                    {
                        // create an employee object
                        var dept = new Department()
                        {
                            DepartmentId = reader.GetString(0),
                            Name = reader.GetString(1),
                            Description = reader.GetString(2),
                            Visibility = reader.GetBoolean(3)      
                        };

                        // Save Employees into List
                        departmentsInDB.Add(dept);
                    }
                    // close the reader
                    reader.Close();
                }
            }
            catch (Exception ex)
            {

                throw new ApplicationException("There was a problem retrieving your data.", ex);
            }
            finally
            {
                // final housekeeping
                conn.Close();
            }

            return departmentsInDB;
        }

        /// <summary>
        /// Updates Department Information
        /// </summary>
        /// <param name="departmentID"></param>
        /// <returns></returns>
        public static bool UpdateDepartment ( Department department )
        {
            return false;
        }
        
        /// <summary>
        /// Deletes Department
        /// </summary>
        /// <param name="departmentID"></param>
        /// <returns></returns>
        public static bool DeleteDepartment ( string departmentID)
        {
            return false;
        }


        // User Role CRUD
        /// <summary>
        /// Create User Roles
        /// </summary>
        /// <param name="userRoles"></param>
        /// <returns></returns>
        public static bool CreateUserRole ( UserRoles userRoles )
        {
            return false;
        }

        /// <summary>
        /// Retrieve employee roles by department.
        /// </summary>
        /// <param name="departmentId">Department Id employee for retrieval</param>
        /// <param name="isAll">Indicator to retrieve all roles irrespective departmentId</param>
        /// <returns>Returns a list of UserRole Objects</returns>
        public static List<UserRoles> RetrieveUserRoles(string departmentId, bool isAll)
        {
            var userRolesInDb = new List<UserRoles>();

            //Getting connection
            var conn = DBConnection.GetConnection();

            //Using stored procedure
            var cmdText = @"sp_retrieve_userroles";
            var cmd = new SqlCommand(cmdText, conn);
            cmd.CommandType = CommandType.StoredProcedure;


            // add a parameter to the command based on selection type
            cmd.Parameters.Add("@DepartmentId", SqlDbType.VarChar);
            string departmentIdParameter = isAll ? "dbsall" : departmentId;
            cmd.Parameters["@DepartmentId"].Value = departmentIdParameter;
            

            try
            {
                // you have to open a connection before using it
                conn.Open();

                // create a data reader with our command
                var reader = cmd.ExecuteReader();

                // check to make sure the reader has data
                if (reader.HasRows)
                {
                    // process the data reader
                    while (reader.Read())
                    {
                        // create an employee object
                        var roles = new UserRoles()
                        {
                            UserRolesId = reader.GetString(0),
                            Name = reader.GetString(1),
                            Description = reader.GetString(2),
                            DepartmentId = reader.GetString(3)
                        };

                        // Save Employees into List
                        userRolesInDb.Add(roles);
                    }
                    // close the reader
                    reader.Close();
                }
            }
            catch (Exception ex)
            {

                throw new ApplicationException("There was a problem retrieving employee roles data.", ex);
            }
            finally
            {
                // final housekeeping
                conn.Close();
            }

            return userRolesInDb;
            
        }

        /// <summary>
        /// Updates User Role Information
        /// </summary>
        /// <param name="userRolesID"></param>
        /// <returns></returns>
        public static bool UpdateUserRoles ( UserRoles userRoles )
        {
            return false;
        }

        /// <summary>
        /// Deletes User Roles.
        /// </summary>
        /// <param name="userRolesID"></param>
        /// <returns></returns>
        public static bool DeleteUserRoles ( int userRolesID )
        {
            return false;
        }


        // User Address CRUD
        /// <summary>
        /// Create new Address For Employee
        /// </summary>
        /// <param name="userID">Employee User ID</param>
        /// <param name="address">Employee Address</param>
        /// <returns></returns>
        public static int CreateAddressByUserID( int userID, Address address)
        {
            int count = 0;

            var conn = DBConnection.GetConnection();
            var cmdText = @"sp_create_new_user_address";
            var cmd = new SqlCommand(cmdText, conn);

            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@AddressLine1", address.AddressLines[0] );
            cmd.Parameters.AddWithValue( "@AddressLine2", address.AddressLines[1] );
            cmd.Parameters.AddWithValue( "@AddressLine3", address.AddressLines[2] );
            cmd.Parameters.AddWithValue("@City", address.City);
            cmd.Parameters.AddWithValue("@StateID", address.StateID);
            cmd.Parameters.AddWithValue("@Zip", address.Zip);
            cmd.Parameters.AddWithValue("@CountryID", address.CountryID);
            cmd.Parameters.AddWithValue("@AddressTypeID", address.AddressTypeId);
            cmd.Parameters.AddWithValue("@UserID", userID);

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
        /// Retrieves Addresses By Personal Information ID
        /// </summary>
        /// <param name="userID">Personal Information ID Parameter for Select Query</param>
        /// <returns>Returns a list of Addresses</returns>
        private static List<Address> retrieveAddressesByUserID(int userID) 
        {
            //Retrieve all details
            Address address = null;
            var addressList = new List<Address>();
            var addressLinesInDB = new List<String>();

            var conn = DBConnection.GetConnection();
            var cmdText = @"sp_retrieve_address_by_userID";
            var cmd = new SqlCommand(cmdText, conn);
            cmd.CommandType = CommandType.StoredProcedure;

            cmd.Parameters.Add("@userID", SqlDbType.Int);
            cmd.Parameters["@userID"].Value = userID;

            try
            {
                conn.Open();
                var reader = cmd.ExecuteReader();

                // check to make sure the reader has data
                if (reader.HasRows)
                {
                    // process the data reader
                    while (reader.Read())
                    {
                        address = new Address()
                        {
                           UserAddressId = reader.GetInt32(0),
                           City = reader.IsDBNull(4) ? null : reader.GetString(4),
                           StateID = reader.IsDBNull(5) ? -1 : reader.GetInt32(5),
                           Zip = reader.IsDBNull(8) ? null : reader.GetString(8),
                           CountryID = reader.IsDBNull(9) ? -1 : reader.GetInt32(9),
                           AddressTypeId = reader.IsDBNull(11) ? -1 : reader.GetInt32(11)                           
                        };

                        //Process AddressLines
                        addressLinesInDB = new List<String>();
                        addressLinesInDB.Add( reader.IsDBNull(2) ? null : reader.GetString(1) );
                        addressLinesInDB.Add( reader.IsDBNull(3) ? null : reader.GetString(2) );
                        addressLinesInDB.Add( reader.IsDBNull(4) ? null : reader.GetString(3) );
                        address.AddressLines = addressLinesInDB;

                        // Saving Address to list
                        addressList.Add(address);
                    }
                    // close the reader
                    reader.Close();
                }
            }
            catch (Exception e)
            {
                throw;
            }
            finally
            {
                conn.Close();
            }

            //Return Address list
            return addressList;
        }

        /// <summary>
        /// Update Address By User ID
        /// </summary>
        /// <param name="userID"></param>
        /// <param name="address"></param>
        /// <returns></returns>
        public static int UpdateAddressByUserID ( int userID, Address address )
        {
            int count = 0;

            var conn = DBConnection.GetConnection();
            var cmdText = @"sp_update_user_address";
            var cmd = new SqlCommand(cmdText, conn);

            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@UsersAddressID", address.UserAddressId);
            cmd.Parameters.AddWithValue("@AddressLine1", address.AddressLines[0]);
            cmd.Parameters.AddWithValue("@AddressLine2", address.AddressLines[1]);
            cmd.Parameters.AddWithValue("@AddressLine3", address.AddressLines[2]);
            cmd.Parameters.AddWithValue("@City", address.City);
            cmd.Parameters.AddWithValue("@StateID", address.StateID);
            cmd.Parameters.AddWithValue("@Zip", address.Zip);
            cmd.Parameters.AddWithValue("@CountryID", address.CountryID);
            cmd.Parameters.AddWithValue("@AddressTypeID", address.AddressTypeId);
            cmd.Parameters.AddWithValue("@UserID", userID);

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
        /// Deletes Address By User ID.
        /// </summary>
        /// <param name="userID"></param>
        /// <returns></returns>
        private static bool DeleteAddressByUserID ( int userID )
        {
            return false;
        }


        // Address Types CRUD
        /// <summary>
        /// Creates new Address Type
        /// </summary>
        /// <param name="addressType"></param>
        /// <returns></returns>
        public static bool CreateAddressType ( AddressType addressType )
        {
            return false;
        }

        /// <summary>
        /// Retrieve AddressTypes by AddressType ID
        /// </summary>
        /// <param name="addresstypeID"></param>
        /// <param name="retrieveAll"></param>
        /// <returns>Returns a list of AddressTypes</returns>
        public static List<AddressType> RetrieveAddressTypesByID(int addresstypeID, bool retrieveAll)
        {
            var addressTypeList = new List<AddressType>();

            //Getting connection
            var conn = DBConnection.GetConnection();

            //Using stored procedure
            var cmdText = @"sp_retrieve_addresstype_by_id";
            var cmd = new SqlCommand(cmdText, conn);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.Add("@AddressTypeID", SqlDbType.Int);
            int addressTypeIdParameter = retrieveAll ? -1 : addresstypeID;
            cmd.Parameters["@AddressTypeID"].Value = addressTypeIdParameter;

            try
            {
                // you have to open a connection before using it
                conn.Open();

                // create a data reader with our command
                var reader = cmd.ExecuteReader();

                // check to make sure the reader has data

                if (reader.HasRows)
                {
                    // process the data reader
                    while (reader.Read())
                    {
                        var addresstype = new AddressType()
                        {
                            AddressTypeId = reader.GetInt32(0),
                            Name = reader.GetString(1),
                            Description = reader.GetString(2)
                        };
                        addressTypeList.Add(addresstype);
                    }
                    // close the reader
                    reader.Close();
                }
            }
            catch (Exception ex)
            {

                throw new ApplicationException("There was a problem retrieving address type data.", ex);
            }
            finally
            {
                // final housekeeping
                conn.Close();
            }

            return addressTypeList;
        }

        /// <summary>
        /// Updates Address Types
        /// </summary>
        /// <param name="addressTypeID"></param>
        /// <returns></returns>
        public static bool UpdateAddressType ( AddressType addressType )
        {
            return false;
        }

        /// <summary>
        /// Deletes Address Type
        /// </summary>
        /// <param name="addressTypeID"></param>
        /// <returns></returns>
        public static bool DeleteAddresstype ( int addressTypeID ) //very dangerous message
        {
            return false;
        }


        // Clearance Level CRUD
        /// <summary>
        /// Creates new clearance level
        /// </summary>
        /// <param name="clearanceLevel">Clerance Level DTO</param>
        /// <returns></returns>
        public static bool CreateClearanceLevel ( ClearanceLevel clearanceLevel )
        {
            return false;
        }

        /// <summary>
        /// Retreive Clearance Levels from Database based on Department
        /// </summary>
        /// <param name="departmentId">Department ID</param>
        /// <param name="retrieveAll">Signal to Retrieve all irresepective of Department ID</param>
        /// <returns>Returns a List of Clearance Levels</returns>
        public static List<ClearanceLevel> RetrieveClearanceByDeptID(string departmentId, bool retrieveAll)
        {
            var clearanceLevelInDB = new List<ClearanceLevel>();

            //Getting connection
            var conn = DBConnection.GetConnection();

            //Using stored procedure
            var cmdText = @"sp_retrieve_clearance_levels_by_deptId";
            var cmd = new SqlCommand(cmdText, conn);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.Add("@DepartmentId", SqlDbType.VarChar);
            string departmentIdParameter = retrieveAll ? "dbsall" : departmentId;
            cmd.Parameters["@DepartmentId"].Value = departmentIdParameter;

            try
            {
                // you have to open a connection before using it
                conn.Open();

                // create a data reader with our command
                var reader = cmd.ExecuteReader();

                // check to make sure the reader has data
                if (reader.HasRows)
                {
                    // process the data reader
                    while (reader.Read())
                    {
                        // create an clearancelevel object
                        var cll = new ClearanceLevel()
                        {
                            ClearanceLevelId = reader.GetInt32(0),
                            Name = reader.GetString(1),
                            Description = reader.GetString(2),
                            DepartmentId = reader.GetString(3),

                        };

                        // Save Employees into List
                        clearanceLevelInDB.Add(cll);
                    }
                    // close the reader
                    reader.Close();
                }
            }
            catch (Exception ex)
            {

                throw new ApplicationException("There was a problem retrieving clearance level data.", ex);
            }
            finally
            {
                // final housekeeping
                conn.Close();
            }

            return clearanceLevelInDB;
        }

        /// <summary>
        /// Updates Clearance Level By Clearancelevel ID
        /// </summary>
        /// <param name="clearanceLevelID">Clearance Level ID</param>
        /// <returns></returns>
        public static bool UpdateClearanceLevel ( ClearanceLevel clearanceLevel ) //potentially dangerous method
        {
            return false;
        }

        /// <summary>
        /// Deletes Clearance Level by ID
        /// </summary>
        /// <param name="clearanceLevelID">Clerance Level ID</param>
        /// <returns></returns>
        public static bool DeleteClearanceLevel ( int clearanceLevelID )
        {
            return false;
        }




    }
}
