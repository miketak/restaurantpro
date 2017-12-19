using System;
using System.Security;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using RestaurantPro.Core.Services;
using RestaurantPro.Infrastructure.Repositories;
using RestaurantPro.Infrastructure.Services;

namespace RestaurantPro.Infrastructure.UnitTests
{
    [TestClass]
    public class UserAuthenticationServiceTests
    {
        private readonly IUserAuthenticationService _userAuthenticationService;
        private const string FailedAuthenticationMessage = "Authentication Failed!";

        public UserAuthenticationServiceTests()
        {
            _userAuthenticationService = new UserAuthenticationService();
        }

        [TestMethod]
        public void AuthenticateUserWithCorrectNameCorrectPassword()
        {
            //Arrange
            string expectedUsername = "rkpadi";
            string password = "password";

            //Act
            var user = _userAuthenticationService.AuthenticateUser(expectedUsername, 
                ConvertToSecureString(password));

            //Assert
            Assert.AreEqual(expectedUsername, user.Username);
        }

        [TestMethod]
        [ExpectedException(typeof(ApplicationException), FailedAuthenticationMessage)]
        public void AuthenticateUserWithCorrectNameWrongPassword()
        {
            //Arrange
            string expectedUsername = "rkpadi";
            string password = "pass";

            //Act
            var user = _userAuthenticationService.AuthenticateUser(expectedUsername,
                ConvertToSecureString(password));
        }

        [TestMethod]
        [ExpectedException(typeof(ApplicationException), FailedAuthenticationMessage)]
        public void AuthenticateUserWithWrongNameCorrectPassword()
        {
            //Arrange
            string expectedUsername = "rkgoat";
            string password = "password";

            //Act
            var user = _userAuthenticationService.AuthenticateUser(expectedUsername,
                ConvertToSecureString(password));
        }

        [TestMethod]
        [ExpectedException(typeof(ApplicationException), FailedAuthenticationMessage)]
        public void AuthenticateUserWithWrongNameWrongPassword()
        {
            //Arrange
            string expectedUsername = "zigi";
            string password = "pass";

            //Act
            var user = _userAuthenticationService.AuthenticateUser(expectedUsername,
                ConvertToSecureString(password));
        }

        [Ignore]
        private SecureString ConvertToSecureString(string rawPassword)
        {
            //shall leave soon
            var encodedPassword = new SecureString();

            foreach (char c in rawPassword)
                encodedPassword.AppendChar(c);

            return encodedPassword;
        }
        
    }
}