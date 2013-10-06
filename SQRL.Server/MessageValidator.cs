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
            if(msg.Uri == null || msg.PublicKeyBase64 == null || msg.SignatureBase64 == null)
            {
                return;
            }

            if (!SqrlAuthenticator.Verify(msg.Uri.ToString(), msg.SignatureBase64, msg.PublicKeyBase64))
            {
                return;
            }

            var handler = GetHandler();
            if (handler != null)
            {
                handler.AuthenticateSession(msg.PublicKeyBase64, msg.Nonce);
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