using Blazored.LocalStorage;
using Microsoft.AspNetCore.Components.Authorization;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using System.Text.Json;
using System.Text;
using BRIX.GameService.Contracts.Account;
using BRIX.Web.Client.Services.Http;
using Newtonsoft.Json.Linq;
using Microsoft.AspNetCore.WebUtilities;
using Microsoft.Extensions.Options;
using BRIX.Web.Client.Options;
using System.Net.Http;

namespace BRIX.Web.Client.Services.Auth
{
    public class AuthService : IAuthService
    {
        private readonly HttpClient _httpClient;
        private readonly AuthenticationStateProvider _authenticationStateProvider;
        private readonly ILocalStorageService _localStorage;
        private readonly GameServiceOptions _gameServiceOptions;

        private static readonly JsonSerializerOptions _jsonOptions = new() { PropertyNameCaseInsensitive = true };

        public AuthService(
            HttpClient httpClient,
            AuthenticationStateProvider authenticationStateProvider,
            ILocalStorageService localStorage,
            IOptions<GameServiceOptions> gameServiceOptions)
        {
            _authenticationStateProvider = authenticationStateProvider;
            _localStorage = localStorage;
            _gameServiceOptions = gameServiceOptions.Value;
            _httpClient = httpClient;
            _httpClient.BaseAddress = new Uri(_gameServiceOptions.ServiceAddress);
        }

        public async Task<SignUpResponse> SignUp(SignUpRequest model)
        {
            SignUpResponse result = await _httpClient.PostAsJsonAsync<SignUpRequest, SignUpResponse>(
                "api/account/signup",
                model
            );

            return result;
        }

        public async Task<SignInResponse> SignIn(SignInRequest model)
        {
            HttpResponseMessage response = await _httpClient.PostAsJsonAsync("api/account/signin", model);
            SignInResponse? signInResult = await response.Content.ReadFromJsonAsync<SignInResponse>(_jsonOptions);

            if (signInResult == null || response?.IsSuccessStatusCode != true)
            {
                return signInResult ?? new() { Error = "Неожиданный формат ответа от api/account/signin." };
            }

            if (signInResult?.Successful == true)
            {
                await _localStorage.SetItemAsync("authToken", signInResult?.Token);
                ((JWTAuthenticationStateProvider)_authenticationStateProvider).MarkUserAsAuthenticated(model.Email);
                _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("bearer", signInResult?.Token);
            }

            return signInResult!;
        }

        public async Task SignOut()
        {
            await _localStorage.RemoveItemAsync("authToken");
            ((JWTAuthenticationStateProvider)_authenticationStateProvider).MarkUserAsLoggedOut();
            _httpClient.DefaultRequestHeaders.Authorization = null;
        }

        public async Task<bool> ForgotPassword(string email)
        {
            Dictionary<string, string?> queryParams = new() { { "email", email } };
            string uri = new(QueryHelpers.AddQueryString("api/account/forgotpassword", queryParams));

            ForgotPasswordResponse response = await _httpClient.GetAsJsonAsync<ForgotPasswordResponse>(uri);

            return response.Success;
        }

        public async Task<bool> ResetPassword(string userId, string newPassword, string token)
        {
            ResetPasswordResponse response = await _httpClient.PostAsJsonAsync<ResetPasswordRequest, ResetPasswordResponse>(
                "api/account/resetpassword", 
                new ResetPasswordRequest 
                { 
                    UserId = userId,
                    Password = newPassword,
                    Token = token
                }
            );

            return response.Success;
        }

        public async Task<ResendConfirmationEmailResponse> ResendConfirmationEmail(string email)
        {
            Dictionary<string, string?> queryParams = new() { { "email", email } };
            string uri = new(QueryHelpers.AddQueryString("api/account/resendconfirmationemail", queryParams));

            ResendConfirmationEmailResponse response = await _httpClient
                .GetAsJsonAsync<ResendConfirmationEmailResponse>(uri);

            return response;
        }
    }
}