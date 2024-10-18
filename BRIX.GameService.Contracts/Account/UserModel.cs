namespace BRIX.GameService.Contracts.Account
{
    public class UserModel
    {
        public string Email { get; set; } = string.Empty;

        public bool IsAuthenticated { get; set; }
    }
}
