using Newtonsoft.Json;

namespace BRIX.Web.Client.Services.Http
{
    public static class HttpContentExtensions
    {
        public static async Task<T> ReadAsJsonAsync<T>(
            this HttpContent content,
            JsonSerializerSettings? jsonSerializerSettings = null)
        {
            if (jsonSerializerSettings == null)
            {
                jsonSerializerSettings = new JsonSerializerSettings
                {
                    TypeNameHandling = TypeNameHandling.Auto,
                    Formatting = Formatting.Indented,
                    ReferenceLoopHandling = ReferenceLoopHandling.Ignore,
                    DateParseHandling = DateParseHandling.None
                };
            }

            string json = await content.ReadAsStringAsync();
            T value = JsonConvert.DeserializeObject<T>(json, jsonSerializerSettings) ?? default!;

            return value;
        }

        public static async Task<TResponse> PostAsJsonAsync<TRequest, TResponse>(
            this HttpClient httpClient, 
            string uri, 
            TRequest request)
        {
            try
            {
                HttpResponseMessage response = await httpClient.PostAsync(uri, request, HttpUtility.JsonFormatter);
                string responseString = await response.Content.ReadAsStringAsync();

                return await response.Content.ReadAsJsonAsync<TResponse>();
            }
            catch(Exception ex)
            {
                string msg = ex.Message;

                return default!;
            }
        }

        public static async Task<TResponse> GetAsJsonAsync<TResponse>(this HttpClient httpClient, string uri)
        {
            HttpResponseMessage response = await httpClient.GetAsync(uri);

            return await response.Content.ReadAsJsonAsync<TResponse>();
        }
    }
}
