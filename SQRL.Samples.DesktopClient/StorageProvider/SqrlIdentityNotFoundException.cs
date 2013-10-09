using System;

namespace SQRL.Samples.DesktopClient.StorageProvider
{
    public class SqrlIdentityNotFoundException : Exception
    {
        public SqrlIdentityNotFoundException()
            :base("The requested identity was not found.")
        {
            
        }

        public SqrlIdentityNotFoundException(string message)
            :base(message)
        {
            
        }

        public SqrlIdentityNotFoundException(string message, Exception innerException)
            :base(message, innerException)
        {
            
        }
    }
}