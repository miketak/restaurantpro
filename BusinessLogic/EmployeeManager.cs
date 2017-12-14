using DataAccessLayer;
using DataObjects;
using DataObjects.ProgramDataObjects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLogic
{

    /// <summary>
    /// Manages Employee Related Activities
    /// </summary>
    public class EmployeeManager
    {
        // Employee CRUD

        /// <summary>
        /// Create new Employee
        /// </summary>
        /// <param name="employee">Employee Object</param>
        /// <returns>Return Confirmation Result</returns>
        public bool CreateEmployee(Employee employee)
        {
            bool result = false;

            //Set Password
            var userEncode = new UserManager();
           // employee.PasswordHash = userEncode.HashSHA256(employee.Username.ToUpper());

            //Set Hire Date - control must be created in presentation layer
            employee.HireDate = DateTime.Now;

            //Handle Empty SSN Issue
            employee.SSNo = employee.SSNo != null ? employee.SSNo : String.Empty;

            //Handle Empty Pic Url issue
            employee.PicUrl = employee.PicUrl != null ? employee.PicUrl : String.Empty;

            //User isBlocked :: False
            employee.isBlocked = false;

            //Write Employee and Get new UserId
            employee.UserId = EmployeeAccessor.CreateEmployee(employee);

            // Write Addresses if not null
            int count = 0;
            if (employee.Address != null)
            {
                foreach (var addressElement in employee.Address)
                {
                    count += EmployeeAccessor.CreateAddressByUserID(employee.UserId, addressElement);
                }

                if ( employee.UserId != 0 && count == employee.Address.Count )
                    result = true;

                return result;
            }
            
            // Check for write succcess if address is null
            if (employee.UserId != 0 )
                result = true;

            return result;

        }

        /// <summary>
        /// Retrieves Employee from database using Employee Id or User Id
        /// </summary>
        /// <param name="username">Paramter used to extract Employee Details</param>
        /// <returns>Employee Object Containing All Details</returns>
        public Employee RetrieveEmployeeByUsername(string username)
        {
            var employee = new Employee();

            //Retrieve all employee information
            try
            {
                var allUserDataInDB = EmployeeAccessor.RetrieveEmployeeByUsername(username);
                employee.UserId = allUserDataInDB.UserId;
                employee.Username = allUserDataInDB.Username;
                employee.FirstName = allUserDataInDB.FirstName;
                employee.LastName = allUserDataInDB.LastName;
                employee.OtherNames = allUserDataInDB.OtherNames;
                employee.DepartmentId = allUserDataInDB.DepartmentId;
                employee.Department = allUserDataInDB.Department;
                employee.PhoneNumber = allUserDataInDB.PhoneNumber;
                employee.Email = allUserDataInDB.Email;
                employee.PicUrl = allUserDataInDB.PicUrl;
                employee.isEmployed = allUserDataInDB.isEmployed;
                employee.isBlocked = allUserDataInDB.isBlocked;
                employee.UserRolesId = allUserDataInDB.UserRolesId;
                employee.JobDesignation = allUserDataInDB.JobDesignation;
                employee.ClearanceLevelId = allUserDataInDB.ClearanceLevelId;
                employee.ClearanceLevel = allUserDataInDB.ClearanceLevel;
                //employee.PersonalInfoId = allUserDataInDB.PersonalInfoId;
                employee.Gender = allUserDataInDB.Gender;
                employee.DateOfBirth = allUserDataInDB.DateOfBirth;
                employee.CountryId = allUserDataInDB.CountryId;
                employee.Nationality = allUserDataInDB.Nationality;
                employee.AdditonalInfo = allUserDataInDB.AdditonalInfo;
                employee.Address = allUserDataInDB.Address; //this is a list
                employee.HireDate = allUserDataInDB.HireDate;
                employee.MaritalStatus = allUserDataInDB.MaritalStatus;
                employee.PersonalEmail = allUserDataInDB.PersonalEmail;
                employee.PersonalPhoneNumber = allUserDataInDB.PersonalPhoneNumber;

            }
            catch (Exception)
            {

                throw;
            }

            return employee;
        }

        /// <summary>
        /// Retrieves all Active Employees From Database
        /// </summary>
        /// <param name="isEmployed">True or False boolean paramter to retrieveCountry active or inactive employees respecetively</param>
        /// <returns>A list of active or inactive Employee objects from Database.</returns>
        public List<Employee> RetrieveEmployees(bool isEmployed)
        {
            var employeeList = new List<Employee>();

            employeeList = EmployeeAccessor.RetrieveEmployees(isEmployed);

            return employeeList;
        }

        /// <summary>
        /// Updates Employee information
        /// </summary>
        /// <param name="employee"></param>
        /// <returns></returns>
        public bool UpdateEmployeeByID(Employee employee)
        {
            bool result = false;

            //Set Hire Date - control must be created in presentation layer
            employee.HireDate = DateTime.Now;

            //Handle Empty SSN Issue
            employee.SSNo = employee.SSNo != null ? employee.SSNo : String.Empty;

            //Handle Empty Pic Url issue
            employee.PicUrl = employee.PicUrl != null ? employee.PicUrl : String.Empty;

            //User isBlocked :: False
            employee.isBlocked = false;

            //Update Employee Details
            result = EmployeeAccessor.UpdateEmployeeByID(employee);

            //Update Employee Address
            int count = 0;
            try
            {
                if (employee.Address != null)
                {
                    foreach (var addressElement in employee.Address)
                    {
                        if (addressElement.UserAddressId != 0) // Update Employee Address
                        {
                            count += EmployeeAccessor.UpdateAddressByUserID(employee.UserId, addressElement);
                        }
                        else // Create New Address
                        {
                            count += EmployeeAccessor.CreateAddressByUserID(employee.UserId, addressElement);
                        }

                    }

                    //Check if write was successful
                    if (employee.UserId != 0 && count == employee.Address.Count)
                    {
                        result = true;
                    }

                    else
                    {
                        result = false;
                    }
                }
            }
            catch (Exception)
            {                
                throw;
            }
            
            //Perform update for addresses
            return result;
        }

        /// <summary>
        /// Delete Employee By ID
        /// </summary>
        /// <param name="userID"></param>
        /// <returns>Returns Confirmation</returns>
        public bool DeleteEmployeeByID(int userID)
        {
            bool result = false;

            result = EmployeeAccessor.DeleteEmployeeByID(userID);

            return result;
        }


        // Department CRUD

        /// <summary>
        /// Creates new Department
        /// </summary>
        /// <param name="department"></param>
        /// <returns></returns>
        public bool CreateDepartment(Department department)
        {
            bool result = false;

            result = EmployeeAccessor.CreateDepartment(department);

            return result;
        }

        /// <summary>
        /// Retrieves Department by visibility
        /// </summary>
        /// <param name="userID">Visibility indicator</param>
        /// <returns>Returns a list of departments</returns>
        public List<Department> RetrieveDepartmentsByVisibility(bool retrieveAll)
        {
            var departments = new List<Department>();

            var departmentAccess = new EmployeeAccessor();
            departments = departmentAccess.RetrieveDepartments(retrieveAll);


            return departments;
        }

        /// <summary>
        /// Updates Department Information
        /// </summary>
        /// <param name="department"></param>
        /// <returns></returns>
        public bool UpdateDepartment(Department department)
        {
            bool result = false;

            result = EmployeeAccessor.UpdateDepartment(department);

            return result;
        }

        /// <summary>
        /// Deletes Departments
        /// </summary>
        /// <param name="departmentID"></param>
        /// <returns></returns>
        public bool DeleteDepartment(string departmentID) //potentially dangerous
        {
            bool result = false;

            result = EmployeeAccessor.DeleteDepartment(departmentID);

            return result;
        }


        //UserRole CRUD

        /// <summary>
        /// Creates new User Roles
        /// </summary>
        /// <param name="userRole"></param>
        /// <returns>Returns confirmation</returns>
        public bool CreateUserRole(UserRoles userRole)
        {
            bool result = false;

            result = EmployeeAccessor.CreateUserRole(userRole);

            return result;
        }

        /// <summary>
        /// Retrieves Job Positions based on DepartmentID
        /// </summary>
        /// <param name="departmentId">Retrieves ID</param>
        /// <param name="isRetrieveAll">Paramter to indicate if all should be retrieved</param>
        /// <returns>List of User Roles</returns>
        public List<UserRoles> RetrieveUserRolesByDeptID(string departmentId, bool isRetrieveAll)
        {
            var jobPositions = new List<UserRoles>();

            jobPositions = EmployeeAccessor.RetrieveUserRoles(departmentId, isRetrieveAll);

            return jobPositions;
        }

        /// <summary>
        /// Updates User Roles
        /// </summary>
        /// <param name="userRoles"></param>
        /// <returns>Returns a boolean</returns>
        public bool UpdateUserRoles(UserRoles userRoles)
        {
            bool result = false;

            result = EmployeeAccessor.UpdateUserRoles(userRoles);

            return result;
        }

        /// <summary>
        /// Delete User Role By ID
        /// </summary>
        /// <param name="userRolesID"></param>
        /// <returns></returns>
        public bool DeleteUserRoleByID(int userRolesID)
        {
            bool result = false;

            result = EmployeeAccessor.DeleteUserRoles(userRolesID);

            return result;
        }



        //Clearance Level CRUD

        /// <summary>
        /// Create clearance levels
        /// </summary>
        /// <param name="clearanceLevel"></param>
        /// <returns></returns>
        public bool CreateClearanceLevel(ClearanceLevel clearanceLevel)
        {
            bool result = false;

            result = EmployeeAccessor.CreateClearanceLevel(clearanceLevel);

            return result;
        }

        /// <summary>
        /// Retrieves Employee Clearance Data
        /// </summary>
        /// <param name="departmentId"></param>
        /// <param name="retrieveAll"></param>
        /// <returns></returns>
        public List<ClearanceLevel> RetrieveClearanceByDeptID(string departmentId, bool retrieveAll)
        {
            var clearanceLevels = EmployeeAccessor.RetrieveClearanceByDeptID(departmentId, retrieveAll);

            return clearanceLevels;
        }

        /// <summary>
        /// Update Clearance levels
        /// </summary>
        /// <param name="clearanceLevel"></param>
        /// <returns></returns>
        public bool UpdateClearanceLevel(ClearanceLevel clearanceLevel)
        {
            bool result = false;

            result = EmployeeAccessor.UpdateClearanceLevel(clearanceLevel);

            return result;

        }

        /// <summary>
        /// Deletes Clearance Level
        /// </summary>
        /// <param name="clearanceLevelId">Clearance Level ID</param>
        /// <returns></returns>
        public bool DeleteCleranceLevel(int clearanceLevelId)
        {
            bool result = false;

            result = EmployeeAccessor.DeleteClearanceLevel(clearanceLevelId);

            return result;
        }



        //AddressTypes CRUD

        /// <summary>
        /// Creates new Address Type
        /// </summary>
        /// <param name="addressType">Address Type DTO</param>
        /// <returns>Returns success boolean</returns>
        public bool CreateAddressType(AddressType addressType)
        {
            bool result = false;

            result = EmployeeAccessor.CreateAddressType(addressType);

            return result;
        }

        /// <summary>
        /// Retrieves AddressTypes by ID
        /// </summary>
        /// <param name="addresstypeID">Address Type Integer ID</param>
        /// <param name="retrieveAll">Retrieve all signal</param>
        /// <returns>A list of Address Types</returns>
        public List<AddressType> RetrieveAddressTypeByID(int addresstypeID, bool retrieveAll)
        {
            List<AddressType> addressTypesList = new List<AddressType>();

            addressTypesList = EmployeeAccessor.RetrieveAddressTypesByID(addresstypeID, retrieveAll);

            return addressTypesList;
        }

        /// <summary>
        /// Updates Address Type Information
        /// </summary>
        /// <param name="addressType"></param>
        /// <returns></returns>
        public bool UpdateAddressType(AddressType addressType)
        {
            bool result = false;

            result = EmployeeAccessor.UpdateAddressType(addressType);

            return result;
        }

        /// <summary>
        /// Delete AddressType
        /// </summary>
        /// <param name="addressTypeID"></param>
        /// <returns></returns>
        public bool DeleteAddressType(int addressTypeID)
        {
            bool result = false;

            result = EmployeeAccessor.DeleteAddresstype(addressTypeID);

            return result;
        }

    }
}
