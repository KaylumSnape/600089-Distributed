using System;
using System.IO;
using System.Security.Cryptography;

namespace DistSysAcw.Cryptography
{
    public class AesCryptography
    {
        public static byte[] Encrypt(string plainText, byte[] key, byte[] iv)
        {
            try
            {
                byte[] encrypted;

                // Create an Aes object
                // with the specified key and IV.
                using (var myAes = Aes.Create())
                {
                    myAes.Key = key;
                    myAes.IV = iv;

                    // Create an encryptor to perform the stream transform.
                    var encryptor = myAes.CreateEncryptor(myAes.Key, myAes.IV);

                    // Create the streams used for encryption.
                    using (var msEncrypt = new MemoryStream())
                    {
                        using (var csEncrypt = new CryptoStream(msEncrypt, encryptor, CryptoStreamMode.Write))
                        {
                            using (var swEncrypt = new StreamWriter(csEncrypt))
                            {
                                //Write all data to the stream.
                                swEncrypt.Write(plainText);
                            }

                            encrypted = msEncrypt.ToArray();
                        }
                    }
                }

                // Return the encrypted bytes from the memory stream.
                return encrypted;
            }
            catch (Exception)
            {
                return null;
            }
        }
    }
}