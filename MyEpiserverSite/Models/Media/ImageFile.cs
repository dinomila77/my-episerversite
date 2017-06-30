using System.ComponentModel.DataAnnotations;
using EPiServer.Core;
using EPiServer.DataAbstraction;
using EPiServer.DataAnnotations;
using EPiServer.Framework.DataAnnotations;

namespace MyEpiserverSite.Models.Media
{
    [ContentType(DisplayName = "ImageFile", GUID = "05019e32-c56d-4b75-ac44-2015bf71c919", Description = "")]
    [MediaDescriptor(ExtensionString = "jpg,jpeg,jpe,gif,bmp,png,ico")]
    public class ImageFile : ImageData
    {
        [Display(
            Name = "Description",
            Description = "Description field's description",
            GroupName = SystemTabNames.Content,
            Order = 20)]
        public virtual string Copyright { get; set; }

    }
}