namespace BRIX.GameService.Contracts.Account
{
    public class ResendConfirmationEmailResponse
    {
        public bool Success { get; set; }

        public int RetryAfterInSeconds { get; set; }
    }
}
