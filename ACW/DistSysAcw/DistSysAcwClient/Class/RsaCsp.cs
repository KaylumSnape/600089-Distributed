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
    }
}
