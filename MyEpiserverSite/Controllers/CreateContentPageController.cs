using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using EPiServer;
using EPiServer.Core;
using EPiServer.Framework.DataAnnotations;
using EPiServer.Web.Mvc;
using MyEpiserverSite.Models.Pages;
using MyEpiserverSite.Models.ViewModels;

namespace MyEpiserverSite.Controllers
{
    [TemplateDescriptor(Default = true)]
    public class CreateContentPageController : PageControllerBase<CreateContentPage>
    {
        public ActionResult Index(CreateContentPage currentPage)
        {
            var model = PageViewModel.Create(currentPage);
            return View(model);
        }
    }
}