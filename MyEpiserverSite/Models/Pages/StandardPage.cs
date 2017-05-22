using System;
using System.ComponentModel.DataAnnotations;
using EPiServer.Core;
using EPiServer.DataAbstraction;
using EPiServer.DataAnnotations;
using EPiServer.SpecializedProperties;
using EPiServer.Web;

namespace MyEpiserverSite.Models.Pages
{
    [ContentType(DisplayName = "StandardPage", GUID = "b6d77a81-15af-4f6b-9a3b-f8cd0ad9fcdc", Description = "")]
    public class StandardPage : SitePageData
    {

        [CultureSpecific]
        [Display(
            Name = "Main body",
            Description = "The main body will be shown in the main content area of the page, using the XHTML-editor you can insert for example text, images and tables.",
            GroupName = SystemTabNames.Content,
            Order = 1)]
        public virtual XhtmlString MainBody { get; set; }

        [Display(
            Name = "Main Content Area",
            GroupName = SystemTabNames.Content,
            Order = 320)]
        public virtual ContentArea MainContentArea { get; set; }

        [UIHint(UIHint.Image)]
        [Display(Name = "Main Image")]
        public virtual ContentReference Image { get; set; }

        public virtual string Heading { get; set; }

        [UIHint(UIHint.Textarea)]
        public virtual string Introduction { get; set; }

        //[UIHint(UIHint.MediaFile)]
        //[Display(Name = "Main Image")]
        //public virtual ContentReference Image { get; set; }
    }
}