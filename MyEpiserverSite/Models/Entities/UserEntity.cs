using System.ComponentModel.DataAnnotations;

namespace MyEpiserverSite.Models.Entities
{
    public class UserEntity
    {
        [Required]
        public virtual string FirstName { get; set; }

        [Required]
        public virtual string LastName { get; set; }

        [Required]
        public virtual string Address { get; set; }

        [Required]
        public virtual string ZipCode { get; set; }

        [Required]
        public virtual string City { get; set; }

        [Required]
        [EmailAddress]
        public virtual string Email { get; set; }

        [Required]
        public virtual string PersonalNumber { get; set; }

        public virtual string PhoneNumber { get; set; }
    }
}