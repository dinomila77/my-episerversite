using System.Web.Mvc;
using System.Web.Security;
using EPiServer.Web.Mvc;
using MyEpiserverSite.Models.Pages;

namespace MyEpiserverSite.Controllers
{
    public abstract class PageControllerBase<T> : PageController<T> where T : SitePageData
    {
        public ActionResult Logout()
        {
            FormsAuthentication.SignOut();
            return RedirectToAction("Index");
        }
    }
}