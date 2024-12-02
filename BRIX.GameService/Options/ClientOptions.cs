namespace BRIX.GameService.Options
{
    public class ClientOptions
    {
        public const string Client = nameof(Client);

        public string ClientAddress { get; set; } = string.Empty;
        public string ConfirmOkRedirectAddress { get; set; } = string.Empty;
        public string ConfirmFailedRedirectAddress { get; set; } = string.Empty;
        public string ResetPasswordAddress { get; set; } = string.Empty;
    }
}
