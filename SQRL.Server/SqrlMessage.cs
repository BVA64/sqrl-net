using System;

namespace SQRL.Server
{
    public class SqrlMessage
    {
        public Uri Uri { get; set; }

        public string SignatureBase64 { get; set; }

        public string PublicKeyBase64 { get; set; }

        public string ClientNonce { get; set; }

        public string ServerNonce { get; set; }

        public string IpAddress { get; set; }

        public string Version { get; set; }

        public string Options { get; set; }

        public int AdditionalDomainCharacters { get; set; }

        public byte[] SignatureBytes
        {
            get { return Convert.FromBase64String(UrlSafeBase64Encoder.Decode(SignatureBase64)); }
        }

        public byte[] PublicKeyBytes
        {
            get { return Convert.FromBase64String(UrlSafeBase64Encoder.Decode(PublicKeyBase64)); }
        }
    }
}