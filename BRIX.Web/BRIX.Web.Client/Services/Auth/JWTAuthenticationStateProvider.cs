using Blazored.LocalStorage;
using BRIX.Web.Shared.JWT;
using Microsoft.AspNetCore.Components.Authorization;
using System.Security.Claims;

namespace BRIX.Web.Client.Services.Auth
{
    public class JWTAuthenticationStateProvider(ILocalStorageService localStorage) : AuthenticationStateProvider
    {
        public override async Task<AuthenticationState> GetAuthenticationStateAsync()
        {
            try
            {
                AuthenticationState notAuthenticatedState = new (new ClaimsPrincipal(new ClaimsIdentity()));
                string? savedToken = await localStorage.GetItemAsync<string>(LocalStorageKeys.AuthToken);

                if (string.IsNullOrWhiteSpace(savedToken))
                {
                    return notAuthenticatedState;
                }

                DateTime tokenExpirationDate = JWTHelper.GetExpirationDateFromJwt(savedToken);

                if (tokenExpirationDate <= DateTime.UtcNow)
                {
                    // TODO: Место для работы с Refresh-токеном.
                    await localStorage.SetItemAsStringAsync(LocalStorageKeys.AuthToken, string.Empty);
                    MarkUserAsLoggedOut();

                    return notAuthenticatedState;
                }


                return new AuthenticationState(
                    new ClaimsPrincipal(
                        new ClaimsIdentity(JWTHelper.ParseClaimsFromJwt(savedToken), "jwt")
                    )
                );
            }
            catch (InvalidOperationException)
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
