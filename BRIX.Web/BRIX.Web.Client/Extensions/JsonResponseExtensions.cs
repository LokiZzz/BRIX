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

        public static OperationResult ToOperationResult(this JsonResponse jsonResponse)
        {
            OperationResult result = new()
            {
                Successfull = jsonResponse.HttpStatusCode == System.Net.HttpStatusCode.OK,
            };

            if (jsonResponse.ProblemDetalization is not null)
            {
                IEnumerable<string> errorCodes = jsonResponse.ProblemDetalization.Problems.Select(x => x.Code);
                string[] errors = errorCodes.Select(x => 
                        Resource.ResourceManager.GetString(x.ToResourceKey()) ?? Resource.Problem_UnknownError)
                    .Cast<string>()
                    .ToArray();

                result.Errors = errors;
            }

            return result;
        }
    }
}
