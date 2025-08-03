using Blazored.LocalStorage;
using Microsoft.AspNetCore.Components.Authorization;
using System.Net.Http.Headers;
using BRIX.GameService.Contracts.Account;
using BRIX.Web.Client.Services.Http;
using Microsoft.AspNetCore.WebUtilities;
using System.Net;
using BRIX.Web.Client.Models.Common;
using BRIX.Web.Client.Extensions;
using BRIX.Web.Client.Models.Account;
using BRIX.Web.Client.Options;
using Microsoft.Extensions.Options;
using BRIX.Web.Problems;
using BRIX.Web.Client.Services.UI;

namespace BRIX.Web.Client.Services.Auth
{
    public class AuthService : IAuthService
    {
        private readonly AuthenticationStateProvider _authStateProvider;
        private readonly ILocalStorageService _localStorage;
        private readonly HttpClientBuilder _httpClientBuilder;
        private readonly ModalService _modalService;

        private readonly string _baseAddress;

        public AuthService(
            AuthenticationStateProvider authStateProvider,
            ILocalStorageService localStorage,
            HttpClientBuilder httpClient,
            IOptions<GameServiceOptions> gameServiceOptions,
            ModalService modalService)
        {
            _authStateProvider = authStateProvider;
            _localStorage = localStorage;
            _httpClientBuilder = httpClient;
            _modalService = modalService;
            _baseAddress = gameServiceOptions.Value.ServiceAddress;
        }

        public async Task<OperationResult> SignUp(SignUpRequest model)
        {
            _modalService.IsBusy = true;

            using HttpClient http = await _httpClientBuilder.CreateAsync(_baseAddress);

            JsonResponse response = await http.PostJsonAsync("api/account/signup", model);
            OperationResult result = response.ToOperationResult();

            _modalService.PushErrors(result.Errors);
            _modalService.IsBusy = false;

            return result;
        }

        public async Task<SignInResult> SignIn(SignInRequest model)
        {
            _modalService.IsBusy = true;

            using HttpClient http = await _httpClientBuilder.CreateAsync(_baseAddress);

            JsonResponse<SignInResponse> response = await http.PostJsonAsync<SignInRequest, SignInResponse>(
                "api/account/signin", 
                model
            );

            if (!string.IsNullOrEmpty(response.Payload?.Token))
            {
                await _localStorage.SetItemAsStringAsync(LocalStorageKeys.AuthToken, response.Payload.Token);
                ((JWTAuthenticationStateProvider)_authStateProvider).MarkUserAsAuthenticated(model.Email);
            }

            SignInResult result = response.ToOperationResult<SignInResult>();
            result.NeedToConfirmAccount = response.HasProblem(ProblemCodes.Account.NeedToConfirmAccount);

            if (!result.NeedToConfirmAccount)
            {
                _modalService.PushErrors(result.Errors);
            }

            _modalService.IsBusy = false;

            return result;
        }

        public async Task SignOut()
        {
            await _localStorage.SetItemAsStringAsync(LocalStorageKeys.AuthToken, string.Empty);
            ((JWTAuthenticationStateProvider)_authStateProvider).MarkUserAsLoggedOut();
        }

        public async Task<OperationResult> ForgotPassword(string email)
        {
            _modalService.IsBusy = true;

            string uri = QueryHelpers.AddQueryString("api/account/forgotpassword", "email", email);

            using HttpClient http = await _httpClientBuilder.CreateAsync(_baseAddress);

            JsonResponse response = await http.GetJsonAsync(uri);
            OperationResult result = response.ToOperationResult();

            _modalService.PushErrors(result.Errors);
            _modalService.IsBusy = false;

            return result;
        }

        public async Task<OperationResult> ResetPassword(string userId, string newPassword, string token)
        {
            _modalService.IsBusy = true;

            using HttpClient http = await _httpClientBuilder.CreateAsync(_baseAddress);

            JsonResponse response = await http.PostJsonAsync(
                "api/account/resetpassword", 
                new ResetPasswordRequest 
                { 
                    UserId = userId,
                    Password = newPassword,
                    Token = token
                }
            );

            OperationResult result = response.ToOperationResult();

            _modalService.PushErrors(result.Errors);
            _modalService.IsBusy = false;

            return result;
        }

        public async Task<ResendEmailConfirmationResult> ResendConfirmationEmail(string email)
        {
            _modalService.IsBusy = true;

            string uri = QueryHelpers.AddQueryString("api/account/resendconfirmationemail", "email", email);

            using HttpClient http = await _httpClientBuilder.CreateAsync(_baseAddress);

            JsonResponse<ResendConfirmationEmailResponse> response = await http
                .GetJsonAsync<ResendConfirmationEmailResponse>(uri);
            OperationResult result = response.ToOperationResult();

            _modalService.PushErrors(result.Errors);
            _modalService.IsBusy = false;

            return new ResendEmailConfirmationResult()
            {
                Successfull = response.HttpStatusCode == HttpStatusCode.OK
                    && response.Payload?.EmailWasSent == true,
                Errors = response.ExtractErrors(),
                RetryAfterInSeconds = response.Payload?.RetryAfterInSeconds ?? 0
            };
        }
    }
}