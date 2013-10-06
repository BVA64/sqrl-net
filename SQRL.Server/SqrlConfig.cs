using System;

namespace SQRL.Server
{
    public static class SqrlConfig
    {
        static SqrlConfig()
        {
            NonceLength = 64;
        }

        public static int NonceLength { get; set; }

        public static ISqrlAuthenticationHandlerFactory AuthenticationHandlerFactory { get; set; }
    }
}