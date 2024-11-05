using BRIX.GameService.Entities.Users;
using BRIX.GameService.Contracts.Account;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using SignInResult = Microsoft.AspNetCore.Identity.SignInResult;
using Microsoft.Extensions.Options;
using BRIX.GameService.Options;
using BRIX.GameService.Services.Mail;
using System.Collections.Specialized;
using Microsoft.AspNetCore.WebUtilities;

namespace BRIX.GameService.Controllers.Account
{
    [ApiController]
    [Route("api/[controller]/[action]")]
    public class AccountController(
        UserManager<User> userManager,
        SignInManager<User> signInManager,
        IOptions<JWTOptions> jwtOptions,
        IMailService mail) : Controller
    {
        private readonly UserManager<User> _userManager = userManager;
        private readonly SignInManager<User> _signInManager = signInManager;
        private readonly JWTOptions _jwtOptions = jwtOptions?.Value ?? throw new ArgumentNullException(nameof(jwtOptions));
        private readonly IMailService _mail = mail ?? throw new ArgumentNullException(nameof(mail));

        [HttpPost]
        public async Task<IActionResult> SignUp([FromBody] SignUpRequest model)
        {
            User newUser = new() { UserName = model.Email, Email = model.Email };
            IdentityResult result = await _userManager.CreateAsync(newUser, model.Password);

            if (!result.Succeeded)
            {
                return Ok(new SignUpResponse
                {
                    Successful = false,
                    Errors = result.Errors.Select(x => x.Description)
                });
            }

            string code = await _signInManager.UserManager.GenerateEmailConfirmationTokenAsync(newUser);
            string hostUrl = $"{HttpContext.Request.Scheme}://{HttpContext.Request.Host}/api/account/confirm";
            Dictionary<string, string?> queryParams = new() { { "id", newUser.Id.ToString() }, { "code", code } };
            Uri uri = new(QueryHelpers.AddQueryString(hostUrl, queryParams));

            await _mail.SendAsync(
                [newUser.Email],
                "Confirmation email",
                $"Confirmation link:\n{uri}"
            );

            return Ok(new SignUpResponse { Successful = true });
        }

        [HttpPost]
        public async Task<IActionResult> SignIn([FromBody] SignInRequest signIn)
        {
            SignInResult result = await _signInManager.PasswordSignInAsync(
                signIn.Email, 
                signIn.Password, 
                signIn.RememberMe, 
                lockoutOnFailure: false
            );

            if (!result.Succeeded)
            {
                User? user = await _userManager.FindByEmailAsync(signIn.Email);
                
                if(user?.EmailConfirmed == false)
                {
                    return BadRequest(new SignInResponse { Error = "Need to confirm email." });
                }

                return BadRequest(new SignInResponse { Error = "Username and password are invalid." });
            }

            Claim[] claims = [new Claim(ClaimTypes.Name, signIn.Email)];
            string secret = _jwtOptions.SecurityKey ?? throw new Exception("Не указан секрет.");
            SymmetricSecurityKey key = new (Encoding.UTF8.GetBytes(secret));
            SigningCredentials creds = new (key, SecurityAlgorithms.HmacSha256);
            DateTime expiry = DateTime.Now.AddDays(_jwtOptions.ExpiryInDays);
            JwtSecurityToken token = new(
                _jwtOptions.Issuer,
                _jwtOptions.Audience,
                claims,
                expires: expiry,
                signingCredentials: creds
            );

            return Ok(new SignInResponse { 
                Successful = true, Token = new JwtSecurityTokenHandler().WriteToken(token) 
            });
        }

        [HttpGet]
        public async Task<IActionResult> Confirm([FromQuery] string id, [FromQuery] string code)
        {
            User? user = await _signInManager.UserManager.FindByIdAsync(id!);

            if (user is null)
            {
                return BadRequest("Undefined user.");
            }

            IdentityResult confirmationResult = await _signInManager.UserManager.ConfirmEmailAsync(user, code);

            if (confirmationResult.Succeeded)
            {
                return Ok("Account confirmed.");
            }
            else
            {
                return BadRequest("Wrong confirmation code.");
            }
        }

        [HttpGet]
        public IActionResult GetHello()
        {
            return Ok($"Hello!");
        }

        [Authorize]
        [HttpGet]
        public IActionResult GetSecuredHello()
        {
            string claims = HttpContext.Request.Headers
                .Select(x => $"{x.Key}: {x.Value},\n")
                .Aggregate((x, y) => x + y);

            return Ok($"Secured Hello!\n{claims}");
        }
    }
}
