using Blazored.LocalStorage;
using System.Net.Http.Headers;

namespace BRIX.Web.Client.Services.Http
{
    public class HttpClientBuilder(ILocalStorageService localStorage)
    {
        public async Task<HttpClient> CreateAsync(string baseUrl)
        {
            HttpClient client = new () { BaseAddress = new Uri(baseUrl) };
            string? savedToken = await localStorage.GetItemAsync<string>(LocalStorageKeys.AuthToken);

            if (savedToken != null) 
            {
                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("bearer", savedToken);
            }

            return client;
        }
    }
}
