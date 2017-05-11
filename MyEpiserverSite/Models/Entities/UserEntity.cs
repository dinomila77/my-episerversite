using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace MyEpiserverSite.Models.Entities
{
    public class UserEntity
    {
        [Required]
        [Display(Name = "First name")]
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