using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using EPiServer;
using EPiServer.Core;
using EPiServer.DataAbstraction;
using EPiServer.DataAccess;
using EPiServer.Framework.DataAnnotations;
using EPiServer.Web.Mvc;
using EPiServer.Web.PageExtensions;
using MyEpiserverSite.Models.Pages;
using MyEpiserverSite.Models.ViewModels;

namespace MyEpiserverSite.Controllers
{
    [TemplateDescriptor(Default = true)]
    public class CreateContentPageController : PageControllerBase<CreateContentPage>
    {
        //public PageData CurrentPage
        //{
        //    get
        //    {
        //        var pageRouteHelper = EPiServer.ServiceLocation.ServiceLocator.Current.GetInstance<EPiServer.Web.Routing.IPageRouteHelper>();
        //        return pageRouteHelper.Page;
        //    }
        //}
        //public void GetCurrent()
        //{
        //    var pageRouteHelper = EPiServer.ServiceLocation.ServiceLocator.Current.GetInstance<EPiServer.Web.Routing.IPageRouteHelper>();
            
        //}

        public ActionResult Index(CreateContentPage currentPage)
        {
            var model = PageViewModel.Create(currentPage);
            return View(model);
        }

        [HttpGet]
        public ActionResult SubmitForm()
        {
            return PartialView("CreateContent");
        }

        [HttpPost]
        public ActionResult SubmitForm(UserBasicViewModel model, CreateContentPage currentPage)
        { 
            if (!ModelState.IsValid) return PartialView("CreateContent",model);
            //if (currentPage.ParentId == null) return PartialView("CreateContent", model);
            try
            {
                ContentReference parent = currentPage.ParentId;
                IContentRepository contentRepository = EPiServer.ServiceLocation.ServiceLocator.Current.GetInstance<IContentRepository>();
                StandardPage standardPage = contentRepository.GetDefault<StandardPage>(parent);

                standardPage.PageName = "New page";
                standardPage.Introduction = $"{model.Email} {model.PersonalNumber}";

                contentRepository.Save(standardPage,SaveAction.Publish);
                ModelState.Clear();
                TempData["success"] = true;
                return PartialView("CreateContent");
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("error", "Something went wrong... " + ex.Message);
                return PartialView("CreateContent");
            }
        }
    }
}