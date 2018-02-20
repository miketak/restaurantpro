using System.Security;

namespace RestaurantPro.Infrastructure.UnitTests
{
    public static class ExtensionMethods
    {
        public static SecureString ToSecureString(this string str)
        {
            return ConvertToSecureString(str);
        }

        private static SecureString ConvertToSecureString(string rawPassword)
        {
            var encodedPassword = new SecureString();

            foreach (char c in rawPassword)
                encodedPassword.AppendChar(c);

            return encodedPassword;
        }
    }
}