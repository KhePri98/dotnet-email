using System.Net.Mail;

namespace Alanyang.DotNetEmail.ApplicationCore.Interfaces;

public interface IEmailSendingQueue
{
    Task AddEmailAsync(
        string recipient,
        string subject,
        string body,
        IEnumerable<string>? cc,
        IEnumerable<string>? bcc,
        IEnumerable<Attachment>? attachments);
    Task SendEmailAsync();
    Task DeleteAlreadySentAsync();
}