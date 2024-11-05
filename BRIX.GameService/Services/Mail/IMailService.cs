
namespace BRIX.GameService.Services.Mail
{
    public interface IMailService
    {
        Task SendAsync(string[] toEmails, string subject, string messageText);
    }
}