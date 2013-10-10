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
            string d = context.Request.QueryString["d"];
            int additionalChars;
            int.TryParse(d, out additionalChars);
            var msg = new SqrlMessage
                {
                    SignatureBase64 = UrlSafeBase64Encoder.Decode(context.Request.QueryString["sqrlsig"]),
                    PublicKeyBase64 = UrlSafeBase64Encoder.Decode(context.Request.QueryString["sqrlkey"]),
                    ClientNonce = UrlSafeBase64Encoder.Decode(context.Request.QueryString["sqrlnon"]),
                    ServerNonce = context.Request.QueryString["webnon"],
                    IpAddress = context.Request.QueryString["ip"],
                    Version = context.Request.QueryString["sqrlver"],
                    Options = context.Request.QueryString["sqrlopt"],
                    AdditionalDomainCharacters = additionalChars,
                    Uri = context.Request.Url
                };

            var validator = new MessageValidator();
            validator.Validate(msg);

            context.Response.StatusCode = (int) HttpStatusCode.OK;
            context.Response.End();
        }

        public bool IsReusable
        {
            get { return false; }
        }
    }
}