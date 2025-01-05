using BRIX.GameService.Contracts.Common;
using System.Net;

namespace BRIX.Web.Client.Services.Http
{
    public class JsonResponse<T> : JsonResponse where T : class
    {
        public T? Payload { get; set; }

        public bool HasProblem(string problemCode)
        {
            return ProblemDetalization?.Problems.Any(x => x.Code == problemCode) == true;
        }
    }

    public class JsonResponse
    {
        public ProblemDetalization? ProblemDetalization { get; set; }

        public string RawContent { get; set; } = string.Empty;

        public HttpStatusCode HttpStatusCode { get; set; }
    }
}
