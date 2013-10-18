using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Net.Http;
using System.Text;
using SQRL.Server;
using Sodium;

namespace SQRL.Client
{
    public class SqrlClient
    {
        private const string SqrlVersion = "1";
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
            string site = uri.Host;
            int additionalDomainsChars = GetAdditionalDomainChars(uri);
            if (additionalDomainsChars > 1 && additionalDomainsChars <= uri.LocalPath.Length)
            {
                site += uri.LocalPath.Substring(0, additionalDomainsChars);
            }

            byte[] siteKey = _identity.GetSitePrivateKey(site);
            var keys = CryptoSign.GenerateKeyPair(siteKey);
            var message = new SqrlMessage
            {
                PublicKeyBase64 = Convert.ToBase64String(keys.PublicKey)
            };

            StringBuilder url = new StringBuilder(uri.ToString());
            if (string.IsNullOrEmpty(uri.Query))
                url.Append("?");
            else if (!uri.Query.EndsWith("?") && !uri.Query.EndsWith("&"))
                url.Append("&");

            url.Append("sqrlver=").Append(SqrlVersion);
            url.Append("&sqrlkey=").Append(UrlSafeBase64Encoder.Encode(message.PublicKeyBase64));

            message.Uri = new Uri(url.ToString());

            var idn = new IdnMapping();
            var puny = idn.GetAscii(message.Uri.ToString());
            byte[] urlBytes = Encoding.ASCII.GetBytes(puny);
            byte[] signed = CryptoSign.Sign(urlBytes, keys.SecretKey);

            // The signature is only the first 64 bytes, the rest is the message
            Array.Resize(ref signed, 64);

            message.SignatureBase64 = Convert.ToBase64String(signed);

            return message;
        }

        private int GetAdditionalDomainChars(Uri uri)
        {
            int d = 0;
            var domain = uri.Query.TrimStart('?')
                .Split(new[] {'&'}, StringSplitOptions.RemoveEmptyEntries)
                .FirstOrDefault(s => s.StartsWith("d="));
            if (domain != null)
            {
                string domainStr = domain.Substring(2);

                int.TryParse(domainStr, out d);
            }
            return d;
        }

        private void SendMessage(SqrlMessage message)
        {
            string url = message.Uri.ToString();

            url = message.Uri.Scheme == SchemeSqrls
                      ? url.Replace("sqrl://", "https://")
                      : url.Replace("qrl://", "http://");

            var content = new FormUrlEncodedContent(new[]
                {
                    new KeyValuePair<string, string>("sqrlsig", UrlSafeBase64Encoder.Encode(message.SignatureBase64))
                });

            using (var client = new HttpClient())
            {
                var result = client.PostAsync(url, content).Result;
                result.EnsureSuccessStatusCode();
            }
        }
    }
}