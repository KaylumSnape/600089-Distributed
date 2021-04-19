using System;
using System.Security.Cryptography;
using System.Text;

namespace DistSysAcw.Cryptography
{
    // https://docs.microsoft.com/en-us/dotnet/api/system.security.cryptography.rsacryptoserviceprovider.usemachinekeystore?view=net-5.0
    // Asymmetric cipher RSA, encryption and decryption.
    // Perform key exchange in the beginning of Transport Layer Security (TLS) handshake.
    public class RsaCryptography
    {
        // Singleton RSA instance.
        private static RsaCryptography _rsaInstance;

        // To tell csp instances to use the machine key store instead of the user profile key store.
        private static CspParameters _cspParams;

        // Lock object that will be used to synchronise threads during first access to the Singleton.
        private static readonly object Lock = new object();

        // Stores the public and private keys.
        private readonly RSAParameters _rsaParams;

        // Removed key size as having issue sending lots of data in URI query, AddFifty.
        // Private to prevent new construction calls.
        private RsaCryptography()
        {
            // Use the machine key store instead of the user profile key store.
            _cspParams = new CspParameters
            {
                KeyContainerName = "DisSysKeyContainer",
                Flags = CspProviderFlags.UseMachineKeyStore
            };
            // Use new instance of RSACryptoServiceProvider to generate public and private key data.
            using var rsa = new RSACryptoServiceProvider(_cspParams) {KeySize = 1024};
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
            catch (Exception)
            {
                return null;
            }
        }

        public byte[] Encrypt(byte[] plainText)
        {
            try
            {
                using var rsa = new RSACryptoServiceProvider(_cspParams);
                rsa.ImportParameters(_rsaParams);
                return rsa.Encrypt(plainText, false);
            }
            catch (Exception)
            {
                return null;
            }
        }

        public byte[] Decrypt(byte[] cypherText)
        {
            try
            {
                using var rsa = new RSACryptoServiceProvider(_cspParams);
                rsa.ImportParameters(_rsaParams);
                return rsa.Decrypt(cypherText, false);
            }
            catch (Exception)
            {
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
                return BitConverter.ToString(rsa.SignData(Encoding.ASCII.GetBytes(message),
                    CryptoConfig.CreateFromName("SHA1")));
            }
            catch (Exception)
            {
                return null;
            }
        }

        #region TASK14
        public string AddFifty(string encryptedHexInteger, string encryptedHexSymkey, string encryptedHexIv)
        {
            // Remove Hex encoding to get RSA encrypted strings.
            var encryptedIntegerBytes = Converters.HexStringToBytes(encryptedHexInteger);
            var encryptedSymkeyBytes = Converters.HexStringToBytes(encryptedHexSymkey);
            var encryptedIvBytes = Converters.HexStringToBytes(encryptedHexIv);

            // Decrypt RSA encrypted strings.
            var decryptedInteger = Decrypt(encryptedIntegerBytes);
            var decryptedSymkey = Decrypt(encryptedSymkeyBytes);
            var decryptedIv = Decrypt(encryptedIvBytes);
            if (decryptedInteger is null || decryptedSymkey is null || decryptedIv is null) return null;

            // Get int and add fifty.
            var integer = BitConverter.ToInt32(decryptedInteger, 0);
            integer += 50;

            // Aes encrypt integer using provided Key and IV.
            var aesInt = AesCryptography.Encrypt(integer.ToString(), decryptedSymkey, decryptedIv);
            if (aesInt is null) return null;

            // Hex encode and return.
            return BitConverter.ToString(aesInt);
        }

        #endregion
    }
}