using System.ComponentModel.DataAnnotations;
using EPiServer;
using EPiServer.Core;
using EPiServer.DataAbstraction;
using EPiServer.DataAnnotations;

namespace MyEpiserverSite.Models.Blocks
{
    public class SiteBlockData : BlockData
    {
        //public PageData CurrentPage
        //{
        //    get
        //    {
        //        var pageRouteHelper = EPiServer.ServiceLocation.ServiceLocator.Current.GetInstance<EPiServer.Web.Routing.IPageRouteHelper>();
        //        return pageRouteHelper.Page;
        //    }
        //}
    }
}