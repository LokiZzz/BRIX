using BRIX.GameService.Contracts.Account;
using BRIX.Web.Client.Models.Common;

namespace BRIX.Web.Client.Services.Auth
{
    public interface IAuthService
    {
        Task<SignInResponse> SignIn(SignInRequest model);
        Task SignOut();
        Task<OperationResult> SignUp(SignUpRequest model);
        Task<bool> ForgotPassword(string email);
        Task<bool> ResetPassword(string userId, string newPassword, string token);
        Task<ResendConfirmationEmailResponse> ResendConfirmationEmail(string email);
    }
}