using StayEasePro_WEBAPP.Data.CustomValidation;
using System.ComponentModel.DataAnnotations;

namespace StayEasePro_WebApplication.Data.DTO_s
{
    
    public class SignUpModel
    {
        [Required(ErrorMessage = "Email or phone number is required.")]
        [CustomEmailOrPhoneValidation] // Apply custom validation
        public string EmailOrPhone { get; set; }

        [Required(ErrorMessage = "First name is required.")]
        public string FirstName { get; set; }

        [Required(ErrorMessage = "Last name is required.")]
        public string LastName { get; set; }

        [Required(ErrorMessage = "Password is required.")]
        [MinLength(6, ErrorMessage = "Password must be at least 6 characters long.")]
        public string Password { get; set; }
    }

    public class SignInModel
    {
        [Required(ErrorMessage = "Email address is required.")]
        [EmailAddress(ErrorMessage = "Please enter a valid email address.")]
        public string Email { get; set; }

        [Required(ErrorMessage = "Password is required.")]
        [MinLength(6, ErrorMessage = "Password must be at least 6 characters long.")]
        public string Password { get; set; }


        public bool RememberMe { get; set; }
    }
}
