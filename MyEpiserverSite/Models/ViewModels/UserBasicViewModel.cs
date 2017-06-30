using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace MyEpiserverSite.Models.ViewModels
{
    public class UserBasicViewModel : IValidatableObject
    {
        [Required]
        [EmailAddress]
        public string Email { get; set; }

        [Required]
        [Display(Name = "Personal number")]
        public string PersonalNumber { get; set; }

        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            if (!IsValidPersonalNumber(PersonalNumber))
            {
                yield return new ValidationResult("The personal number you've entered is not valid.");
            }
        }

        private static bool IsValidPersonalNumber(string identityNumber)
        {
            int sum = 0;

            for (int i = 0; i < identityNumber.Length; i++)
            {
                int n = identityNumber[i] - '0';
                if (i % 2 == 0)
                    n = n * 2 > 9 ? n * 2 - 9 : n * 2;
                sum += n;
            }
            return sum % 10 == 0;
        }
    }
}