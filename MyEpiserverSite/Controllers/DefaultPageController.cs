using System;
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
    [TemplateDescriptor(Inherited = true)]
    public class DefaultPageController : PageControllerBase<SitePageData>
    {
        public ViewResult Index(SitePageData currentPage)
        {
            var model = CreateModel(currentPage);
            return View($"~/Views/{currentPage.GetOriginalType().Name}/Index.cshtml", model);
        }

        private static IPageViewModel<SitePageData> CreateModel(SitePageData page)
        {
            var type = typeof(PageViewModel<>).MakeGenericType(page.GetOriginalType());
            return Activator.CreateInstance(type, page) as IPageViewModel<SitePageData>;
        }
    }
}