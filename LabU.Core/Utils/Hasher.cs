using System.Security.Cryptography;
using System.Text;

namespace LabU.Core.Utils
{
    public class Hasher
    {
        public static string HashPassword(string pswd)
        {
            byte[] data = Encoding.Default.GetBytes(pswd);
            var hash = SHA1.HashData(data);
            return Convert.ToBase64String(hash);
        }
    }
}
