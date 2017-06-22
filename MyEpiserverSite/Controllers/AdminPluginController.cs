using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using EPiServer;
using EPiServer.Core;
using EPiServer.DataAccess;
using EPiServer.PlugIn;
using MyEpiserverSite.Models.Pages;
using MyEpiserverSite.Models.ViewModels;

namespace MyEpiserverSite.Controllers
{
    [GuiPlugIn(
            Area = PlugInArea.AdminMenu,
            Url = "~/AdminPlugin/Index",
            DisplayName = "My Plugin"
            )]
    public class AdminPluginController : Controller
    {
        //public ActionResult Index(CreateContentPage currentPage)
        //{
        //    var model = PageViewModel.Create(currentPage);
        //    return View("Index",model);
        //}

        private readonly IContentLoader _contentLoader;
        private readonly IContentRepository _contentRepository;

        public AdminPluginController(IContentLoader contentLoader, IContentRepository contentRepository)
        {
            _contentLoader = contentLoader;
            _contentRepository = contentRepository;
        }

        public ActionResult Index(string btnCreate)
        {
            //var desc = _contentLoader.GetDescendents(ContentReference.StartPage);
            //var model = new CreateContentViewModel
            //{
            //    Text = "This is my plugin",
            //    ContentReferences = desc
            //};
            //var reference = new ContentReference(49);
            //var content = _contentLoader.Get<IContent>(new ContentReference(49));
            var pages = _contentLoader.GetChildren<PageData>(ContentReference.StartPage);
            var model = new CreateContentViewModel
            {
                Description = "This is my plugin",
                Pages = pages,
                //ParentId = reference
            };

            //if (btnCreate != null)
            //{
            //    Create(model);
            //}

            return View("Index", model);
        }

        [HttpPost]
        public ActionResult Index(CreateContentViewModel model)
        {
            

            if (ModelState.IsValid)
            {                
                var parent = model.ParentId;
                StandardPage standardPage = _contentRepository.GetDefault<StandardPage>(parent);

                standardPage.PageName = "Plugin page";
                standardPage.Introduction = model.Text;
                _contentRepository.Save(standardPage, SaveAction.Publish);
                TempData["success"] = "Page created!";
            }

            return RedirectToAction("Index");
            //return View("Index", model);
        }

        //public void Create(CreateContentViewModel model)
        //{
        //    //var parent = model.Pages.Select(c => c.ContentLink);

        //    ContentReference parent = model.ParentId;
        //    StandardPage standardPage = _contentRepository.GetDefault<StandardPage>(parent);

        //    standardPage.PageName = "Plugin page";
        //    standardPage.Introduction = model.Text;
        //    _contentRepository.Save(standardPage, SaveAction.Publish);
        //    TempData["success"] = "Page created!";
        //}
    }
}