using System;
using System.ComponentModel.DataAnnotations;
using EPiServer.Core;
using EPiServer.DataAbstraction;
using EPiServer.DataAnnotations;
using EPiServer.SpecializedProperties;
using EPiServer.Web;

namespace MyEpiserverSite.Models.Pages
{
    [ContentType(DisplayName = "CreateContentPage", GUID = "8d2c2dee-e7d5-49b4-9781-f9ce4efb3b9a", Description = "")]
    public class CreateContentPage : SitePageData
    {
        [UIHint(UIHint.Textarea)]
        public virtual string Introduction { get; set; }

        [AllowedTypes(typeof(StandardPage))]
        [Display(Name = "Parent Id")]
        public virtual ContentReference ParentId { get; set; }
    }
}