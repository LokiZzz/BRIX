namespace BRIX.GameService.Contracts.Account
{
    public class SignUpResponse
    {
        public bool Successful { get; set; }

        public IEnumerable<string> Errors { get; set; } = [];
    }
}
