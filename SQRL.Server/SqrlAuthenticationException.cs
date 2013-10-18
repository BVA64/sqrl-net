using System;

namespace SQRL.Server
{
    public class SqrlAuthenticationException : Exception
    {
        public SqrlAuthenticationException()
        {
        }

        public SqrlAuthenticationException(string message)
            : base(message)
        {
        }

        public SqrlAuthenticationException(string message, Exception innerException)
            : base(message, innerException)
        {
        }
    }
}