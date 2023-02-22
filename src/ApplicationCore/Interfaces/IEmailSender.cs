using System.Net.Mail;

namespace Alanyang.DotNetEmail.ApplicationCore.Interfaces;

public interface IEmailSender
{
    void SendEmail(
        string recipient,
        string subject,
        string body,
        bool isBodyHtml = true);
    void SendEmail(
        string recipient,
        string subject,
        string body,
        IEnumerable<string>? cc,
        IEnumerable<string>? bcc,
        IEnumerable<Attachment>? attachments,
        bool isBodyHtml = true);
    void SendEmail(MailMessage mailMessage);
    Task SendEmailAsync(
        string recipient,
        string subject,
        string body,
        bool isBodyHtml = true);
    Task SendEmailAsync(
        string recipient,
        string subject,
        string body,
        IEnumerable<string>? cc,
        IEnumerable<string>? bcc,
        IEnumerable<Attachment>? attachments,
        bool isBodyHtml = true);
    Task SendEmailAsync(MailMessage mailMessage);
}