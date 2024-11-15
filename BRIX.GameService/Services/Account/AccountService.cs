using BRIX.GameService.Contracts.Account;
using BRIX.GameService.Entities.Users;
using BRIX.GameService.Options;
using BRIX.GameService.Services.Mail;
using BRIX.Web.Shared.JWT;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.WebUtilities;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using SignInResult = Microsoft.AspNetCore.Identity.SignInResult;

namespace BRIX.GameService.Services.Account
{
    public class AccountService(
        SignInManager<User> signInManager,
        IOptions<JWTOptions> jwtOptions,
        IMailService mail,
        IHttpContextAccessor httpContext) : IAccountService
    {
        private readonly SignInManager<User> _signInManager = signInManager;
        private readonly UserManager<User> _userManager = signInManager.UserManager;

        private readonly JWTOptions _jwtOptions = jwtOptions?.Value
            ?? throw new ArgumentNullException(nameof(jwtOptions));

        private readonly IMailService _mail = mail 
            ?? throw new ArgumentNullException(nameof(mail));
        private readonly HttpContext _httpContext = httpContext?.HttpContext
            ?? throw new ArgumentNullException(nameof(mail));

        public async Task<SignUpResponse> SignUp(SignUpRequest model)
        {
            User newUser = new() { UserName = model.Email, Email = model.Email };
            IdentityResult result = await _userManager.CreateAsync(newUser, model.Password);

            if (result.Succeeded)
            {
                string code = await _signInManager.UserManager.GenerateEmailConfirmationTokenAsync(newUser);
                string hostUrl = $"{_httpContext.Request.Scheme}://" +
                    $"{_httpContext.Request.Host}/api/account/confirm";
                Dictionary<string, string?> queryParams = new() { { "id", newUser.Id.ToString() }, { "code", code } };
                Uri uri = new(QueryHelpers.AddQueryString(hostUrl, queryParams));

                await _mail.SendAsync(
                    [newUser.Email],
                    "Confirmation email",
                    $"Confirmation link:\n{uri}"
                );
            }

            return new SignUpResponse 
            { 
                Successful = result.Succeeded, 
                Errors = result.Errors.Select(x => x.Description) 
            };
        }

        public async Task<SignInResponse> SignIn(SignInRequest model)
        {
            SignInResult result = await _signInManager.PasswordSignInAsync(
                model.Email,
                model.Password,
                model.RememberMe,
                lockoutOnFailure: false
            );

            if (!result.Succeeded)
            {
                User? user = await _userManager.FindByEmailAsync(model.Email);

                if (user?.EmailConfirmed == false)
                {
                    return new SignInResponse { Error = "Need to confirm email." };
                }

                return new SignInResponse { Error = "Username and password are invalid." };
            }

            Claim[] claims = [
                new Claim(ClaimTypes.Name, model.Email),
                new Claim(ClaimTypes.Email, model.Email),
            ];

            string secret = _jwtOptions.SecurityKey ?? throw new Exception("Не указан секрет.");
            SymmetricSecurityKey key = new(Encoding.UTF8.GetBytes(secret));
            SigningCredentials creds = new(key, SecurityAlgorithms.HmacSha256);
            DateTime expiry = DateTime.Now.AddDays(_jwtOptions.ExpiryInDays);
            JwtSecurityToken token = new(
                _jwtOptions.Issuer,
                _jwtOptions.Audience,
                claims,
                expires: expiry,
                signingCredentials: creds
            );

            return new SignInResponse
            {
                Successful = true,
                Token = new JwtSecurityTokenHandler().WriteToken(token)
            };
        }

        public async Task<bool> Confirm(string userId, string code)
        {
            User? user = await _userManager.FindByIdAsync(userId!);

            if (user is not null)
            {
                IdentityResult result = await _userManager.ConfirmEmailAsync(user, code);

                return result.Succeeded;
            }

            return false;
        }

        public async Task<User?> GetCurrentUser()
        {
            string? token = _httpContext.Request.Headers["Authorization"];

            if(string.IsNullOrEmpty(token))
            {
                return null;
            }

            List<Claim> claims = JWTHelper.ParseClaimsFromJwt(token);
            string email = claims?.FirstOrDefault(x => x.Type == ClaimTypes.Email)?.Value ?? string.Empty;
            User? user = string.IsNullOrEmpty(email) ? null : await _userManager.FindByEmailAsync(email);

            return user;
        }

        public async Task<User> GetCurrentUserGuaranteed()
        {
            return await GetCurrentUser() ?? throw new InvalidOperationException("User is not found.");
        }
    }
}
