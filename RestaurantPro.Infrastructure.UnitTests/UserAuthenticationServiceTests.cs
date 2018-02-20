using System;
using System.Security;
using System.Threading.Tasks;
using NUnit.Framework;
using RestaurantPro.Core.Services;
using RestaurantPro.Infrastructure.Repositories;
using RestaurantPro.Infrastructure.Services;

namespace RestaurantPro.Infrastructure.UnitTests
{
    [TestFixture]
    public class UserAuthenticationServiceTests
    {
        private readonly IUserAuthenticationService _userAuthenticationService;

        public UserAuthenticationServiceTests()
        {
            _userAuthenticationService = new UserAuthenticationService(new UserRepository(new RestProContext()));
        }

        [Test]
        public async Task AuthenticateUser_WithCorrectUsernameAndPassword_ReturnsUser()
        {
            string username = "rkpadi";
            string password = "password";

            var user = await _userAuthenticationService.AuthenticateUser(username, password.ToSecureString());

            Assert.That(user.Username, Is.EqualTo(username));
        }

        [Test]
        public void AuthenticateUser_WithCorrectUsernameAndWrongPassword_ThrowsApplicationException()
        {
            Assert.That(() => _userAuthenticationService.AuthenticateUser("rkpadi",
                "pass".ToSecureString()), Throws.Exception.TypeOf<ApplicationException>());
        }

        [Test]
        public void AuthenticateUser_WithWrongUsernameAndCorrectPassword_ThrowsApplicationException()
        {
            Assert.That(() => _userAuthenticationService.AuthenticateUser("rkgoat",
                "password".ToSecureString()), Throws.Exception.TypeOf<ApplicationException>());
        }

        [Test]
        public void AuthenticateUser_WithWrongUsernameAndWrongPassword_ThrowsApplicationException()
        {
            Assert.That(() => _userAuthenticationService.AuthenticateUser("zigi",
                "pass".ToSecureString()), Throws.Exception.TypeOf<ApplicationException>());
        }  
    }
}