namespace SQRL.Server
{
    public interface ISqrlAuthenticationHandler
    {
        void StartSession(string sessionId);

        bool VerifySession(string ipAddress, string sessionId);

        void AuthenticateSession(string userId, string ipAddress, string sessionId);
    }
}