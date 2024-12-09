namespace BRIX.GameService.Contracts.Account
{
    public class SignInResponse
    {
        public bool Successful { get; set; }

        public bool NeedToConfirmAccount { get; set; }

        public string Error { get; set; } = string.Empty;

        public string Token { get; set; } = string.Empty;
    }
}
