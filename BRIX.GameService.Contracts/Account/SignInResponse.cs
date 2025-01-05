namespace BRIX.GameService.Contracts.Account
{
    public class SignInResponse
    {
        public bool NeedToConfirmAccount { get; set; }

        public string Token { get; set; } = string.Empty;
    }
}
