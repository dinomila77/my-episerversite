using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using EPiServer.Core;
using EPiServer.DataAbstraction;
using EPiServer.DataAnnotations;

namespace MyEpiserverSite.Models.Blocks
{
    [ContentType(DisplayName = "UserBlock", GUID = "cc4b246e-4c66-4907-8a8d-92061e3ec78a", Description = "")]
    public class UserBlock : SiteBlockData
    {

        //[CultureSpecific]
        //[Display(
        //    Name = "Name",
        //    Description = "Name field's description",
        //    GroupName = SystemTabNames.Content,
        //    Order = 1)]
        //public virtual string Name { get; set; }

        [Required]
        [Display(Name ="First name")]
        public string FirstName { get; set; }

        [Required]
        [Display(Name = "Last name")]
        public string LastName { get; set; }

        [Required]
        public string Address { get; set; }

        [Required]
        [Display(Name = "Zip code")]
        public string ZipCode { get; set; }

        [Required]
        public string City { get; set; }

        [Required]
        [EmailAddress]
        public string Email { get; set; }

        [Required]
        [Display(Name = "Personal number")]
        public string PersonalNumber { get; set; }

        [Display(Name = "Phone number")]
        public string PhoneNumber { get; set; }
    }
}