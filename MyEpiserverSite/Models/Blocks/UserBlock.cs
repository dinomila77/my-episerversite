using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using EPiServer.Core;
using EPiServer.DataAbstraction;
using EPiServer.DataAnnotations;
using MyEpiserverSite.Models.Entities;

namespace MyEpiserverSite.Models.Blocks
{
    [ContentType(DisplayName = "User Block", GUID = "cc4b246e-4c66-4907-8a8d-92061e3ec78a", Description = "Block for registering user")]
    public class UserBlock : SiteBlockData
    {
        [Ignore]
        public UserEntity User { get; set; }

        public UserBlock()
        {
            User = new UserEntity();
        }

        [CultureSpecific]
        [Required]
        [Display(
            Name = "Heading",
            GroupName = SystemTabNames.Content,
            Order = 1)]
        public virtual string Heading { get; set; }

        [Required]
        [Display(
            Name = "First Name label",
            GroupName = SystemTabNames.Content,
            Order = 2)]
        public virtual string FirstName { get; set; }

        [Required]
        [Display(Name = "Last name label",
            GroupName = SystemTabNames.Content,
            Order = 3)]
        public virtual string LastName { get; set; }

        [Required]
        [Display(Name = "Address label",
            GroupName = SystemTabNames.Content,
            Order = 4)]
        public virtual string Address { get; set; }

        [Required]
        [Display(Name = "Zip code label",
            GroupName = SystemTabNames.Content,
            Order = 5)]
        public virtual string ZipCode { get; set; }

        [Required]
        [Display(Name = "City label",
            GroupName = SystemTabNames.Content,
            Order = 6)]
        public virtual string City { get; set; }

        [Required]
        [Display(Name = "Email address label",
            GroupName = SystemTabNames.Content,
            Order = 7)]
        public virtual string Email { get; set; }

        [Required]
        [Display(Name = "Personal number label",
            GroupName = SystemTabNames.Content,
            Order = 8)]
        public virtual string PersonalNumber { get; set; }

        [Required]
        [Display(Name = "Phone number label",
            GroupName = SystemTabNames.Content,
            Order = 9)]
        public virtual string PhoneNumber { get; set; }
    }
}