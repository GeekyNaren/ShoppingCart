using System.Security.Cryptography;
using System.Text;

namespace ShoppingCart.Services.Helper
{
    public static class PasswordHelper
    {
        public static bool VerifyPassword(string storedBase64, string password)
        {
            try
            {
                var fullBytes = Convert.FromBase64String(storedBase64);
                // salt is first 16 bytes, hash is remaining
                var salt = new byte[16];
                Buffer.BlockCopy(fullBytes, 0, salt, 0, salt.Length);
                var storedHash = new byte[fullBytes.Length - salt.Length];
                Buffer.BlockCopy(fullBytes, salt.Length, storedHash, 0, storedHash.Length);

                var computedHash = Rfc2898DeriveBytes.Pbkdf2(Encoding.UTF8.GetBytes(password), salt, 100_000, HashAlgorithmName.SHA256, storedHash.Length);

                return CryptographicOperations.FixedTimeEquals(storedHash, computedHash);
            }
            catch
            {
                return false;
            }
        }
    }
}
