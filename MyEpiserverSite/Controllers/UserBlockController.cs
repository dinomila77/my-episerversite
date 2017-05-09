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

namespace MyEpiserverSite.Controllers
{
    public class UserBlockController : BlockController<UserBlock>
    {
        public override ActionResult Index(UserBlock currentBlock)
        {
            return PartialView(currentBlock);
        }
    }
}
