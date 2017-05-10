using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using EPiServer;
using EPiServer.Core;
using EPiServer.Framework.DataAnnotations;
using EPiServer.ServiceLocation;
using EPiServer.Web.Mvc;
using MyEpiserverSite.Models.Pages;
using MyEpiserverSite.Models;
using MyEpiserverSite.Models.Blocks;

namespace MyEpiserverSite.Controllers
{
    public class FormPageController : PageController<FormPage>
    {
        public ActionResult Index(FormPage currentPage)
        {
            return View("Index",currentPage);
        }

        public ActionResult Save(FormPage currentPage, ShippingAddress address)
        {
            //if (!ModelState.IsValid) return View("Index", currentPage);
            if (currentPage.MainContentArea != null || currentPage.MainContentArea.Items.Any())
            {
                
                var contentLoader = ServiceLocator.Current.GetInstance<IContentLoader>();

                foreach (var item in currentPage.MainContentArea.Items)
                {
                    var shippingBlock = contentLoader.Get<ShippingAddressBlock>(item.ContentLink);

                    if (shippingBlock != null)
                    {
                        shippingBlock.Address = address;
                    }
                }
            }
            return RedirectToAction("Index",currentPage);
        }
    }
}