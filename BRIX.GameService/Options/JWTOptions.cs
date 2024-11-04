namespace BRIX.GameService.Options
{
    public class JWTOptions
    {
        public const string JWT = nameof(JWT);

        public string Issuer { get; set; } = string.Empty;
        public string Audience { get; set; } = string.Empty;
        public string ExpiryInDays { get; set; } = string.Empty;
        public string SecurityKey { get; set; } = string.Empty;
    }
}
