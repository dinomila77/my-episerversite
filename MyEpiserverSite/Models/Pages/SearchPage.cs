using System;
using System.ComponentModel.DataAnnotations;
using EPiServer.Core;
using EPiServer.DataAbstraction;
using EPiServer.DataAnnotations;
using EPiServer.SpecializedProperties;

namespace MyEpiserverSite.Models.Pages
{
    [ContentType(DisplayName = "SearchPage", GUID = "57e36b3c-7850-458f-93e7-2b680856b3a9", Description = "Search Page")]
    public class SearchPage : PageData
    {

        [CultureSpecific]
        [Display(
            Name = "Page Title",
            Description = "The main body will be shown in the main content area of the page, using the XHTML-editor you can insert for example text, images and tables.",
            GroupName = SystemTabNames.Content,
            Order = 1)]
        public virtual string PageTitle { get; set; }

    }
}