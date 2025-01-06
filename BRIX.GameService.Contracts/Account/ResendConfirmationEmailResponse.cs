namespace BRIX.GameService.Contracts.Account
{
    public class ResendConfirmationEmailResponse
    {
        public bool EmailWasSent { get; set; }

        public int RetryAfterInSeconds { get; set; }
    }
}
