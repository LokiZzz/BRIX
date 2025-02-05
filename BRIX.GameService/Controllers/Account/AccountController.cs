using BRIX.GameService.Contracts.Account;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using BRIX.GameService.Services.Account;
using Microsoft.Extensions.Options;
using BRIX.GameService.Options;
using BRIX.GameService.Services.Utility;

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
            await _accountService.SignUpAsync(model);

            return Ok();
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
            await _accountService.ForgotPassword(email);

            return Ok();
        }

        [HttpPost]
        public async Task<IActionResult> ResetPassword([FromBody] ResetPasswordRequest request)
        {
            await _accountService.ResetPassword(request.UserId, request.Password, request.Token);

            return Ok();
        }

        [HttpGet]
        public async Task<IActionResult> ResendConfirmationEmail([FromQuery] string email)
        {
            return Ok(await _accountService.ResendConfirmationEmail(email));
        }

        [Authorize]
        [HttpGet]
        public IActionResult Check() => Ok();

        [HttpGet]
        public IActionResult GetProblem()
        {
            throw new ProblemException("ProblemCode", "Error in action-method.");
        }
    }
}
