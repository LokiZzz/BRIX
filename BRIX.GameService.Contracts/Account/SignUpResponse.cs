namespace BRIX.GameService.Contracts.Account
{
    public class SignUpResponse
    {
        public bool Successful { get; set; }

        public List<string> Errors { get; set; } = [];
    }
}
