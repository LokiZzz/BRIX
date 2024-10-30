using Blazored.LocalStorage;
using Microsoft.AspNetCore.Components.Authorization;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using System.Text.Json;
using System.Text;
using BRIX.GameService.Contracts.Account;

namespace BRIX.Web.Client.Services
{
    public class AuthService(
        HttpClient httpClient,
        AuthenticationStateProvider authenticationStateProvider,
        ILocalStorageService localStorage) : IAuthService
    {
        private readonly HttpClient _httpClient = httpClient;
        private readonly AuthenticationStateProvider _authenticationStateProvider = authenticationStateProvider;
        private readonly ILocalStorageService _localStorage = localStorage;

        private static readonly JsonSerializerOptions _jsonOptions = new() { PropertyNameCaseInsensitive = true };

        public async Task<SignUpResponse> SignUp(SignUpRequest model)
        {
            HttpResponseMessage result = await _httpClient.PostAsJsonAsync("api/account/signup", model);

            return await result.Content.ReadFromJsonAsync<SignUpResponse>() ?? new();
        }

        public async Task<SignInResponse> SignIn(SignInRequest model)
        {
            string signInAsJson = JsonSerializer.Serialize(model);
            HttpResponseMessage response = await _httpClient.PostAsync(
                "api/account/signup",
                new StringContent(signInAsJson, Encoding.UTF8, "application/json")
            );
            SignInResponse? signInResult = JsonSerializer.Deserialize<SignInResponse>(
                await response.Content.ReadAsStringAsync(),
                _jsonOptions
            );

            if (signInResult == null || response?.IsSuccessStatusCode != true)
            {
                return signInResult ?? new() { Error = "Неожиданный формат ответа от api/account/signin." };
            }

            await _localStorage.SetItemAsync("authToken", signInResult?.Token);
            ((JWTAuthenticationStateProvider)_authenticationStateProvider).MarkUserAsAuthenticated(model.Email);
            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("bearer", signInResult?.Token);

            return signInResult!;
        }

        public async Task SignOut()
        {
            await _localStorage.RemoveItemAsync("authToken");
            ((JWTAuthenticationStateProvider)_authenticationStateProvider).MarkUserAsLoggedOut();
            _httpClient.DefaultRequestHeaders.Authorization = null;
        }
    }
}