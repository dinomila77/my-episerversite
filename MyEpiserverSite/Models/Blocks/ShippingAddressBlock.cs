using System;
using System.ComponentModel.DataAnnotations;
using EPiServer.Core;
using EPiServer.DataAbstraction;
using EPiServer.DataAnnotations;

namespace MyEpiserverSite.Models.Blocks
{
    [ContentType(DisplayName = "Shipping Address Block", 
        GUID = "69d6fce6-1506-48e8-b11e-e6aa169b1640", 
        Description = "Shipping Address Block")]
    public class ShippingAddressBlock : BlockData
    {
        public ShippingAddressBlock()
        {
            Address = new ShippingAddress();
        }

        [Ignore]
        public ShippingAddress Address { get; set; }

        [Display(
            Name = "Heading",
            GroupName = SystemTabNames.Content,
            Order = 100)]
        [Required]
        public virtual string Heading { get; set; }

        [Display(
            Name = "Address Line 1 Label",
            GroupName = SystemTabNames.Content,
            Order = 200)]
        [Required]
        public virtual string Address1Text { get; set; }

        [Display(
            Name = "Address Line 2 Label",
            GroupName = SystemTabNames.Content,
            Order = 300)]
        [Required]
        public virtual string Address2Text { get; set; }

        [Display(
            Name = "Town Label",
            GroupName = SystemTabNames.Content,
            Order = 400)]
        [Required]
        public virtual string TownText { get; set; }

        [Display(
            Name = "Postcode Label",
            GroupName = SystemTabNames.Content,
            Order = 500)]
        [Required]
        public virtual string PostcodeText { get; set; }

    }
}