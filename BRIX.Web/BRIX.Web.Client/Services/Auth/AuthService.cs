﻿using Blazored.LocalStorage;
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
using BRIX.Web.Problems;

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
            result.NeedToConfirmAccount = response.ProblemDetalization?.Problems
                .Any(x => x.Code == ProblemCodes.Account.NeedToConfirmAccount) == true;

            return result;
        }

        public async Task SignOut()
        {
            await _localStorage.RemoveItemAsync("authToken");
            ((JWTAuthenticationStateProvider)_authStateProvider).MarkUserAsLoggedOut();
            _httpClient.DefaultRequestHeaders.Authorization = null;
        }

        public async Task<OperationResult> ForgotPassword(string email)
        {
            string uri = QueryHelpers.AddQueryString("api/account/forgotpassword", "email", email);
            JsonResponse response = await _httpClient.GetJsonAsync(uri);

            return response.ToOperationResult();
        }

        public async Task<OperationResult> ResetPassword(string userId, string newPassword, string token)
        {
            JsonResponse response = await _httpClient.PostJsonAsync(
                "api/account/resetpassword", 
                new ResetPasswordRequest 
                { 
                    UserId = userId,
                    Password = newPassword,
                    Token = token
                }
            );

            return response.ToOperationResult();
        }

        public async Task<ResendEmailConfirmationResult> ResendConfirmationEmail(string email)
        {
            string uri = QueryHelpers.AddQueryString("api/account/resendconfirmationemail", "email", email);

            JsonResponse<ResendConfirmationEmailResponse> response = await _httpClient
                .GetJsonAsync<ResendConfirmationEmailResponse>(uri);

            ResendEmailConfirmationResult result = response.ToOperationResult<ResendEmailConfirmationResult>();
            result.RetryAfterInSeconds = response.Payload?.RetryAfterInSeconds ?? 0;
            
            return result;
        }
    }
}