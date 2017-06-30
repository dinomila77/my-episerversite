using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using EPiServer;
using EPiServer.Core;
using EPiServer.DataAccess;
using EPiServer.Framework.DataAnnotations;
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

        [HttpGet]
        public ActionResult SubmitForm()
        {
            return PartialView("CreateContent");
        }

        [HttpPost]
        public ActionResult SubmitForm(UserBasicViewModel model, CreateContentPage currentPage)
        {
            if (!ModelState.IsValid) return PartialView("CreateContent", model);
            //if (currentPage.ParentId == null) return PartialView("CreateContent", model);
            try
            {
                string pageName = "News";
                ContentReference parent = currentPage.ParentId;
                IContentRepository repository = EPiServer.ServiceLocation.ServiceLocator.Current.GetInstance<IContentRepository>();
                var children = repository.GetChildren<PageData>(parent).ToList();
                StandardPage standardPage = repository.GetDefault<StandardPage>(parent);

                //The code example below shows how to define the user defined property MainBody:
                standardPage.Property["MainBody"].Value = $"<p>{model.Email}</p> <p>{model.PersonalNumber}</p>";

                //The following case achieves the same with a strongly typed model:
                //standardPage.MainBody = new XhtmlString($"Strong typed <p>{model.Email}</p> <p>{model.PersonalNumber}</p>");

                char[] delimiterChars = { '(', ')' };
                if (children.Any() && children.Exists(p => p.Name == pageName))
                {
                    var tempList = new List<PageData>();                   
                    foreach (var pageData in children)
                    {
                        var split = pageData.Name.Split(delimiterChars);
                        var name = split[0];
                        if (name == pageName && pageData.Name != pageName)
                        {
                            tempList.Add(pageData);
                        }
                    }

                    if (tempList.Count == 0)
                    {
                        standardPage.Name = pageName + "(2)";
                        repository.Save(standardPage, SaveAction.Publish);
                    }
                    else
                    {
                        standardPage.Name = FindHighestNrInPageNames(tempList,pageName,delimiterChars);
                        repository.Save(standardPage, SaveAction.Publish);
                    }
                }
                else
                {
                    standardPage.Name = pageName;
                    repository.Save(standardPage, SaveAction.Publish);
                }

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

        private static string FindHighestNrInPageNames(List<PageData> childPages, string pageName, char[] delimiterChars)
        {
            var max = childPages.Select(n => n.Name.Split(delimiterChars,StringSplitOptions.RemoveEmptyEntries)).Select(x=> x[1]).Max(i=> int.Parse(i.Substring(0)));       
            return pageName + $"({max + 1})";
        }
    }
}