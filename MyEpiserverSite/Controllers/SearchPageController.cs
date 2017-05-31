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
    public class SearchPageController : PageControllerBase<SearchPage>
    {
        [HttpGet]
        public ActionResult Index(SearchPage currentPage)
        {
            var page = PageViewModel.Create(currentPage);
            return View(page);
        }

        [HttpPost]
        public ActionResult Index(SearchPage currentPage, string searchTerm)
        {
            var model = new SearchPageViewModel(currentPage, searchTerm);
            return View("SearchResults",model);
        }
    }
}