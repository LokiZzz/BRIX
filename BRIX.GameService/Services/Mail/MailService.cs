using BRIX.GameService.Options;
using MailKit.Net.Smtp;
using Microsoft.Extensions.Options;
using MimeKit;
using MimeKit.Text;

namespace BRIX.GameService.Services.Mail
{
    public class MailService(IOptions<SMTPOptions> options) : IMailService
    {
        private readonly SMTPOptions _options = options?.Value
            ?? throw new ArgumentNullException(nameof(options));

        public async Task SendAsync(string[] toEmails, string subject, string messageText)
        {
            MimeMessage message = new();
            message.From.Add(new MailboxAddress(_options.SenderName, _options.SenderAddress));
            message.To.AddRange(toEmails.Select(x => new MailboxAddress("", x)));
            message.Subject = subject;
            message.Body = new TextPart(TextFormat.Plain) { Text = messageText };

            using var client = new SmtpClient();
            await client.ConnectAsync(_options.Server, _options.Port, MailKit.Security.SecureSocketOptions.StartTls);
            await client.AuthenticateAsync(_options.Username, _options.Password);
            await client.SendAsync(message);
            await client.DisconnectAsync(true);
        }
    }
}
