using System;
using System.Collections.Generic;
using System.Security.Cryptography;
using System.Text;

namespace DistSysACWClient.Class
{
    public class Verify
    {
        //public bool VerifyHash(RSAParameters rsaParams, byte[] signedData, dynamic signature)
        //{
        //    RSACryptoServiceProvider rsaCSP = new RSACryptoServiceProvider();
        //    SHA1Managed hash = new SHA1Managed();
        //    byte[] hashedData;
        //    CoreExtensions.RSACryptoExtensions.FromXmlStringCore22(rsaCSP, signature);
        //    //rsaCSP.ImportParameters(rsaParams);
        //    bool dataOK = rsaCSP.VerifyData(signedData, CryptoConfig.MapNameToOID("SHA1"), signature);
        //    hashedData = hash.ComputeHash(signedData);
        //    return rsaCSP.VerifyHash(hashedData, CryptoConfig.MapNameToOID("SHA1"), signature);
        //}

        public  bool VerifySignedHash(byte[] OriginalData, byte[] SignedData, dynamic Key)
        {
            try
            {
                bool f = true;
                RSACryptoServiceProvider RSAalg = new RSACryptoServiceProvider();
                CoreExtensions.RSACryptoExtensions.FromXmlStringCore22(RSAalg, Key);
                f = RSAalg.VerifyData(OriginalData, new SHA1CryptoServiceProvider(), SignedData);
                return f;
            }
            catch (CryptographicException e)
            {
                Console.WriteLine(e.Message);

                return false;
            }
        }
        public byte[] string_to_ascii(string Message)  //just takes string and converts to ascii
        {
            ASCIIEncoding ascii = new ASCIIEncoding();
            Byte[] encodedBytes = ascii.GetBytes(Message);

            return encodedBytes;
        }
    }
}
