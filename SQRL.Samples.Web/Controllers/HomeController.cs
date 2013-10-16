using System;
using System.Configuration;
using System.Web.Mvc;

namespace SQRL.Samples.Web.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            ViewBag.DownloadLink = ConfigurationManager.AppSettings["ClientDownloadLink"];
            return View();
        }
    }
}
