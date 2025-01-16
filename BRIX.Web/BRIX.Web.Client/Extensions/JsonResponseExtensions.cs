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
            return new T()
            {
                Successfull = jsonResponse.HttpStatusCode == System.Net.HttpStatusCode.OK,
                Errors = jsonResponse.ExtractErrors()
            };
        }

        public static OperationResult ToOperationResult(this JsonResponse jsonResponse) =>
            jsonResponse.ToOperationResult<OperationResult>();

        public static List<string> ExtractErrors(this JsonResponse jsonResponse)
        {
            if (jsonResponse.ProblemDetalization is not null)
            {
                return jsonResponse.ProblemDetalization.Problems
                    .Select(GetLocalizedErrorMessage)
                    .ToList();
            }
            else
            {
                return [];
            }
        }

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
