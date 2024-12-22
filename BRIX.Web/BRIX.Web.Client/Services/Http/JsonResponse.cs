using BRIX.GameService.Contracts.Common;
using System.Net;

namespace BRIX.Web.Client.Services.Http
{
    public class JsonResponse<T> where T : class
    {
        public T? Payload { get; set; }

        public ProblemDetalization? Errors { get; set; }

        public string RawContent { get; set; } = string.Empty;

        public HttpStatusCode HttpStatusCode { get; set; }
    }
}
