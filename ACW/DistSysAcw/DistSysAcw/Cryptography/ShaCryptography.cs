using System;
using System.Security.Cryptography;
using System.Text;

namespace DistSysAcw.Cryptography
{
    #region TASK9
    // Decoupled functionality.
    // Hashing functions, one way transformations unable to be decrypted.
    // https://stackoverflow.com/questions/17292366/hashing-with-sha1-algorithm-in-c-sharp
    // https://docs.microsoft.com/en-us/dotnet/api/system.bitconverter.tostring?view=net-5.0
    // The cryptographic API operates on sequences of bytes, convert our message to byte[] first.
    // BitConverter.ToString(Byte[]) adds '-' deliminator, remove it to conform with spec.
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
