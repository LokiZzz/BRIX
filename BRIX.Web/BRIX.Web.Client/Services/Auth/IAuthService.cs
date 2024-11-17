using BRIX.GameService.Contracts.Account;

namespace BRIX.Web.Client.Services.Auth
{
    public interface IAuthService
    {
        Task<SignInResponse> SignIn(SignInRequest model);
        Task SignOut();
        Task<SignUpResponse> SignUp(SignUpRequest model);
    }
}