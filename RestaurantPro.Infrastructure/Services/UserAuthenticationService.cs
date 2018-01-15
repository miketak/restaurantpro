using System;
using System.Diagnostics;
using System.Linq;
using System.Runtime.InteropServices;
using System.Security;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using RestaurantPro.Core.Domain;
using RestaurantPro.Core.Repositories;
using RestaurantPro.Core.Services;
using RestaurantPro.Infrastructure.Repositories;

namespace RestaurantPro.Infrastructure.Services
{
    public class UserAuthenticationService : IUserAuthenticationService
    {
        private readonly IUserRepository _userRepository;

        public UserAuthenticationService(IUserRepository userRepository)
        {
            _userRepository = userRepository;
        }

        public UserAuthenticationService() //temporary constructor
        {
            _userRepository = new UserRepository(new RestProContext());
        }

        public async Task<User> AuthenticateUser(string username, SecureString password)
        {
            User user = null;
            PrevalidateLoginCredentials(username, password);

            try
            {
                var isPassValidation = await VerifyUsernameAndPassword(username, password);

                if (isPassValidation)
                    user = await RetrieveUserByUsername(username);
                else
                    throw new ApplicationException("Authentication Failed!");
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw new ApplicationException(e.Message);
            }

            return user;
        }

        private async Task<bool> VerifyUsernameAndPassword(string username, SecureString password)
        {
            User user = null;
            try
            {
                user = await RetrieveUserByUsername(username);
            }
            catch (Exception e)
            {
                Debug.WriteLine(e);
                throw new ApplicationException(e.Message);
            }

            if (user == null)
                return false;

            return ConfirmPassword(SecureStringToString(password), user.PasswordHash, user.SaltHash);
        }

        private Task<User> RetrieveUserByUsername(string username)
        {
            var user = _userRepository.SingleOrDefaultAsync(u => u.Username == username);

            return user;
        }

        private void PrevalidateLoginCredentials(string username, SecureString password)
        {
            if (username == null) throw new ArgumentNullException("username");
            if (password == null) throw new ArgumentNullException("password");

            if (username.Length < 2 || username.Length > 30)
                throw new ApplicationException("Invalid Username");

            if (password.Length < 3) // to future mike - use some regex complexity
                throw new ApplicationException("Invalid Password");
        }

        #region Hash and Security Methods

        private byte[] Hash(string value, byte[] salt)
        {
            return Hash(Encoding.UTF8.GetBytes(value), salt);
        }

        private byte[] Hash(byte[] value, byte[] salt)
        {
            byte[] saltedValue = value.Concat(salt).ToArray();

            return new SHA256Managed().ComputeHash(saltedValue);
        }

        private bool ConfirmPassword(string password, byte[] in_passwordHash, byte[] in_passwordSalt)
        {
            byte[] passwordHash = Hash(password, in_passwordSalt);

            return in_passwordHash.SequenceEqual(passwordHash);
        }

        private string SecureStringToString(SecureString value)
        {
            IntPtr valuePtr = IntPtr.Zero;
            try
            {
                valuePtr = Marshal.SecureStringToGlobalAllocUnicode(value);
                return Marshal.PtrToStringUni(valuePtr);
            }
            finally
            {
                Marshal.ZeroFreeGlobalAllocUnicode(valuePtr);
            }
        }

        #endregion
         
    }
}