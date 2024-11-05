namespace BRIX.GameService.Services.Mail
{
    public class SMTPOptions
    {
        public const string SMTP = nameof(SMTP);

        public string Server { get; set; } = string.Empty;
        public int Port { get; set; }
        public string SenderName { get; set; } = string.Empty;
        public string SenderAddress { get; set; } = string.Empty;
        public string Username { get; set; } = string.Empty;
        public string Password { get; set; } = string.Empty;
    }
}
