using System;
using System.ComponentModel.DataAnnotations;
using EPiServer.Core;
using EPiServer.DataAbstraction;
using EPiServer.DataAnnotations;
using EPiServer.SpecializedProperties;

namespace MyEpiserverSite.Models.Pages
{
    [ContentType(
        DisplayName = "Form Page",
        GroupName = "Form Page", 
        Description = "Form Page",
        GUID = "7607a06d-22da-4ab0-b5bb-f4dfe0a51a20")]
    public class FormPage : PageData
    {
        [Display(
            Name = "Content Area",
            GroupName = SystemTabNames.Content,
            Order = 10)]
        public virtual ContentArea MainContentArea { get; set; }
    }
}