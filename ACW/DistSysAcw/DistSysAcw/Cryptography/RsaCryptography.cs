using System;
using System.IO;
using System.Security.Cryptography;
using System.Text;
using System.Xml.Serialization;
using Exception = System.Exception;

namespace DistSysAcw.Cryptography
{
    // https://docs.microsoft.com/en-us/dotnet/api/system.security.cryptography.rsacryptoserviceprovider.usemachinekeystore?view=net-5.0
    // Asymmetric cipher RSA, encryption and decryption.
    // Perform key exchange in the beginning of Transport Layer Security (TLS) handshake.
    public class RsaCryptography
    {
        // Singleton RSA instance.
        private static RsaCryptography _rsaInstance;

        // Stores the public and private keys.
        private readonly RSAParameters _rsaParams;
        
        // To tell csp instances to use the machine key store instead of the user profile key store.
        private static CspParameters _cspParams;

        // Lock object that will be used to synchronise threads during first access to the Singleton.
        private static readonly object Lock = new object();
        
        // Private to prevent new construction calls.
        private RsaCryptography()
        {
            // Use the machine key store instead of the user profile key store.
            _cspParams = new CspParameters
            {
                KeyContainerName = "MyKeyContainer",
                Flags = CspProviderFlags.UseMachineKeyStore
            };
            // Use new instance of RSACryptoServiceProvider to generate public and private key data.
            using var rsa = new RSACryptoServiceProvider(2048, _cspParams);
            // True exports public and private keys, false just exports public.
            _rsaParams = rsa.ExportParameters(true);
        }



        // Get singleton instance.
        public static RsaCryptography GetRsaInstance()
        {
            // If an instance doesn't exist, make one.
            if (_rsaInstance != null) return _rsaInstance;

            // Lock the creation of the instance, as multiple clients could pass initial check on startup.
            lock (Lock)
            {
                // Because multiple can get through first check, we have to check again.
                _rsaInstance ??= new RsaCryptography();
            }
            return _rsaInstance;
        }

        public string GetPublicKey()
        {
            try
            {
                using var rsa = new RSACryptoServiceProvider(_cspParams);
                rsa.ImportParameters(_rsaParams);
                return rsa.ToXmlString(false);
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                return null;
            }

        }

        public string Encrypt(string plainText)
        {
            try
            {
                using var rsa = new RSACryptoServiceProvider(_cspParams);
                rsa.ImportParameters(_rsaParams);
                return Encoding.Unicode.GetString(rsa.Encrypt(Encoding.Unicode.GetBytes(plainText), false));
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                return null;
            }
        }

        public string Decrypt(string cypherText)
        {
            try
            {
                using var rsa = new RSACryptoServiceProvider(_cspParams);
                rsa.ImportParameters(_rsaParams);
                return Encoding.Unicode.GetString(rsa.Decrypt(Encoding.Unicode.GetBytes(cypherText), false));
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                return null;
            }
        }

        public string Sign(string message)
        {
            try
            {
                using var rsa = new RSACryptoServiceProvider(_cspParams);
                rsa.ImportParameters(_rsaParams);
                // Computes the hash value of the specified byte array using the specified hash algorithm, and signs the resulting hash value.
                // Then converts it to Hex.
                return BitConverter.ToString(rsa.SignData(Encoding.ASCII.GetBytes(message), CryptoConfig.CreateFromName("SHA1")));
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                return null;
            }
        }
    }
}
