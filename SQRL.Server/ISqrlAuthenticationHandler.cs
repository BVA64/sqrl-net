namespace SQRL.Server
{
    public interface ISqrlAuthenticationHandler
    {
        void StartSession(string sessionId);

        void AuthenticateSession(string userId, string sessionId);
    }
}