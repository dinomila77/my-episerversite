using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using EPiServer;
using EPiServer.Core;
using EPiServer.Web;
using EPiServer.Web.Mvc;
using MyEpiserverSite.Models.Blocks;
using MyEpiserverSite.Models.ViewModels;

namespace MyEpiserverSite.Controllers
{
    public class UserBlockController : BlockController<UserBlock>
    {
        public override ActionResult Index(UserBlock currentBlock)
        {
            return PartialView();
        }

        //public ActionResult Index()
        //{
        //    return View();
        //}

        [HttpPost]
        public ActionResult UserBasic(UserBasicViewModel userBasic, string btnNext)
        {
            if (btnNext != null)
            {
                if (ModelState.IsValid && Request.IsAjaxRequest())
                {
                    UserBlock userBlock = GetUser();
                    userBlock.Email = userBasic.Email;
                    userBlock.PersonalNumber = userBasic.PersonalNumber;

                    return PartialView("_UserDetails");
                }
            }

            return PartialView("_UserBasic");
        }

        public ActionResult UserDetails(UserDetailsViewModel userDetails, string btnPrev, string btnSave)
        {
            
            return PartialView("_UserDetails");
        }

        private UserBlock GetUser()
        {
            if (Session["user"] == null)
            {
                Session["user"] = new UserBlock();
            }
            return (UserBlock) Session["user"];
        }
    }
}
