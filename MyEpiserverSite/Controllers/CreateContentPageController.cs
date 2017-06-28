﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
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
                string pageName = "New page";
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
                    var s = pageName.Split(delimiterChars);

                    //var nameList = children.FindAll(p=> p.Name.Split('(').ToString() == s[0]);
                    //var tempList = children.SelectMany(p => p.Name.Split(delimiterChars,StringSplitOptions.RemoveEmptyEntries));
                    var tempList = new List<PageData>();
                    foreach (var pageData in children)
                    {
                        var split = pageData.Name.Split(delimiterChars);
                        var name = split[0];
                        if (name == pageName)
                        {
                            tempList.Add(pageData);
                        }
                    }

                    if (!HasInteger(tempList))
                    {
                        standardPage.Name = pageName += "(2)";
                        repository.Save(standardPage, SaveAction.Publish);
                    }
                    else
                    {
                        standardPage.Name = GetInteger(tempList);
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

        private static bool HasInteger(List<PageData> childPages)
        {
            foreach (var childPage in childPages)
            {
                var charArray = childPage.Name.ToCharArray();
                foreach (var c in charArray)
                {
                    if (char.IsDigit(c))
                    {
                        return true;
                    }
                }
            }
            return false;
        }

        private static bool HasInteger(string page)
        {
            var charArray = page.ToCharArray();
            foreach (var c in charArray)
            {
                if (char.IsDigit(c))
                {
                    return true;
                }
            }
            return false;
        }

        private static string GetInteger(List<PageData> childPages)
        {
            var myNrs = new List<string>();
            var myList = new List<string>();
            char[] delimiterChars = { '(', ')' };
            foreach (var childPage in childPages)
            {
                if (HasInteger(childPage.Name))
                    myList.Add(childPage.Name);
            }

            foreach (var s in myList)
            {

                var nr = s.Split(delimiterChars);
                myNrs.Add(nr[1]);

            }

            var max = myNrs.OrderByDescending(v => int.Parse(v.Substring(0))).First();
            var max2 = myNrs.Select(v => int.Parse(v.Substring(0))).Max();
            var strArray = myList.Select(s => s.Split(delimiterChars)).FirstOrDefault();
            string result = strArray[0] += $"({max2 + 1})";

            return result;
        }
    }
}