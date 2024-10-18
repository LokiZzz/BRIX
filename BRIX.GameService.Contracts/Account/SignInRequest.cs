using System.ComponentModel.DataAnnotations;

namespace BRIX.GameService.Contracts.Account
{
    public class SignInRequest
    {
        [Required]
        public string Email { get; set; } = string.Empty;

        [Required]
        public string Password { get; set; } = string.Empty;

        public bool RememberMe { get; set; }
    }
}
