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
        public virtual string FirstName { get; set; }

        [Required]
        [Display(Name = "Last name")]
        public virtual string LastName { get; set; }

        [Required]
        public virtual string Address { get; set; }

        [Required]
        [Display(Name = "Zip code")]
        public virtual string ZipCode { get; set; }

        [Required]
        public virtual string City { get; set; }

        [Required]
        [EmailAddress]
        public virtual string Email { get; set; }

        [Required]
        [Display(Name = "Personal number")]
        public virtual string PersonalNumber { get; set; }

        [Display(Name = "Phone number")]
        public virtual string PhoneNumber { get; set; }
    }
}