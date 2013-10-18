using System;
using System.Net;
using System.Web;

namespace SQRL.Server
{
    public class SqrlHttpHandler : IHttpHandler
    {
        public void ProcessRequest(HttpContext context)
        {
            ProcessRequest(new HttpContextWrapper(context));
        }

        public void ProcessRequest(HttpContextBase context)
        {
            try
            {
                string d = context.Request.QueryString["d"];
                int additionalChars;
                int.TryParse(d, out additionalChars);
                var msg = new SqrlMessage
                    {
                        SignatureBase64 = UrlSafeBase64Encoder.Decode(context.Request.Form["sqrlsig"]),
                        PublicKeyBase64 = UrlSafeBase64Encoder.Decode(context.Request.QueryString["sqrlkey"]),
                        ServerNonce = context.Request.QueryString["nut"],
                        Version = context.Request.QueryString["sqrlver"],
                        Options = context.Request.QueryString["sqrlopt"],
                        AdditionalDomainCharacters = additionalChars,
                        Uri = GetAdjustedUrl(context),
                        IpAddress = context.Request.GetClientIpAddress()
                    };

                var validator = new MessageValidator();
                validator.Validate(msg);

                context.Response.StatusCode = (int) HttpStatusCode.OK;
            }
            catch (SqrlAuthenticationException ex)
            {
                context.Response.StatusCode = (int) HttpStatusCode.Forbidden;
                context.Response.StatusDescription = ex.Message;
            }

            context.Response.End();
        }

        private static Uri GetAdjustedUrl(HttpContextBase context)
        {
            string scheme = context.Request.GetScheme();
            if ("https".Equals(scheme, StringComparison.OrdinalIgnoreCase) &&
                "http".Equals(context.Request.Url.Scheme, StringComparison.OrdinalIgnoreCase))
            {
                string uri = context.Request.Url.ToString();
                uri = uri.Replace("http://", "https://");
                return new Uri(uri);
            }

            return context.Request.Url;
        }

        public bool IsReusable
        {
            get { return false; }
        }
    }
}