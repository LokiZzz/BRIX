using Blazored.LocalStorage;
using BRIX.Web.Shared;
using Microsoft.AspNetCore.Components.Authorization;
using System.Net.Http.Headers;
using System.Security.Claims;
using System.Text.Json;

namespace BRIX.Web.Client.Services
{
    public class JWTAuthenticationStateProvider(HttpClient httpClient, ILocalStorageService localStorage)
        : AuthenticationStateProvider
    {
        private readonly HttpClient _httpClient = httpClient;
        private readonly ILocalStorageService _localStorage = localStorage;

        public override async Task<AuthenticationState> GetAuthenticationStateAsync()
        {
            try
            {
                string? savedToken = await _localStorage.GetItemAsync<string>("authToken");

                if (string.IsNullOrWhiteSpace(savedToken))
                {
                    return new AuthenticationState(new ClaimsPrincipal(new ClaimsIdentity()));
                }

                _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("bearer", savedToken);

                return new AuthenticationState(
                    new ClaimsPrincipal(
                        new ClaimsIdentity(JWTHelper.ParseClaimsFromJwt(savedToken), "jwt")
                    )
                );
            }
            catch(InvalidOperationException)
            {
                // Попытка получить данные во время пререндеринга.
                return new AuthenticationState(new ClaimsPrincipal(new ClaimsIdentity()));
            }
        }

        public void MarkUserAsAuthenticated(string email)
        {
            ClaimsPrincipal authenticatedUser = new(new ClaimsIdentity([new Claim(ClaimTypes.Name, email)], "apiauth"));
            Task<AuthenticationState> authState = Task.FromResult(new AuthenticationState(authenticatedUser));
            NotifyAuthenticationStateChanged(authState);
        }

        public void MarkUserAsLoggedOut()
        {
            ClaimsPrincipal anonymousUser = new(new ClaimsIdentity());
            Task<AuthenticationState> authState = Task.FromResult(new AuthenticationState(anonymousUser));
            NotifyAuthenticationStateChanged(authState);
        }
    }
}
