using BRIX.GameService.Contracts.Account;
using BRIX.Web.Client.Localization;
using System.ComponentModel.DataAnnotations;

namespace BRIX.Web.Client.Models.Account
{
    public class SignUpModel
    {
        [Required(ErrorMessageResourceName = nameof(Resource.Validation_EmailRequired), ErrorMessageResourceType = typeof(Resource))]
        [EmailAddress(ErrorMessageResourceName = nameof(Resource.Validation_EmailFormat), ErrorMessageResourceType = typeof(Resource))]
        public string Email { get; set; } = string.Empty;

        [Required(ErrorMessageResourceName = nameof(Resource.Validation_PasswordRequired), ErrorMessageResourceType = typeof(Resource))]
        [StringLength(100, MinimumLength = 6, ErrorMessageResourceName = nameof(Resource.Validation_PasswordLength), ErrorMessageResourceType = typeof(Resource))]
        public string Password { get; set; } = string.Empty;

        [Compare(nameof(Password), ErrorMessageResourceName = nameof(Resource.Validation_PasswordConfirmCompare), ErrorMessageResourceType = typeof(Resource))]
        public string ConfirmPassword { get; set; } = string.Empty;

        public SignUpRequest ToDto()
        {
            return new SignUpRequest() { Email = Email, Password = Password };
        }
    }
}
