using System;

namespace SQRL.Server
{
    public class SqrlMessage
    {
        public Uri Uri { get; set; }

        public string SignatureBase64 { get; set; }

        public string PublicKeyBase64 { get; set; }

        public string Nonce
        {
            get
            {
                if(Uri == null) throw new InvalidOperationException("Unable to get nonce when Uri is null.");
                return Uri.Query.Length > 0 ? Uri.Query.Remove(0, 1) : Uri.Query;
            }
        }

        public byte[] SignatureBytes
        {
            get { return Convert.FromBase64String(SignatureBase64); }
        }

        public byte[] PublicKeyBytes
        {
            get { return Convert.FromBase64String(PublicKeyBase64); }
        }
    }
}