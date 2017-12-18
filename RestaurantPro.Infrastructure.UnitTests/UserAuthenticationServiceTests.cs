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
        private IUserAuthenticationService _userAuthenticationService;

        public UserAuthenticationServiceTests()
        {
            _userAuthenticationService = new UserAuthenticationService();
        }

        [TestMethod]
        public void AuthenticateUserWithKnownName()
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