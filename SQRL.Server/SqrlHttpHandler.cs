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
            context.Response.StatusCode = (int) HttpStatusCode.OK;
        }

        public bool IsReusable
        {
            get { return false; }
        }
    }
}