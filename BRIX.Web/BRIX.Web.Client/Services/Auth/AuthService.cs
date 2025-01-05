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
using System.Net.Http;
using BRIX.Web.Client.Options;
using Microsoft.Extensions.Options;

namespace BRIX.Web.Client.Services.Auth
{
    public class AuthService : IAuthService
    {
        private readonly AuthenticationStateProvider _authStateProvider;
        private readonly ILocalStorageService _localStorage;
        private readonly HttpClient _httpClient;

        public AuthService(
            AuthenticationStateProvider authStateProvider,
            ILocalStorageService localStorage,
            HttpClient httpClient,
            IOptions<GameServiceOptions> gameServiceOptions)
        {
            _authStateProvider = authStateProvider;
            _localStorage = localStorage;
            _httpClient = httpClient;
            _httpClient.BaseAddress = new Uri(gameServiceOptions.Value.ServiceAddress);
        }

        public async Task<OperationResult> SignUp(SignUpRequest model)
        {
            JsonResponse response = await _httpClient.PostJsonAsync("api/account/signup", model);

            return response.ToOperationResult();
        }

        public async Task<SignInResult> SignIn(SignInRequest model)
        {
            JsonResponse<SignInResponse> response = await _httpClient.PostJsonAsync<SignInRequest, SignInResponse>(
                "api/account/signin", 
                model
            );

            if (!string.IsNullOrEmpty(response.Payload?.Token))
            {
                await _localStorage.SetItemAsync("authToken", response.Payload.Token);
                ((JWTAuthenticationStateProvider)_authStateProvider).MarkUserAsAuthenticated(model.Email);
                _httpClient.DefaultRequestHeaders.Authorization =
                    new AuthenticationHeaderValue("bearer", response.Payload.Token);
            }

            SignInResult result = response.ToOperationResult<SignInResult>();
            result.NeedToConfirmAccount = response.Payload?.NeedToConfirmAccount == true;

            return result;
        }

        public async Task SignOut()
        {
            await _localStorage.RemoveItemAsync("authToken");
            ((JWTAuthenticationStateProvider)_authStateProvider).MarkUserAsLoggedOut();
            _httpClient.DefaultRequestHeaders.Authorization = null;
        }

        public async Task<bool> ForgotPassword(string email)
        {
            Dictionary<string, string?> queryParams = new() { { "email", email } };
            string uri = new(QueryHelpers.AddQueryString("api/account/forgotpassword", queryParams));

            var response = await _httpClient.GetJsonAsync<ForgotPasswordResponse>(uri);

            return response.HttpStatusCode == HttpStatusCode.OK;
        }

        public async Task<bool> ResetPassword(string userId, string newPassword, string token)
        {
            JsonResponse<ResetPasswordResponse> response = 
                await _httpClient.PostJsonAsync<ResetPasswordRequest, ResetPasswordResponse>(
                "api/account/resetpassword", 
                new ResetPasswordRequest 
                { 
                    UserId = userId,
                    Password = newPassword,
                    Token = token
                }
            );

            return response.HttpStatusCode == HttpStatusCode.OK;
        }

        public async Task<ResendConfirmationEmailResponse> ResendConfirmationEmail(string email)
        {
            Dictionary<string, string?> queryParams = new() { { "email", email } };
            string uri = new(QueryHelpers.AddQueryString("api/account/resendconfirmationemail", queryParams));

            JsonResponse<ResendConfirmationEmailResponse> response = await _httpClient
                .GetJsonAsync<ResendConfirmationEmailResponse>(uri);

            return response.Payload ?? throw new Exception("response.Payload is null");
        }
    }
}