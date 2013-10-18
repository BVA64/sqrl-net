using System;
using System.Web;

namespace SQRL.Server
{
    public static class RequestExtensions
    {
         public static string GetScheme(this HttpRequestBase request)
         {
             string proto = request.Headers["X-Forwarded-Proto"];
             if (string.IsNullOrEmpty(proto))
             {
                 return request.Url.Scheme;
             }

             return proto;
         }

        public static bool GetIsSecureConnection(this HttpRequestBase request)
        {
            string proto = request.GetScheme();
            return request.IsSecureConnection ||
                   string.Equals(proto, "https", StringComparison.InvariantCultureIgnoreCase);
        }

        public static string GetClientIpAddress(this HttpRequestBase request)
        {
            string ipAddress = request.ServerVariables["HTTP_X_FORWARDED_FOR"];
            if (!string.IsNullOrEmpty(ipAddress))
            {
                ipAddress = ipAddress.Split(',')[0];
            }
            return ipAddress ?? request.ServerVariables["REMOTE_ADDR"];

        }
    }
}