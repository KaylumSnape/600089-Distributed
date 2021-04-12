using System;
using System.Security.Cryptography;
using System.Text;

namespace DistSysAcw.Cryptography
{
    #region TASK9
    // Decoupled functionality.
    // https://stackoverflow.com/questions/17292366/hashing-with-sha1-algorithm-in-c-sharp
    public static class ShaCryptography
    {
        public static string Sha1Encrypt(string message)
        {
            using var sha1 = new SHA1CryptoServiceProvider();
            return (BitConverter.ToString((
                    sha1.ComputeHash(Encoding.ASCII.GetBytes(message))))
                .Replace("-", ""));
        }

        public static string Sha256Encrypt(string message)
        {
            using var sha256 = new SHA256CryptoServiceProvider();
            return (BitConverter.ToString((
                    sha256.ComputeHash(Encoding.ASCII.GetBytes(message))))
                .Replace("-", ""));
        }
    }
    #endregion
}
