using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using EPiServer;
using EPiServer.Core;
using EPiServer.Framework.Serialization.Json.Internal;
using EPiServer.Web;
using EPiServer.Web.Mvc;
using MyEpiserverSite.Models.Blocks;
using MyEpiserverSite.Models.Entities;
using MyEpiserverSite.Models.ViewModels;
using MyEpiserverSite.Utilities;
using Newtonsoft.Json;

namespace MyEpiserverSite.Controllers
{
    public class UserBlockController : BlockController<UserBlock>
    {
        public override ActionResult Index(UserBlock currentBlock)
        {
            return PartialView();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult UserBasic(UserBasicViewModel userBasic, string btnNext)
        {
            if (btnNext != null)
            {
                if (ModelState.IsValid)
                {
                    var userEntity = GetUserEntity();
                    userEntity.Email = userBasic.Email;
                    userEntity.PersonalNumber = userBasic.PersonalNumber;

                    var userDetails = GetUserDetails(userEntity);
                    return PartialView("_UserDetails",userDetails);
                }
            }
            return PartialView("_UserBasic");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult UserDetails(UserDetailsViewModel userDetails, string btnPrev, string btnSave)
        {
            var userEntity = GetUserEntity();
            if (btnPrev != null)
            {
                StoreUserDetailsToUserEntity(userDetails);
                var userBasic = new UserBasicViewModel
                {
                    Email = userEntity.Email,
                    PersonalNumber = userEntity.PersonalNumber
                };
                ModelState.Clear();
                return PartialView("_UserBasic",userBasic);
            }

            if (btnSave == null) return PartialView("_UserDetails");

            try
            {
                if (ModelState.IsValid)
                {
                    StoreUserDetailsToUserEntity(userDetails);
                    const string path = @"C:\Users\Shkomi\Documents\Temp\epiusers.json";
                    const string errorPath = @"C:\dadsakfak\users.json";
                    var lines = PersistenceUtility.TextToFile(JsonConvert.SerializeObject(userEntity, Formatting.Indented));
                    System.IO.File.AppendAllLines(path, lines);
                    //TempData["success"] = "Thank you for your sumbission.";
                    TempData["success"] = true;
                    RemoveUserEntity();
                    return RedirectToAction("Index");
                }
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("ErrorSavingToDisk", $"{ex.Message} Please try again.");

                var errorMessage = PersistenceUtility.TextToFile($"Error: {ex.Message}");
                const string logPath = @"C:\Users\Shkomi\Documents\Temp\epierror.log";
                System.IO.File.AppendAllLines(logPath, errorMessage);
                return PartialView("_UserDetails");
            }
            
            return PartialView("_UserDetails");
        }

        private UserEntity GetUserEntity()
        {
            if (Session["user"] == null)
            {
                Session["user"] = new UserEntity();
            }
            return (UserEntity)Session["user"];
        }

        private void RemoveUserEntity()
        {
            Session.Remove("user");
        }

        private UserDetailsViewModel GetUserDetails(UserEntity userEntity)
        {
            var userDetails = new UserDetailsViewModel
            {
                FirstName = userEntity.FirstName,
                LastName = userEntity.LastName,
                Address = userEntity.Address,
                City = userEntity.City,
                PhoneNumber = userEntity.PhoneNumber,
                ZipCode = userEntity.ZipCode
            };
            return userDetails;
        }

        private void StoreUserDetailsToUserEntity(UserDetailsViewModel userDetails)
        {
            var userEntity = GetUserEntity();

            userEntity.FirstName = userDetails.FirstName;
            userEntity.LastName = userDetails.LastName;
            userEntity.Address = userDetails.Address;
            userEntity.ZipCode = userDetails.ZipCode;
            userEntity.PhoneNumber = userDetails.PhoneNumber;
            userEntity.City = userDetails.City;
        }
    }
}
