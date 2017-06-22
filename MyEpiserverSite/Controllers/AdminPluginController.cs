using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using EPiServer;
using EPiServer.Core;
using EPiServer.DataAbstraction;
using EPiServer.DataAccess;
using EPiServer.Globalization;
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
        private readonly ILanguageBranchRepository _languageBranchRepository;

        public AdminPluginController(IContentLoader contentLoader, IContentRepository contentRepository, ILanguageBranchRepository languageBranchRepository)
        {
            _contentLoader = contentLoader;
            _contentRepository = contentRepository;
            _languageBranchRepository = languageBranchRepository;
        }

        public ActionResult Index()
        {
            var pages = _contentLoader.GetChildren<StandardPage>(ContentReference.StartPage /*, CultureInfo.GetCultureInfo("sv")*/ );
            var languages = _languageBranchRepository.ListEnabled();

            var model = new CreateContentViewModel
            {
                Description = "This is my plugin",
                Pages = pages,
                Languages = languages
            };
            return View("Index", model);
        }

        [HttpPost]
        public ActionResult Index(CreateContentViewModel model)
        {
           
            if (ModelState.IsValid)
            {
                try
                {
                    var parent = model.ParentId;
                    var cultureInfo = model.Language;
                    StandardPage standardPage = _contentRepository.GetDefault<StandardPage>(parent,cultureInfo);

                    standardPage.PageName = "Plugin page";
                    standardPage.Introduction = model.Text;
                    _contentRepository.Save(standardPage, SaveAction.Publish);
                    TempData["success"] = "Page created!";
                }
                catch (Exception ex)
                {
                    ModelState.AddModelError("error",ex.Message);
                    throw;
                }
                
            }

            return RedirectToAction("Index");
            //return View("Index", model);
        }
    }
}