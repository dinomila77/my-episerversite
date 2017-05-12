using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using EPiServer;
using EPiServer.Core;
using EPiServer.Web;
using EPiServer.Web.Mvc;
using EPiServer.Web.Routing;
using MyEpiserverSite.Models.Blocks;
using MyEpiserverSite.Models.Entities;
using MyEpiserverSite.Models.ViewModels;

namespace MyEpiserverSite.Controllers
{
    public class UserBlockController : BlockController<UserBlock>
    {
        //private readonly UrlResolver _urlResolver;

        //public UserBlockController()
        //{
            
        //}
        //public UserBlockController(UrlResolver urlResolver)
        //{
        //    _urlResolver = urlResolver;
        //}

        public override ActionResult Index(UserBlock currentBlock)
        {
            var currentPageLink = ControllerContext.RequestContext.GetContentLink();
            //var currentBlockLink = ((IContent) currentBlock).ContentLink;

            var viewModel = new UserBasicViewModel
            {
                CurrentPageLink = currentPageLink
            };

            return PartialView(viewModel);
        }

        [HttpPost]
        public ActionResult UserBasic(UserBasicViewModel userBasic, string btnNext, UserBlock currentBlock)
        {
            //var url = _urlResolver.GetUrl(userBasic.CurrentPageLink);
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
            return (UserEntity) Session["user"];
        }
    }
}
