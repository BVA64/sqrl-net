using System;

namespace SQRL.Server
{
    public interface ISqrlAuthenticationHandlerFactory
    {
        ISqrlAuthenticationHandler Create();
    }
}