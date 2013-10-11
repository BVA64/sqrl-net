using System;
using System.Threading.Tasks;
using Microsoft.AspNet.SignalR;

namespace SQRL.Samples.Web
{
    public class LoginHub : Hub
    {
        public override Task OnConnected()
        {
            string id = Context.QueryString["sqrl"];
            Groups.Add(Context.ConnectionId, id);
            return base.OnConnected();
        }
    }
}