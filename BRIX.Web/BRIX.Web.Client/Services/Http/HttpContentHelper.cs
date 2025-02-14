using Newtonsoft.Json;
using System.Net.Http.Formatting;
using BRIX.GameService.Contracts.Common;

namespace BRIX.Web.Client.Services.Http
{
    /// <summary>
    /// Хелпер для работы с JsonResponse и ProblemResponse.
    /// Имеются перегрузки для работы с запросами, для которых не нужен контракт ответа, когда
    /// достаточно только статус-кода и 
    /// </summary>
    public static class HttpContentExtensions
    {
        private static readonly JsonSerializerSettings _defaultJsonSettings = new()
        {
            TypeNameHandling = TypeNameHandling.Auto,
            Formatting = Formatting.Indented,
            ReferenceLoopHandling = ReferenceLoopHandling.Ignore,
            DateParseHandling = DateParseHandling.None
        };

        private static readonly JsonMediaTypeFormatter _jsonFormatter = new()
        {
            SerializerSettings = _defaultJsonSettings
        };

        public static async Task<JsonResponse<TResponse>> ReadJsonAsync<TResponse>(
            this HttpResponseMessage response)
            where TResponse : class
        {
            try
            {
                response.EnsureSuccessStatusCode();
                
                return await response.BuildJsonResponseAsync<TResponse>();
            }
            catch (HttpRequestException)
            {
                return await response.BuildJsonResponseAsync<TResponse>(isError: true);
            }
            catch
            {
                throw;
            }
        }

        public static async Task<JsonResponse> ReadJsonAsync(this HttpResponseMessage response)
        {
            return await response.ReadJsonAsync<object>();
        }

        public static async Task<JsonResponse<TResponse>> GetJsonAsync<TResponse>(
            this HttpClient httpClient,
            string uri)
            where TResponse : class
        {
            HttpResponseMessage response = await httpClient.GetAsync(uri);

            return await response.ReadJsonAsync<TResponse>();
        }

        public static async Task<JsonResponse> GetJsonAsync(this HttpClient httpClient, string uri)
        {
            HttpResponseMessage response = await httpClient.GetAsync(uri);

            return await response.ReadJsonAsync();
        }

        public static async Task<JsonResponse<TResponse>> PostJsonAsync<TRequest, TResponse>(
            this HttpClient httpClient,
            string uri,
            TRequest request)
            where TResponse : class
        {
            HttpResponseMessage response = await httpClient.PostAsync(uri, request, _jsonFormatter);

            return await response.ReadJsonAsync<TResponse>();
        }

        public static async Task<JsonResponse> PostJsonAsync<TRequest>(
            this HttpClient httpClient,
            string uri,
            TRequest request)
        {
            HttpResponseMessage response = await httpClient.PostAsync(uri, request, _jsonFormatter);

            return await response.ReadJsonAsync();
        }

        public static async Task<JsonResponse<TResponse>> PutJsonAsync<TRequest, TResponse>(
            this HttpClient httpClient,
            string uri,
            TRequest request)
            where TResponse : class
        {
            HttpResponseMessage response = await httpClient.PutAsync(uri, request, _jsonFormatter);

            return await response.ReadJsonAsync<TResponse>();
        }

        public static async Task<JsonResponse> PutJsonAsync<TRequest>(
            this HttpClient httpClient,
            string uri,
            TRequest request)
        {
            HttpResponseMessage response = await httpClient.PutAsync(uri, request, _jsonFormatter);

            return await response.ReadJsonAsync();
        }

        public static async Task<JsonResponse<TResponse>> DeleteJsonAsync<TResponse>(
            this HttpClient httpClient,
            string uri)
            where TResponse : class
        {
            HttpResponseMessage response = await httpClient.DeleteAsync(uri);

            return await response.ReadJsonAsync<TResponse>();
        }

        public static async Task<JsonResponse> DeleteJsonAsync(
            this HttpClient httpClient,
            string uri)
        {
            HttpResponseMessage response = await httpClient.DeleteAsync(uri);

            return await response.ReadJsonAsync();
        }

        private static async Task<JsonResponse<TResponse>> BuildJsonResponseAsync<TResponse>(
            this HttpResponseMessage response,
            bool isError = false)
            where TResponse : class
        {
            string rawContent = await response.Content.ReadAsStringAsync();

            JsonResponse<TResponse> jsonResponse = new() 
            { 
                HttpStatusCode = response.StatusCode,
                RawContent = rawContent
            };
            
            if(isError)
            {
                ProblemResponse problemResponse = 
                    JsonConvert.DeserializeObject<ProblemResponse>(rawContent, _defaultJsonSettings)
                    ?? throw new Exception("Ошибка десериализации ответа от сервера.");
                jsonResponse.ProblemDetalization = problemResponse.Detalization;
            }
            else
            {
                jsonResponse.Payload = JsonConvert.DeserializeObject<TResponse>(rawContent, _defaultJsonSettings);
            }

            return jsonResponse;
        }
    }
}