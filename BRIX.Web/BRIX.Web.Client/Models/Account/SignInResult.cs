using BRIX.Web.Client.Models.Common;

namespace BRIX.Web.Client.Models.Account
{
    public class SignInResult : OperationResult
    {
        public bool NeedToConfirmAccount { get; set; }
    }
}
