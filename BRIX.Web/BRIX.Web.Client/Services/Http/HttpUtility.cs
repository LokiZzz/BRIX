using Newtonsoft.Json;
using System.Net.Http.Formatting;

namespace BRIX.Web.Client.Services.Http
{
    public class HttpUtility
    {
        public static JsonMediaTypeFormatter JsonFormatter => new()
        {
            SerializerSettings = new JsonSerializerSettings
            {
                Formatting = Formatting.Indented,
                ReferenceLoopHandling = ReferenceLoopHandling.Ignore,
                DateParseHandling = DateParseHandling.None,
                TypeNameHandling = TypeNameHandling.Auto,
            }
        };
    }
}
