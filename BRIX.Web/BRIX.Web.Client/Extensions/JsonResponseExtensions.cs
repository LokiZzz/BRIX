using BRIX.GameService.Contracts.Common;
using BRIX.Web.Client.Localization;
using BRIX.Web.Client.Models.Common;
using BRIX.Web.Client.Services.Http;

namespace BRIX.Web.Client.Extensions
{
    public static class JsonResponseExtensions
    {
        public static string ToResourceKey(this string problemCode)
        {
            return $"Problem_{problemCode}";
        }

        public static T ToOperationResult<T>(this JsonResponse jsonResponse)
            where T : OperationResult, new()
        {
            T result = new()
            {
                Successfull = jsonResponse.HttpStatusCode == System.Net.HttpStatusCode.OK
            };

            if (jsonResponse.ProblemDetalization is not null)
            {
                result.Errors = jsonResponse.ProblemDetalization.Problems
                    .Select(GetLocalizedErrorMessage)
                    .ToArray(); ;
            }

            return result;
        }

        public static OperationResult ToOperationResult(this JsonResponse jsonResponse) =>
            jsonResponse.ToOperationResult<OperationResult>();

        private static string GetLocalizedErrorMessage(Problem problem)
        {
            string? message = Resource.ResourceManager.GetString(problem.Code.ToResourceKey());

            if(message is not null)
            {
                return message;
            }

            if(!string.IsNullOrEmpty(problem.Message))
            {
                return problem.Message;
            }

            return Resource.Problem_UnknownError;
        }
    }
}
