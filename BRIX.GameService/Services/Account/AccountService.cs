using BRIX.GameService.Contracts.Account;
using BRIX.GameService.Entities;
using BRIX.GameService.Entities.Users;
using BRIX.GameService.Options;
using BRIX.GameService.Services.Mail;
using BRIX.Web.Shared.JWT;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.WebUtilities;
using Microsoft.EntityFrameworkCore.Internal;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using SignInResult = Microsoft.AspNetCore.Identity.SignInResult;
using BRIX.Library.Extensions;
using BRIX.GameService.Services.Utility;

namespace BRIX.GameService.Services.Account
{
    public class AccountService(
        SignInManager<User> signInManager,
        IOptions<JWTOptions> jwtOptions,
        IOptions<ClientOptions> clientOptions,
        IMailService mail,
        IHttpContextAccessor httpContext,
        IDbContextFactory<ApplicationDbContext> contextFactory) : IAccountService
    {
        private readonly SignInManager<User> _signInManager = signInManager;
        private readonly UserManager<User> _userManager = signInManager.UserManager;

        private readonly JWTOptions _jwtOptions = jwtOptions?.Value
            ?? throw new ArgumentNullException(nameof(jwtOptions));
        private readonly ClientOptions _clientOptions = clientOptions?.Value
            ?? throw new ArgumentNullException(nameof(clientOptions));

        private readonly IMailService _mail = mail 
            ?? throw new ArgumentNullException(nameof(mail));
        private readonly HttpContext _httpContext = httpContext?.HttpContext
            ?? throw new ArgumentNullException(nameof(mail));

        private readonly IDbContextFactory<ApplicationDbContext> _contextFactory = contextFactory;

        public async Task SignUpAsync(SignUpRequest model)
        {
            User newUser = new() { UserName = model.Email, Email = model.Email };
            IdentityResult result = await _userManager.CreateAsync(newUser, model.Password);

            if (result.Succeeded)
            {
                await SendConfirmationEmail(newUser);

                return;
            }

            throw new ProblemException(
                result.Errors.Select(x => (x.Code, x.Description)).ToArray()
            );
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
                    return new SignInResponse { Error = "Need to confirm email.", NeedToConfirmAccount = true };
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

        public async Task<ForgotPasswordResponse> ForgotPassword(string email)
        {
            User? user = await _userManager.FindByEmailAsync(email);

            if (user is not null)
            {
                string token = await _userManager.GeneratePasswordResetTokenAsync(user);
                Dictionary<string, string?> queryParams = new() { { "id", user.Id.ToString() }, { "code", token } };
                Uri uri = new(QueryHelpers.AddQueryString(_clientOptions.ResetPasswordAddress, queryParams));

                await _mail.SendAsync(
                    [email],
                    "Reset password",
                    $"Reset password link:\n{uri}"
                );

                return new() { Success = true };
            }

            return new();
        }

        public async Task<ResetPasswordResponse> ResetPassword(string userId, string password, string token)
        {
            User? user = await _userManager.FindByIdAsync(userId);

            if (user is not null)
            {
                IdentityResult result = await _userManager.ResetPasswordAsync(user, token, password);

                if(result.Succeeded)
                {
                    return new() { Success = true };
                }
            }

            return new();
        }

        public async Task<ResendConfirmationEmailResponse> ResendConfirmationEmail(string email)
        {
            User? user = await _userManager.FindByEmailAsync(email);

            if(user is null)
            {
                return new ResendConfirmationEmailResponse();
            }

            using ApplicationDbContext context = _contextFactory.CreateDbContext();
            EmailConfirmationTries? tries = context.EmailConfirmationTries.FirstOrDefault(x => x.UserId == user.Id);

            if (tries is null)
            {
                tries = new EmailConfirmationTries() { UserId = user.Id, LastTryDateTimeUtc = DateTime.UtcNow };
                context.EmailConfirmationTries.Add(tries);
            }

            int timeoutInSeconds = tries.Count switch
            {
                0 => 0,
                1 => 30,
                2 => 60,
                >= 3 => 300,
                _ => throw new Exception("Невалидное значение кол-ва попыток подтверждения аккаунтов.")
            };

            bool timeoutExpires = DateTime.UtcNow - tries.LastTryDateTimeUtc > new TimeSpan(0, 0, timeoutInSeconds);
            tries.Count++;
            tries.LastTryDateTimeUtc = DateTime.UtcNow;
            await context.SaveChangesAsync();

            if (timeoutExpires)
            {
                await SendConfirmationEmail(user);

                return new ResendConfirmationEmailResponse { Success = true };
            }
            
            TimeSpan retryAfter = new TimeSpan(0, 0, timeoutInSeconds);
            TimeSpan hasPassed = DateTime.UtcNow - tries.LastTryDateTimeUtc;
            int retryAfterInSeconds = (retryAfter - hasPassed).TotalSeconds.Round();

            return new ResendConfirmationEmailResponse { RetryAfterInSeconds = retryAfterInSeconds };
        }

        private async Task SendConfirmationEmail(User user)
        {
            if (!string.IsNullOrEmpty(user.Email))
            {
                string code = await _signInManager.UserManager.GenerateEmailConfirmationTokenAsync(user);
                string hostUrl = $"{_httpContext.Request.Scheme}://" +
                    $"{_httpContext.Request.Host}/api/account/confirm";
                Dictionary<string, string?> queryParams = new() { { "id", user.Id.ToString() }, { "code", code } };
                Uri uri = new(QueryHelpers.AddQueryString(hostUrl, queryParams));

                await _mail.SendAsync(
                    [user.Email],
                    "Confirmation email",
                    $"Confirmation link:\n{uri}"
                );
            }
        }
    }
}
