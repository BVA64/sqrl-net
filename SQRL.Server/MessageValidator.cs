using System;

namespace SQRL.Server
{
    public class MessageValidator
    {
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
            return Convert.ToBase64String(nonceBytes);
        }

        public void Validate(SqrlMessage msg)
        {
            ISqrlAuthenticationHandler handler = GetHandler();
            if (handler == null || msg.Uri == null || msg.PublicKeyBase64 == null || msg.SignatureBase64 == null)
            {
                throw new InvalidOperationException();
            }

            if (!handler.VerifySession(msg.IpAddress, msg.ServerNonce))
            {
                throw new Exception("Session not found.");
            }

            string url = msg.Uri.ToString().Remove(0, msg.Uri.Scheme.Length + 3);
            int index = url.IndexOf("&sqrlsig=", StringComparison.Ordinal);
            if (index <= 0)
            {
                return;
            }

            url = url.Remove(index);

            if (!SqrlAuthenticator.Verify(url, msg.SignatureBase64, msg.PublicKeyBase64))
            {
                throw new Exception("Authentication failed.");
            }

            handler.AuthenticateSession(msg.PublicKeyBase64, msg.IpAddress, msg.ServerNonce);
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