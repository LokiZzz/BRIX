using BRIX.GameService.Contracts.Account;
using BRIX.Web.Client.Localization;
using System.ComponentModel.DataAnnotations;

namespace BRIX.Web.Client.Models.Account
{
    public class SignInModel
    {
        [Required(ErrorMessageResourceName = nameof(Resource.Validation_EmailRequired), ErrorMessageResourceType = typeof(Resource))]
        [EmailAddress(ErrorMessageResourceName = nameof(Resource.Validation_EmailFormat), ErrorMessageResourceType = typeof(Resource))]
        public string Email { get; set; } = string.Empty;

        [Required(ErrorMessageResourceName = nameof(Resource.Validation_PasswordRequired), ErrorMessageResourceType = typeof(Resource))]
        public string Password { get; set; } = string.Empty;

        public bool RememberMe { get; set; }

        public SignInRequest ToDto()
        {
            return new SignInRequest() { Email = Email, Password = Password, RememberMe = RememberMe };
        }
    }
}
