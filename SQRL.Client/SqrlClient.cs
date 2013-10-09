using System;
using System.Net.Http;
using System.Text;
using SQRL.Server;
using Sodium;

namespace SQRL.Client
{
    public class SqrlClient
    {
        private const string SqrlVersion = "1.0";
        private const string SchemeSqrl = "qrl";
        private const string SchemeSqrls = "sqrl";

        private readonly Identity _identity;

        public SqrlClient(Identity identity)
        {
            if (identity == null) throw new ArgumentNullException("identity");

            _identity = identity;
        }

        public void Process(string url)
        {
            if (url == null) throw new ArgumentNullException("url");

            var uri = new Uri(url);
            if (uri.Scheme != SchemeSqrl && uri.Scheme != SchemeSqrls)
            {
                throw new ArgumentException(
                    string.Format("Argument url has an invalid URL scheme.  Acceptable values are {0} and {1}.",
                                  SchemeSqrl, SchemeSqrls));
            }

            var message = PrepareMessage(uri);
            SendMessage(message);
        }

        private SqrlMessage PrepareMessage(Uri uri)
        {
            byte[] siteKey = _identity.GetSitePrivateKey(uri.Host);

            StringBuilder url = new StringBuilder(uri.ToString());
            if (string.IsNullOrEmpty(uri.Query))
                url.Append("?");
            else if (!uri.Query.EndsWith("?"))
                url.Append("&");

            url.Append("sqrlnon=").Append(UrlSafeBase64Encoder.Encode(GetClientNonce()));

            string schemalessUrl = url.ToString().Remove(0, uri.Scheme.Length + 3);

            var keypair = CryptoSign.GenerateKeyPair(siteKey);
            byte[] signed = CryptoSign.Sign(schemalessUrl, keypair.SecretKey);
            
            return new SqrlMessage
                {
                    Uri = uri,
                    SignatureBase64 = Convert.ToBase64String(signed),
                    PublicKeyBase64 = Convert.ToBase64String(keypair.PublicKey)
                };
        }

        private string GetClientNonce()
        {
            byte[] ticks = BitConverter.GetBytes(DateTime.UtcNow.Ticks);
            byte[] hash = CryptoHash.SHA256(ticks);
            return Convert.ToBase64String(hash, 0, 9);
        }

        private void SendMessage(SqrlMessage message)
        {
            StringBuilder url = new StringBuilder(message.Uri.ToString());
            if (message.Uri.Scheme == SchemeSqrls)
            {
                url.Replace("sqrl://", "https://");
            }
            else
            {
                url.Replace("qrl://", "http://");
            }

            url.Append("&sqrlsig=").Append(UrlSafeBase64Encoder.Encode(message.SignatureBase64));
            url.Append("&sqrlkey=").Append(UrlSafeBase64Encoder.Encode(message.PublicKeyBase64));
            url.Append("&sqrlver=").Append(SqrlVersion);

            using (var client = new HttpClient())
            {
                var result = client.GetAsync(url.ToString()).Result;
                result.EnsureSuccessStatusCode();
            }
        }
    }
}