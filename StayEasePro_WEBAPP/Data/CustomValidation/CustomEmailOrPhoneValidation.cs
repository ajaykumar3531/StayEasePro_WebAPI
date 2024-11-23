using System.ComponentModel.DataAnnotations;
using System.Text.RegularExpressions;

namespace StayEasePro_WEBAPP.Data.CustomValidation
{
   
    public class CustomEmailOrPhoneValidation : ValidationAttribute
    {
        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            var input = value as string;

            if (string.IsNullOrEmpty(input))
            {
                return new ValidationResult("Email or phone number is required.");
            }

            // Check if the input looks like a phone number
            var phoneRegex = new Regex(@"^\d{10}$"); // Adjust pattern as needed
            if (phoneRegex.IsMatch(input))
            {
                return ValidationResult.Success;
            }

            // Check if the input looks like an email
            if (new EmailAddressAttribute().IsValid(input))
            {
                return ValidationResult.Success;
            }

            // Determine specific error message
            if (input.All(char.IsDigit))
            {
                return new ValidationResult("Please enter a valid phone number (e.g., 10 digits).");
            }
            else
            {
                return new ValidationResult("Please enter a valid email address.");
            }
        }
    }

}
