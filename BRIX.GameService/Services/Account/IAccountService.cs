using BRIX.GameService.Contracts.Account;
using Microsoft.AspNetCore.Identity;

namespace BRIX.GameService.Services.Account
{
    public interface IAccountService
    {
        Task<SignUpResponse> SignUp(SignUpRequest model);
        Task<SignInResponse> SignIn(SignInRequest model);
        Task<bool> Confirm(string userId, string code);
    }
}