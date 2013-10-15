using System;
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
                throw new InvalidOperationException();
            }

            if (msg.Version != SqrlVersion)
            {
                throw new Exception("Invalid version.  Expected version was: " + SqrlVersion);
            }

            if (!handler.VerifySession(msg.IpAddress, msg.ServerNonce))
            {
                throw new Exception("Session not found.");
            }

            if (!Verify(msg))
            {
                throw new Exception("Authentication failed.");
            }

            handler.AuthenticateSession(msg.PublicKeyBase64, msg.IpAddress, msg.ServerNonce);
        }

        private static bool Verify(SqrlMessage sqrl)
        {
            byte[] message;
            if (!CryptoSign.Open(sqrl.SignatureBytes, sqrl.PublicKeyBytes, out message))
            {
                return false;
            }

            string url = sqrl.Uri.ToString()
                             .Replace("https://", "sqrl://")
                             .Replace("http://", "qrl://");

            string signedUrl = Encoding.ASCII.GetString(message);
            return signedUrl.Equals(url, StringComparison.Ordinal);
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