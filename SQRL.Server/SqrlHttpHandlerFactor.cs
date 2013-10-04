using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace SQRL.Server
{
    public class SqrlHttpHandlerFactor : IHttpHandlerFactory
    {
        public IHttpHandler GetHandler(HttpContext context, string requestType, string url, string pathTranslated)
        {
            IHttpHandler handler = null;
            if ("POST".Equals(requestType, StringComparison.OrdinalIgnoreCase))
            {
                handler = new SqrlHttpHandler();
            }

            return handler;
        }

        public void ReleaseHandler(IHttpHandler handler)
        {
        }
    }

}
