using System;
using System.Text;
using Sodium;

namespace SQRL.Server
{
    public class SqrlAuthenticator
    {
        public static bool Verify(string url, string signature, string publicKey)
        {
            if (url == null) throw new ArgumentNullException("url");
            if(signature == null) throw new ArgumentNullException("signature");
            if(publicKey == null) throw new ArgumentNullException("publicKey");

            var signatureBytes = Convert.FromBase64String(signature);
            var keyBytes = Convert.FromBase64String(publicKey);

            byte[] message;
            if (!CryptoSign.Open(signatureBytes, keyBytes, out message))
            {
                return false;
            }

            string signedUrl = Encoding.ASCII.GetString(message);
            return signedUrl.Equals(url, StringComparison.Ordinal);
        }
    }
}