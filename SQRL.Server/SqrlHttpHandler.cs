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
            var msg = new SqrlMessage
                {
                    SignatureBase64 = context.Request.Form["SIG"],
                    PublicKeyBase64 = context.Request.Form["PK"],
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