﻿using System;
using System.ComponentModel.DataAnnotations;
using EPiServer.Core;
using EPiServer.DataAbstraction;
using EPiServer.DataAnnotations;
using EPiServer.SpecializedProperties;

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
            GroupName = SystemTabNames.Content,
            Order = 320)]
        public virtual ContentArea MainContentArea { get; set; }
    }
}