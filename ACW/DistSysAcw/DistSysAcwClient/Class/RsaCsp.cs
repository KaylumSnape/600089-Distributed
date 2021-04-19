using System;
using System.Collections.Generic;
using System.Security.Cryptography;
using System.Text;

namespace DistSysAcwClient.Class
{
    internal class RsaCsp
    {
        // Verify that the original message was signed with the same public key by the server.
        public static bool Verify(string publicKey, byte[] originalMessageBytes, byte[] signedBytes)
        {
            try
            {
                using var rsa = new RSACryptoServiceProvider();
                rsa.FromXmlString(publicKey); // Initializes an RSA object from the key information from an XML string.

                var verified = rsa.VerifyData(
                    originalMessageBytes,
                    signedBytes,
                    HashAlgorithmName.SHA1,
                    RSASignaturePadding.Pkcs1);

                return verified;
            }
            catch (Exception)
            {
                return false;
            }
        }

        // Takes a list of bytes and encrypts them with the clients RSA public Key it got from the server.
        // Tricky to keep track of what byte[] is what value, should probably change this but, this way is less code.
        public static List<byte[]> EncryptList(string xmlPublicKey, List<byte[]> bytesToEncrypt)
        {
            var encryptedBytes = new List<byte[]>();
            try
            {
                //Create a new instance of RSAParameters.


                using var rsa = new RSACryptoServiceProvider();
                rsa.FromXmlString(xmlPublicKey); // Initializes an RSA object from the key information from an XML string.

                // Get publickey.
                var key = rsa.ExportParameters(false); 
                var RSAKeyInfo = new RSAParameters
                {
                    Modulus = key.Modulus,
                    Exponent = key.Exponent,
                    
                };
                rsa.ImportParameters(RSAKeyInfo);

                encryptedBytes.Add(rsa.Encrypt(bytesToEncrypt[0], false)); // Aes Key.
                encryptedBytes.Add(rsa.Encrypt(bytesToEncrypt[1], false)); // Aes IV.
                encryptedBytes.Add(rsa.Encrypt(bytesToEncrypt[2], false)); // Integer.
            }
            catch (Exception)
            {
                return null;
            }

            return encryptedBytes;
        }

        public static byte[] Encrypt(string publicKey, byte[] bytesToEncrypt)
        {
            try
            {
                using var rsa = new RSACryptoServiceProvider();
                rsa.FromXmlString(publicKey); // Initializes an RSA object from the key information from an XML string.
                return rsa.Encrypt(bytesToEncrypt, false);
            }
            catch (Exception)
            {
                return null;
            }
        }
    }
}