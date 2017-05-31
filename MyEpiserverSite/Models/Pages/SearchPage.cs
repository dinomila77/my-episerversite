using System;
using System.ComponentModel.DataAnnotations;
using EPiServer.Core;
using EPiServer.DataAbstraction;
using EPiServer.DataAnnotations;
using EPiServer.SpecializedProperties;

namespace MyEpiserverSite.Models.Pages
{
    [ContentType(DisplayName = "SearchPage", GUID = "57e36b3c-7850-458f-93e7-2b680856b3a9", Description = "Search Page")]
    public class SearchPage : SitePageData
    {

    }
}