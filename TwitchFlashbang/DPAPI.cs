using System.Security.Cryptography;
using System.Text;

namespace TwitchFlashbang
{
    public static class DPAPI
    {
        public static string ProtectData(string data)
        {
            byte[] unprotectedData = Encoding.UTF8.GetBytes(data);
            byte[] protectedData = ProtectedData.Protect(unprotectedData, null, DataProtectionScope.CurrentUser);
            return Convert.ToBase64String(protectedData);
        }

        public static string UnprotectData(string protectedData)
        {
            byte[] encryptedData = Convert.FromBase64String(protectedData);
            byte[] decryptedData = ProtectedData.Unprotect(encryptedData, null, DataProtectionScope.CurrentUser);
            return Encoding.UTF8.GetString(decryptedData);
        }
    }
}
