using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using DataObjects;
using System.Collections.Generic;
using System.Diagnostics;

namespace DataAccessLayer.Test
{
    [TestClass]
    public class EmployeeAccessorTests
    {

        [TestMethod]
        public void createEmployeeTest()
        {
            Trace.WriteLine("Create Employee Testing ***");

            var employee = new Employee();

            employee.FirstName = "Michael";
            employee.LastName = "Takrama";
            employee.OtherNames = "Worlanyo";
            employee.PersonalPhoneNumber = "233245042433";
            employee.PersonalEmail = "mtaks@gmail.com";

            employee.CountryId = 45;

            employee.MaritalStatus = false;
            employee.Gender = true;
            employee.DateOfBirth = DateTime.Now;
            employee.Username = "SCSYCMR";
            employee.PasswordHash = "SCSYCCM";
            employee.PhoneNumber = "2332222";
            employee.Email = "taks@ksman.com";
            employee.HireDate = DateTime.Now;
            employee.SSNo = "234"; //needs to be added in presentation
            employee.PicUrl = "234"; //needs to be added in presentation

            //employee.DepartmentId = "EGDEPT"; //not used in user table
            employee.UserRolesId = "DESENG";
            employee.ClearanceLevelId = 204;
            employee.isEmployed = true;
            employee.AdditonalInfo = "Additional Info";

            int userID = EmployeeAccessor.CreateEmployee(employee);
            Trace.WriteLine("New User ID from test = " + userID);

            int actual;

            if ( userID > 1000)
            {
                actual = 1 ;
            }
            else
            {
                actual = 0;
            }


            int expected = 1;

            Assert.AreEqual(expected, actual);


        }

        [TestMethod]
        public void CreateAddressByUserID()
        {
            Address ad = new Address();
            int userId = 10003;

            List<string> addresslines = new List<string>();
            addresslines.Add("St Louis St");
            addresslines.Add("Block C");
            addresslines.Add("Apt J");

            ad.AddressLines = addresslines;
            ad.City = "Dubuque";
            ad.StateID = 45;
            ad.Zip = "52334";
            ad.CountryID = 45;
            ad.AddressTypeId = 3;

            int a = EmployeeAccessor.CreateAddressByUserID(userId, ad);

            Assert.AreEqual(1, a);

        }
    }
}
