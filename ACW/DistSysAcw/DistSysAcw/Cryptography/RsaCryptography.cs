using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace DistSysAcw.Cryptography
{
    // https://www.youtube.com/watch?v=ywxQLRVqIYU
    // https://docs.microsoft.com/en-us/dotnet/api/system.security.cryptography.rsacryptoserviceprovider.usemachinekeystore?view=net-5.0
    public class RsaCryptography
    {
        private static readonly RSACryptoServiceProvider Csp = new RSACryptoServiceProvider(2048);
        private RSAParameters _privateKey;
        private readonly RSAParameters _publicKey;

        public RsaCryptography()
        {
            _privateKey = Csp.ExportParameters(true);
            _publicKey = Csp.ExportParameters(false);
        }

        public string GetPublicKey()
        {
            var sw = new StringWriter();
            var xs = new XmlSerializer(typeof(RSAParameters));
            xs.Serialize(sw, _publicKey);
            return sw.ToString();
        }
    }
}
