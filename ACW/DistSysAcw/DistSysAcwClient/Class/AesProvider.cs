using System;
using System.Collections.Generic;
using System.IO;
using System.Security.Cryptography;

namespace DistSysAcwClient.Class
{
    internal class AesProvider
    {
        // Return a list of newly generated key and initialization vector (IV).
        public static List<byte[]> GetAesInfo()
        {
            // Create a new instance of the Aes class.
            using var myAes = Aes.Create();
            myAes.GenerateKey();
            myAes.GenerateIV();

            var aesInfo = new List<byte[]>
            {
                myAes.Key,
                myAes.IV
            };
            return aesInfo;
        }

        internal static string Decrypt(byte[] key, byte[] iV, byte[] cipherText)
        {
            try
            {
                // Create an Aes object with the specified key and IV.
                using var myAes = Aes.Create();
                myAes.Key = key;
                myAes.IV = iV;

                // Create a decryptor to perform the stream transform.
                var decryptor = myAes.CreateDecryptor(myAes.Key, myAes.IV);

                // Create the streams used for decryption.
                using var msDecrypt = new MemoryStream(cipherText);
                using var csDecrypt = new CryptoStream(msDecrypt, decryptor, CryptoStreamMode.Read);
                using var srDecrypt = new StreamReader(csDecrypt);

                // Read the decrypted bytes from the decrypting stream and return them.
                return srDecrypt.ReadToEnd();
            }
            catch (Exception)
            {
                return null;
            }

        }
    }
}