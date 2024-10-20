﻿using BRIX.Entity.Users;
using BRIX.GameService.Contracts.Account;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using SignInResult = Microsoft.AspNetCore.Identity.SignInResult;

namespace BRIX.GameService.Controllers.Account
{
    [ApiController]
    [Route("api/[controller]/[action]")]
    public class AccountController : Controller
    {
        private static readonly UserModel LoggedOutUser = new () { IsAuthenticated = false };

        private readonly UserManager<User> _userManager;
        private readonly IConfiguration _configuration;
        private readonly SignInManager<User> _signInManager;

        public AccountController(
            UserManager<User> userManager,
            IConfiguration configuration,
            SignInManager<User> signInManager)
        {
            _userManager = userManager;
            _configuration = configuration;
            _signInManager = signInManager;
        }

        [HttpPost]
        public async Task<IActionResult> Register([FromBody] SignUpRequest model)
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

            return Ok(new SignUpResponse { Successful = true });
        }

        [HttpPost]
        public async Task<IActionResult> Login([FromBody] SignInRequest signIn)
        {
            SignInResult result = await _signInManager.PasswordSignInAsync(
                signIn.Email, 
                signIn.Password, 
                signIn.RememberMe, 
                lockoutOnFailure: false
            );

            if (!result.Succeeded)
            {
                return BadRequest(new SignInResponse { Successful = false, Error = "Username and password are invalid." });
            }

            Claim[] claims = [new Claim(ClaimTypes.Name, signIn.Email)];

            string secret = _configuration["JwtSecurityKey"] ?? throw new Exception("Не указан секрет.");
            SymmetricSecurityKey key = new(Encoding.UTF8.GetBytes(secret));
            SigningCredentials creds = new(key, SecurityAlgorithms.HmacSha256);
            DateTime expiry = DateTime.Now.AddDays(Convert.ToInt32(_configuration["JwtExpiryInDays"]));

            JwtSecurityToken token = new(
                _configuration["JwtIssuer"],
                _configuration["JwtAudience"],
                claims,
                expires: expiry,
                signingCredentials: creds
            );

            return Ok(new SignInResponse { Successful = true, Token = new JwtSecurityTokenHandler().WriteToken(token) });
        }

        [HttpGet]
        public async Task<IActionResult> ConfirmEmail([FromQuery] string email, [FromQuery] string confirmationCode)
        {
            User? user = await _signInManager.UserManager.FindByEmailAsync(email!);

            if (user is null)
            {
                return BadRequest("Undefined user.");
            }

            IdentityResult confirmationResult = await _signInManager.UserManager.ConfirmEmailAsync(user, confirmationCode);

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
        public async Task<IActionResult> GetConfirmationCode([FromQuery] string email)
        {
            User? user = await _signInManager.UserManager.FindByEmailAsync(email!);

            if (user is null)
            {
                return BadRequest();
            }

            string userId = await _signInManager.UserManager.GetUserIdAsync(user);
            string code = await _signInManager.UserManager.GenerateEmailConfirmationTokenAsync(user);

            return Ok(code);
        }

        [Authorize]
        [HttpGet]
        public IActionResult GetSecuredHello()
        {
            return Ok("Hello!");
        }
    }
}
