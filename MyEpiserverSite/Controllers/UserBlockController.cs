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
using MyEpiserverSite.Models.Entities;
using MyEpiserverSite.Models.ViewModels;

namespace MyEpiserverSite.Controllers
{
    public class UserBlockController : BlockController<UserBlock>
    {
        public override ActionResult Index(UserBlock currentBlock)
        {
            return PartialView();
        }

        [HttpPost]
        public ActionResult UserBasic(UserBasicViewModel userBasic, string btnNext)
        {
            if (btnNext != null)
            {
                if (ModelState.IsValid && Request.IsAjaxRequest())
                {
                    UserEntity userEntity = GetUser();
                    userEntity.Email = userBasic.Email;
                    userEntity.PersonalNumber = userBasic.PersonalNumber;

                    return PartialView("_UserDetails");
                }
            }
            return View("Index");
            //return PartialView("_UserBasic");
        }

        public ActionResult UserDetails(UserDetailsViewModel userDetails, string btnPrev, string btnSave)
        {
            
            return PartialView("_UserDetails");
        }

        private UserEntity GetUser()
        {
            if (Session["user"] == null)
            {
                Session["user"] = new UserEntity();
            }
            return (UserEntity)Session["user"];
        }
    }
}
