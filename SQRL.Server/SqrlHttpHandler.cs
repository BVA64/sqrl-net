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
                        Uri = context.Request.Url,
                        IpAddress = GetClientIp(context)
                    };

                var validator = new MessageValidator();
                validator.Validate(msg);

                context.Response.StatusCode = (int) HttpStatusCode.OK;
            }
            catch (Exception ex)
            {
                context.Response.StatusCode = (int) HttpStatusCode.Forbidden;
                context.Response.StatusDescription = ex.Message;
            }

            context.Response.End();
        }

        private string GetClientIp(HttpContextBase context)
        {
            string ipAddress = context.Request.ServerVariables["HTTP_X_FORWARDED_FOR"];
            if (String.IsNullOrEmpty(ipAddress))
                ipAddress = context.Request.ServerVariables["REMOTE_ADDR"];
            return ipAddress;
        }

        public bool IsReusable
        {
            get { return false; }
        }
    }
}