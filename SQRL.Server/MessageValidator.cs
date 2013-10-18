using System;
using System.Globalization;
using System.Text;
using Sodium;

namespace SQRL.Server
{
    public class MessageValidator
    {
        private const string SqrlVersion = "1";

        public string CreateSession()
        {
            string sessionId = GenerateSessionId();

            var handler = GetHandler();
            if (handler != null)
            {
                handler.StartSession(sessionId);
            }

            return sessionId;
        }

        private string GenerateSessionId()
        {
            var nonceBytes = Sodium.Random.GetBytes(SqrlConfig.NonceLength);
            return UrlSafeBase64Encoder.Encode(Convert.ToBase64String(nonceBytes));
        }

        public void Validate(SqrlMessage msg)
        {
            ISqrlAuthenticationHandler handler = GetHandler();
            if (handler == null || msg.Uri == null || msg.PublicKeyBase64 == null || msg.SignatureBase64 == null)
            {
                string error = string.Empty;
                if (handler == null) error = "Handler";
                else if (msg.Uri == null) error = "URL";
                else if (msg.PublicKeyBase64 == null) error = "Public key";
                else if (msg.SignatureBase64 == null) error = "Signature";

                throw new SqrlAuthenticationException(string.Format("Invalid parameters.  {0} was not specified.", error));
            }

            if (msg.Version != SqrlVersion)
            {
                throw new SqrlAuthenticationException("Invalid version.  Expected version was: " + SqrlVersion);
            }

            if (!handler.VerifySession(msg.IpAddress, msg.ServerNonce))
            {
                throw new SqrlAuthenticationException("Session not found.");
            }

            VerifyMessage(msg);

            handler.AuthenticateSession(msg.PublicKeyBase64, msg.IpAddress, msg.ServerNonce);
        }

        private static void VerifyMessage(SqrlMessage sqrl)
        {
            string url = sqrl.Uri.ToString()
                            .Replace("https://", "sqrl://")
                            .Replace("http://", "qrl://");
            
            var idn = new IdnMapping();
            var puny = idn.GetAscii(url);
            var punyBytes = Encoding.ASCII.GetBytes(puny);
            var signatureBytes = sqrl.SignatureBytes;

            var signature = new byte[punyBytes.Length + signatureBytes.Length];
            Buffer.BlockCopy(signatureBytes, 0, signature, 0, signatureBytes.Length);
            Buffer.BlockCopy(punyBytes, 0, signature, signatureBytes.Length, punyBytes.Length);

            if (!CryptoSign.Open(signature, sqrl.PublicKeyBytes))
            {
                throw new SqrlAuthenticationException("Signature verification failed.");
            }
        }

        private ISqrlAuthenticationHandler GetHandler()
        {
            ISqrlAuthenticationHandler handler = null;
            if (SqrlConfig.AuthenticationHandlerFactory != null)
            {
                handler = SqrlConfig.AuthenticationHandlerFactory.Create();
            }

            return handler;
        }
    }
}