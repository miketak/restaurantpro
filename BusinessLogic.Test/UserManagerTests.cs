using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using BusinessLogic;
using DataObjects;
using System.Collections.Generic;
using System.Diagnostics;

namespace BusinessLogic.Test
{
    [TestClass]
    public class UserManagerTests
    {

        [TestMethod]
        public void TryAuthentication()
        {
            var umg = new UserManager();

            var userfromDB = umg.AuthenticateUser("SCSYMCX", "password2");
            Assert.AreEqual(userfromDB.FirstName, "Martin");
            Assert.AreEqual(userfromDB.LastName, "Cox");

        }


        [TestMethod]
        public void CreateEmployeeTestWithAddress()
        {
            var employee = new Employee();

            employee.FirstName = "Ryan";
            employee.LastName = "Paul";
            employee.OtherNames = "Worlanyo";
            employee.PersonalPhoneNumber = "233245042433";
            employee.PersonalEmail = "mtaks@gmail.com";

            employee.CountryId = 45;

            employee.MaritalStatus = false;
            employee.Gender = true;
            employee.DateOfBirth = DateTime.Now;
            employee.Username = "SCSYCMQ";
            //employee.PasswordHash = "SCSYCCM";
            employee.PhoneNumber = "2332222";
            employee.Email = "taks@ksman.com";
            //employee.HireDate = DateTime.Now;
            employee.SSNo = ""; //needs to be added in presentation
            employee.PicUrl = ""; //needs to be added in presentation


            employee.UserRolesId = "DESENG";
            employee.ClearanceLevelId = 203;
            employee.isEmployed = true;
            employee.AdditonalInfo = "Additional Info";


            //Address
            Address ad = new Address();

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

            List<Address> adList = new List<Address>();
            adList.Add(ad);

            employee.Address = adList;

            var emp = new EmployeeManager();
            bool result = emp.CreateEmployee(employee);
           

            int actual;

            if (result == true)
            {
                actual = 1;
            }
            else
            {
                actual = 0;
            }


            int expected = 1;

            Assert.AreEqual(expected, actual);
        }

        [TestMethod]
        public void CreateEmployeeTestNoAddress()
        {
            var employee = new Employee();

            employee.FirstName = "John";
            employee.LastName = "Tei";
            employee.OtherNames = "Worlanyo";
            employee.PersonalPhoneNumber = "233245042433";
            employee.PersonalEmail = "mtaks@gmail.com";

            employee.CountryId = 45;

            employee.MaritalStatus = false;
            employee.Gender = true;
            employee.DateOfBirth = DateTime.Now;
            employee.Username = "SCSYCMN";
            //employee.PasswordHash = "SCSYCCM";
            employee.PhoneNumber = "2332222";
            employee.Email = "taks@ksman.com";
            //employee.HireDate = DateTime.Now;
            employee.SSNo = ""; //needs to be added in presentation
            employee.PicUrl = ""; //needs to be added in presentation


            employee.UserRolesId = "DESENG";
            employee.ClearanceLevelId = 204;
            employee.isEmployed = true;
            employee.AdditonalInfo = "Additional Info";

            var emp = new EmployeeManager();
            bool result = emp.CreateEmployee(employee);


            int actual;

            if (result == true)
            {
                actual = 1;
            }
            else
            {
                actual = 0;
            }


            int expected = 1;

            Assert.AreEqual(expected, actual);
        }

        [TestMethod]
        public void RetrieveUserAccessTest()
        {
            Trace.WriteLine("Retrieve User Starting ***");

            //Parameters 
            var user = new User();
            user.UserRolesId = "DESENG";
            user.ClearanceLevelId = 204;

            UserManager um = new UserManager();
            var userAccess = um.RetrieveUserAccess(user);

            Trace.WriteLine(userAccess[0].FeatureName);

            string expected = "Time Entry App";
            string result = userAccess[0].FeatureName;
            Assert.AreEqual(expected, result);

        }
    }
}
