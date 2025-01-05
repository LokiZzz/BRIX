using BRIX.GameService.Contracts.Account;
using BRIX.Web.Client.Models.Account;
using BRIX.Web.Client.Models.Common;

namespace BRIX.Web.Client.Services.Auth
{
    public interface IAuthService
    {
        Task<SignInResult> SignIn(SignInRequest model);
        Task SignOut();
        Task<OperationResult> SignUp(SignUpRequest model);
        Task<OperationResult> ForgotPassword(string email);
        Task<OperationResult> ResetPassword(string userId, string newPassword, string token);
        Task<ResendEmailConfirmationResult> ResendConfirmationEmail(string email);
    }
}