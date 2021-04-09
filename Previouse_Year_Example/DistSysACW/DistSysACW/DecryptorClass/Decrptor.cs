using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Security.Cryptography;
using System.Text;
using CoreExtensions;

namespace DistSysACW.DecryptorClass
{
    public class Decrptor
    {
        //----------------------------CONVERTS STRING TO AN ARRAY OF BYTES------------------------------------------------------------------------------//
        public byte[] string_to_ascii(string Message)  //just takes string and converts to ascii
        {
            ASCIIEncoding ascii = new ASCIIEncoding();
            Byte[] encodedBytes = ascii.GetBytes(Message);

            return encodedBytes;
        }
        //----------------------------------------------CONVERTS BYTE ARRAY TO HEX STRING--------------------------------------------------------------//
        public string ByteArrayToHexString(byte[] byteArray)
        {
            string hexString = "";
            if (null != byteArray)
            {
                foreach (byte b in byteArray)
                {

                    hexString += (b + 6).ToString("x2"); //the +6 is the salt 
                    hexString += "-";
                }
            }
            return hexString;
        }
        //------------------------------------------ENCRYPTOR-------------------------------------------------------------------------------------------------//
        public byte[] RSAEncrypt(byte[] DataToEncrypt, RSAParameters RSAKeyInfo)
        {
            try
            {
                byte[] encryptedData; using (RSACryptoServiceProvider RSA = new RSACryptoServiceProvider())
                {

                    RSA.ImportParameters(RSAKeyInfo); encryptedData = RSA.Encrypt(DataToEncrypt, false);
                }
                return encryptedData;
            }
            catch (CryptographicException e) 
            {
                Console.WriteLine(e.Message);
                return null;
            }
        }
        //------------------------------------------DECRYPTOR------------------------------------------------------------------------------------------------//
        public byte[] RSADecrypt(byte[] DataToDecrypt, RSAParameters RSAKeyInfo)
        {
            try
            {
                byte[] decryptedData;
                using (RSACryptoServiceProvider RSA = new RSACryptoServiceProvider())
                {
                    RSA.ImportParameters(RSAKeyInfo);
                    decryptedData = RSA.Decrypt(DataToDecrypt, false);
                }
                return decryptedData;
            }
            catch (CryptographicException e)
            {
                Console.WriteLine(e.ToString());
                return null;
            }
        }

        public byte[] SHA256_Encrypt(byte[] message)
        {
            byte[] sha256ByteMessage;
            SHA256 sHA = new SHA256CryptoServiceProvider();
            sha256ByteMessage = sHA.ComputeHash(message);
            return sha256ByteMessage;
        }

        public byte[] SHA1_Encrypt(byte[] message)
        {
            byte[] sha1ByteMessage;
            SHA1 sHA = new SHA1CryptoServiceProvider();
            sha1ByteMessage = sHA.ComputeHash(message);
            return sha1ByteMessage;
        }

        //-------------------------------------------------HASH AND SIGN DATA--------------------------------------------------------//
        public  byte[] HashAndSignBytes(byte[] DataToSign, dynamic Key)
        {
            try
            {
                CoreExtensions.RSACryptoExtensions.FromXmlStringCore22(Middleware.AuthMiddleware.rsaServer,Key);
               return Middleware.AuthMiddleware.rsaServer.SignData(DataToSign, new SHA1CryptoServiceProvider());
            }
            catch (CryptographicException e)
            {
                Console.WriteLine(e.Message);

                return null;
            }
        }
        //-----------------------------------verify
       

    }
}

