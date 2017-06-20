using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using EPiServer.PlugIn;
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
        public ActionResult Index()
        {
            var model = new CreateContentViewModel {Text = "This is my plugin"};
            return View("Index",model);
        }
    }
}