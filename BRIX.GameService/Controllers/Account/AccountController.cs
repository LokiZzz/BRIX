﻿using BRIX.GameService.Contracts.Account;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using BRIX.GameService.Services.Account;
using Microsoft.Extensions.Options;
using BRIX.GameService.Options;

namespace BRIX.GameService.Controllers.Account
{
    [ApiController]
    [Route("api/[controller]/[action]")]
    public class AccountController(
        IAccountService accountService,
        IOptions<ClientOptions> clientOptions) : Controller
    {
        private readonly IAccountService _accountService = accountService;
        private readonly ClientOptions _clientOptions = clientOptions?.Value
             ?? throw new ArgumentNullException(nameof(clientOptions));

        [HttpPost]
        public async Task<IActionResult> SignUp([FromBody] SignUpRequest model)
        {
            throw new Exception("Server exception.");

            return Ok(await _accountService.SignUp(model));
        }

        [HttpPost]
        public async Task<IActionResult> SignIn([FromBody] SignInRequest model)
        {
            return Ok(await _accountService.SignIn(model));
        }

        [HttpGet]
        public async Task<IActionResult> Confirm([FromQuery] string id, [FromQuery] string code)
        {
            string redirectUri = await _accountService.Confirm(id, code)
                ? _clientOptions.ConfirmOkRedirectAddress
                : _clientOptions.ConfirmFailedRedirectAddress;

            return Redirect(redirectUri);
        }

        [HttpGet]
        public async Task<IActionResult> ForgotPassword([FromQuery] string email)
        {
            ForgotPasswordResponse response = await _accountService.ForgotPassword(email);

            return Ok(response);
        }

        [HttpPost]
        public async Task<IActionResult> ResetPassword([FromBody] ResetPasswordRequest request)
        {
            ResetPasswordResponse response = await _accountService.ResetPassword(
                request.UserId, 
                request.Password, 
                request.Token
            );

            return Ok(response);
        }

        [HttpGet]
        public async Task<IActionResult> ResendConfirmationEmail([FromQuery] string email)
        {
            ResendConfirmationEmailResponse result = await _accountService.ResendConfirmationEmail(email);

            return Ok(result);
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
