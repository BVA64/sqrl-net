using System;
using System.Collections.Generic;
using System.Net.Http;
using Sodium;

namespace SQRL.Client
{
    public class SqrlClient
    {
        private const string SchemeSqrl = "sqrl";
        private const string SchemeSqrls = "sqrls";

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

            string url = uri.ToString();
            string schemalessUrl = url.Remove(0, uri.Scheme.Length + 3);

            byte[] signed = CryptoSign.Sign(schemalessUrl, siteKey);

            // TODO: Public Key???
            byte[] publicKey = new byte[siteKey.Length];

            return new SqrlMessage
                {
                    Uri = uri,
                    SignatureBase64 = Convert.ToBase64String(signed),
                    PublicKeyBase64 = Convert.ToBase64String(publicKey)
                };
        }

        private void SendMessage(SqrlMessage message)
        {
            string url = message.Uri.ToString();
            string httpUrl = message.Uri.Scheme == SchemeSqrls
                                 ? url.Replace("sqrls://", "https://")
                                 : url.Replace("sqrl://", "http://");

            using (var client = new HttpClient())
            {
                var content = new FormUrlEncodedContent(new[]
                    {
                        new KeyValuePair<string, string>("SIG", message.SignatureBase64),
                        new KeyValuePair<string, string>("PUB", message.PublicKeyBase64)
                    });

                var result = client.PostAsync(httpUrl, content).Result;
                result.EnsureSuccessStatusCode();
            }
        }
    }
}