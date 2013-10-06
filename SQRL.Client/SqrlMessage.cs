using System;

namespace SQRL.Client
{
    public class SqrlMessage
    {
        public Uri Uri { get; set; }

        public string SignatureBase64 { get; set; }

        public string PublicKeyBase64 { get; set; }
    }
}