using BRIX.Web.Client.Models.Common;

namespace BRIX.Web.Client.Models.Account
{
    public class ResendEmailConfirmationResult : OperationResult
    {
        public int RetryAfterInSeconds { get; set; }
    }
}
