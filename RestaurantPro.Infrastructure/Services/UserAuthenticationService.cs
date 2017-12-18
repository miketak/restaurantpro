using System;
using System.Data.Entity;
using System.Linq;
using System.Runtime.InteropServices;
using System.Security;
using RestaurantPro.Core.Domain;
using RestaurantPro.Core.Repositories;
using RestaurantPro.Core.Services;
using RestaurantPro.Infrastructure.Repositories;

namespace RestaurantPro.Infrastructure.Services
{
    public class UserAuthenticationService : IUserAuthenticationService
    {
        private IUserRepository _userRepository;
        private RestProContext _context;

        public UserAuthenticationService()//IUserRepository userRepository)
        {
            _context = new RestProContext();
            _userRepository = new UserRepository(_context);
        }

        public User AuthenticateUser(string username, SecureString password)
        {
            User user = null;

            //Username & Password Pre-validation
            if (username.Length < 2 || username.Length > 20)
                throw new ApplicationException("Invalid Username");

            if (password.Length < 3) // to future mike - use some regex complexity
                throw new ApplicationException("Invalid Password");

            if (VerifyUsernameAndPassword(username, password))
            {
                password = null;
                user = RetrieveUserByUsername(username);
            }
            else
            {
                throw new ApplicationException("Authentication Failed!");
            }

            return user;
        }

        private bool VerifyUsernameAndPassword(string username, SecureString password)
        {
            //might change implementation to stored procedure implementation
            var user = _userRepository.SingleOrDefault(u => u.Username == username);

            if (user == null)
                return false;

            //will need to be changed future Michael
            var passwordInDb = new SecureString();
            foreach (char c in user.Password)
            {
                passwordInDb.AppendChar(c);
            }

            return IsPasswordSame(password, passwordInDb);
        }

        /// <summary>
        /// Checks for password equality
        /// </summary>
        /// <param name="ss1"></param>
        /// <param name="ss2"></param>
        /// <returns></returns>
        public bool IsPasswordSame(SecureString ss1, SecureString ss2)
        {
            IntPtr bstr1 = IntPtr.Zero;
            IntPtr bstr2 = IntPtr.Zero;
            try
            {
                bstr1 = Marshal.SecureStringToBSTR(ss1);
                bstr2 = Marshal.SecureStringToBSTR(ss2);
                int length1 = Marshal.ReadInt32(bstr1, -4);
                int length2 = Marshal.ReadInt32(bstr2, -4);
                if (length1 == length2)
                {
                    for (int x = 0; x < length1; ++x)
                    {
                        byte b1 = Marshal.ReadByte(bstr1, x);
                        byte b2 = Marshal.ReadByte(bstr2, x);
                        if (b1 != b2) return false;
                    }
                }
                else return false;
                return true;
            }
            finally
            {
                if (bstr2 != IntPtr.Zero) Marshal.ZeroFreeBSTR(bstr2);
                if (bstr1 != IntPtr.Zero) Marshal.ZeroFreeBSTR(bstr1);
            }
        }

        /// <summary>
        /// Retrieves User by Username
        /// </summary>
        /// <param name="username"></param>
        /// <returns></returns>
        public User RetrieveUserByUsername(string username)
        {
            return _userRepository.SingleOrDefault(u => u.Username == username);
        }
    }
}