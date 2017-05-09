using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace MyEpiserverSite.Models.ViewModels
{
    public class UserBasicBlockModel
    {
        [Required]
        [EmailAddress]
        public string Email { get; set; }

        [Required]
        [Display(Name = "Personal number")]
        public string PersonalNumber { get; set; }
    }
}