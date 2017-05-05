using System;
using System.ComponentModel.DataAnnotations;
using EPiServer.Core;
using EPiServer.DataAbstraction;
using EPiServer.DataAnnotations;
using EPiServer.SpecializedProperties;

namespace MyEpiserverSite.Models.Pages
{
    //[ContentType(DisplayName = "SitePageData", GUID = "2dff107a-a5a0-40d6-92ba-8ccc9f507d72", Description = "")]
    public class SitePageData : PageData
    {
        [Display(GroupName = "SEO", Order = 200, Name = "Search keywords")]
        public virtual string MetaKeywords { get; set; }
    }
}