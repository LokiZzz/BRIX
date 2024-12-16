using Newtonsoft.Json;
using System.Net.Http.Json;
using System.Text.Json.Nodes;
using System.Text;
using System.Net.Mime;
using System.Net.Http.Formatting;

namespace BRIX.Web.Client.Services.Http
{
    public static class HttpContentExtensions
    {
        private static JsonMediaTypeFormatter _jsonFormatter => new()
        {
            SerializerSettings = new JsonSerializerSettings
            {
                Formatting = Formatting.Indented,
                ReferenceLoopHandling = ReferenceLoopHandling.Ignore,
                DateParseHandling = DateParseHandling.None,
                TypeNameHandling = TypeNameHandling.Auto,
            }
        };

        private static readonly JsonSerializerSettings _defaultJsonSettings = new()
        {
            TypeNameHandling = TypeNameHandling.Auto,
            Formatting = Formatting.Indented,
            ReferenceLoopHandling = ReferenceLoopHandling.Ignore,
            DateParseHandling = DateParseHandling.None
        };

        public static async Task<T> ReadJsonAsync<T>(this HttpResponseMessage response)
        {
            response.EnsureSuccessStatusCode();
            string json = await response.Content.ReadAsStringAsync();
            T value = JsonConvert.DeserializeObject<T>(json, _defaultJsonSettings) ?? default!;

            return value;
        }

        public static async Task<TResponse> GetAsJsonAsync<TResponse>(this HttpClient httpClient, string uri)
        {
            HttpResponseMessage response = await httpClient.GetAsync(uri);

            return await response.ReadJsonAsync<TResponse>();
        }

        public static async Task<TResponse> PostAsJsonAsync<TRequest, TResponse>(
            this HttpClient httpClient,
            string uri,
            TRequest request)
        {
            HttpResponseMessage response = await httpClient.PostAsync(uri, request, _jsonFormatter);

            return await response.ReadJsonAsync<TResponse>();
        }

        public static async Task<TResponse> PutJsonAsync<TRequest, TResponse>(
            this HttpClient httpClient,
            string uri,
            TRequest request)
        {
            HttpResponseMessage response = await httpClient.PutAsync(uri, request, _jsonFormatter);

            return await response.ReadJsonAsync<TResponse>();
        }

        public static async Task<TResponse> DeleteJsonAsync<TResponse>(this HttpClient httpClient, string uri)
        {
            HttpResponseMessage response = await httpClient.DeleteAsync(uri);

            return await response.ReadJsonAsync<TResponse>();
        }
    }
}
