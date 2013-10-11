using System;
using System.Linq;
using System.Web.Mvc;
using System.Web.Security;
using SQRL.Server;
using WebMatrix.WebData;
using SQRL.Samples.Web.Filters;
using SQRL.Samples.Web.Models;

namespace SQRL.Samples.Web.Controllers
{
    [Authorize]
    [RequireHttpsOnAppHarbor]
    [InitializeSimpleMembership]
    public class AccountController : Controller
    {
        [AllowAnonymous]
        public ActionResult Login(string returnUrl)
        {
            SetSqrlUrl();

            ViewBag.ReturnUrl = returnUrl;
            return View();
        }

        private void SetSqrlUrl()
        {
            Session["SQRL"] = true;
            var sqrl = new MessageValidator();
            string sqrlSessionId = sqrl.CreateSession();
            string ip = GetClientIp();
            string path = string.Format(
                "{0}://{1}/sqrl.axd?ip={2}&webnon={3}", IsSecureConnection() ? "sqrl" : "qrl",
                Request.Url.Authority, ip, sqrlSessionId);

            ViewBag.SqrlUrl = path;
        }

        private bool IsSecureConnection()
        {
            string proto = Request.Headers["X-Forwarded-Proto"];
            return Request.IsSecureConnection ||
                   string.Equals(proto, "https", StringComparison.InvariantCultureIgnoreCase);
        }

        private string GetClientIp()
        {
            string ipAddress = Request.ServerVariables["HTTP_X_FORWARDED_FOR"];
            if (String.IsNullOrEmpty(ipAddress))
                ipAddress = Request.ServerVariables["REMOTE_ADDR"];
            return ipAddress;
        }

        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public ActionResult Login(LoginModel model, string returnUrl)
        {
            var session = GetAuthenticatedSession();
            if (session != null)
            {
                if (WebSecurity.UserExists(session.UserId))
                {
                    WebSecurity.Login(session.UserId, session.UserId);
                    return RedirectToLocal(returnUrl);
                }

                WebSecurity.CreateUserAndAccount(session.UserId, session.UserId);
                WebSecurity.Login(session.UserId, session.UserId);
                return RedirectToAction("Manage");
            }

            // If we got this far, something failed, redisplay form
            ModelState.AddModelError("", "The SQRL login failed.  Please try again.");
            SetSqrlUrl();
            return View(model);
        }

        private UserSession GetAuthenticatedSession()
        {
            string sessionId = Session.SessionID;
            using (var ctx = new UsersContext())
            {
                return ctx.UserSessions.AsNoTracking()
                          .FirstOrDefault(s => s.SessionId == sessionId && s.AuthenticatedDatetime != null);
            }
        }

        //
        // POST: /Account/LogOff

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult LogOff()
        {
            WebSecurity.Logout();
            Session.Abandon();
            return RedirectToAction("Index", "Home");
        }

        //
        // GET: /Account/Manage

        public ActionResult Manage(ManageMessageId? message)
        {
            ViewBag.StatusMessage =
                message == ManageMessageId.ChangePasswordSuccess ? "Your password has been changed."
                : message == ManageMessageId.SetPasswordSuccess ? "Your password has been set."
                : message == ManageMessageId.RemoveLoginSuccess ? "The external login was removed."
                : "";
            ViewBag.ReturnUrl = Url.Action("Manage");
            return View();
        }

        //
        // POST: /Account/Manage

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Manage(LocalPasswordModel model)
        {
            // If we got this far, something failed, redisplay form
            return View(model);
        }

        #region Helpers
        private ActionResult RedirectToLocal(string returnUrl)
        {
            if (Url.IsLocalUrl(returnUrl))
            {
                return Redirect(returnUrl);
            }
            else
            {
                return RedirectToAction("Index", "Home");
            }
        }

        public enum ManageMessageId
        {
            ChangePasswordSuccess,
            SetPasswordSuccess,
            RemoveLoginSuccess,
        }

        private static string ErrorCodeToString(MembershipCreateStatus createStatus)
        {
            // See http://go.microsoft.com/fwlink/?LinkID=177550 for
            // a full list of status codes.
            switch (createStatus)
            {
                case MembershipCreateStatus.DuplicateUserName:
                    return "User name already exists. Please enter a different user name.";

                case MembershipCreateStatus.DuplicateEmail:
                    return "A user name for that e-mail address already exists. Please enter a different e-mail address.";

                case MembershipCreateStatus.InvalidPassword:
                    return "The password provided is invalid. Please enter a valid password value.";

                case MembershipCreateStatus.InvalidEmail:
                    return "The e-mail address provided is invalid. Please check the value and try again.";

                case MembershipCreateStatus.InvalidAnswer:
                    return "The password retrieval answer provided is invalid. Please check the value and try again.";

                case MembershipCreateStatus.InvalidQuestion:
                    return "The password retrieval question provided is invalid. Please check the value and try again.";

                case MembershipCreateStatus.InvalidUserName:
                    return "The user name provided is invalid. Please check the value and try again.";

                case MembershipCreateStatus.ProviderError:
                    return "The authentication provider returned an error. Please verify your entry and try again. If the problem persists, please contact your system administrator.";

                case MembershipCreateStatus.UserRejected:
                    return "The user creation request has been canceled. Please verify your entry and try again. If the problem persists, please contact your system administrator.";

                default:
                    return "An unknown error occurred. Please verify your entry and try again. If the problem persists, please contact your system administrator.";
            }
        }
        #endregion
    }
}
