using BRIX.GameService.Contracts.Account;
using BRIX.GameService.Entities.Users;
using Microsoft.AspNetCore.Identity;

namespace BRIX.GameService.Services.Account
{
    public interface IAccountService
    {
        Task<SignUpResponse> SignUp(SignUpRequest model);
        Task<SignInResponse> SignIn(SignInRequest model);
        Task<bool> Confirm(string userId, string code);
        Task<User?> GetCurrentUser();
        Task<User> GetCurrentUserGuaranteed();
        Task ForgotPassword(string email);
        Task ResetPassword(string userId, string password, string token);
    }
}