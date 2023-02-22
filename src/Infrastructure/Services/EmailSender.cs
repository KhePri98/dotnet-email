using System.Net;
using System.Net.Mail;
using Alanyang.DotNetEmail.ApplicationCore.Interfaces;
using Alanyang.DotNetEmail.Infrastructure.Options;
using Microsoft.Extensions.Options;

namespace Alanyang.DotNetEmail.Infrastructure.Services;

public class EmailSender : IEmailSender
{
    private readonly EmailOptions _emailOptions;

    public EmailSender(IOptions<EmailOptions> options)
    {
        _emailOptions = options.Value;
    }

    public void SendEmail(
        string recipient,
        string subject,
        string body,
        bool isBodyHtml = true)
    {
        using var mailMessage = CreateDefaultMessage();
        mailMessage.To.Add(recipient);
        mailMessage.Subject = subject;
        mailMessage.Body = body;
        mailMessage.IsBodyHtml = isBodyHtml;
        Send(mailMessage);
    }

    public void SendEmail(
        string recipient,
        string subject,
        string body,
        IEnumerable<string>? cc,
        IEnumerable<string>? bcc,
        IEnumerable<Attachment>? attachments,
        bool isBodyHtml = true)
    {
        using var mailMessage = CreateDefaultMessage();
        mailMessage.To.Add(recipient);
        mailMessage.Subject = subject;
        mailMessage.Body = body;
        if (cc != null)
        {
            foreach (var ccAddress in cc)
            {
                mailMessage.CC.Add(ccAddress);
            }
        }
        if (bcc != null)
        {
            foreach (var bccAddress in bcc)
            {
                mailMessage.Bcc.Add(bccAddress);
            }
        }
        if (attachments != null)
        {
            foreach (var attachment in attachments)
            {
                mailMessage.Attachments.Add(attachment);
            }
        }
        mailMessage.IsBodyHtml = isBodyHtml;
        Send(mailMessage);
    }

    public void SendEmail(MailMessage mailMessage)
    {
        Send(mailMessage);
    }

    public async Task SendEmailAsync(
        string recipient,
        string subject,
        string body,
        bool isBodyHtml = true)
    {
        using var mailMessage = CreateDefaultMessage();
        mailMessage.To.Add(recipient);
        mailMessage.Subject = subject;
        mailMessage.Body = body;
        mailMessage.IsBodyHtml = isBodyHtml;
        await SendAsync(mailMessage);
    }

    public async Task SendEmailAsync(
        string recipient,
        string subject,
        string body,
        IEnumerable<string>? cc,
        IEnumerable<string>? bcc,
        IEnumerable<Attachment>? attachments,
        bool isBodyHtml = true)
    {
        using var mailMessage = CreateDefaultMessage();
        mailMessage.To.Add(recipient);
        mailMessage.Subject = subject;
        mailMessage.Body = body;
        if (cc != null)
        {
            foreach (var ccAddress in cc)
            {
                mailMessage.CC.Add(ccAddress);
            }
        }
        if (bcc != null)
        {
            foreach (var bccAddress in bcc)
            {
                mailMessage.Bcc.Add(bccAddress);
            }
        }
        if (attachments != null)
        {
            foreach (var attachment in attachments)
            {
                mailMessage.Attachments.Add(attachment);
            }
        }
        mailMessage.IsBodyHtml = isBodyHtml;
        await SendAsync(mailMessage);
    }

    public async Task SendEmailAsync(MailMessage mailMessage)
    {
        await SendAsync(mailMessage);
    }
    
    private MailMessage CreateDefaultMessage()
    {
        var mailMessage = new MailMessage();
        mailMessage.From = new MailAddress(
            _emailOptions.ApplicationEmailAddress, _emailOptions.ApplicationName);

        return mailMessage;
    }
    
    private void Send(MailMessage mailMessage)
    {
        using var client = new SmtpClient(_emailOptions.Host, _emailOptions.Port);
        client.EnableSsl = _emailOptions.EnableSsl;
        client.UseDefaultCredentials = true;
        if (_emailOptions.RequiresAuthentication)
        {
            client.UseDefaultCredentials = false;
            client.Credentials = new NetworkCredential(
                _emailOptions.UserName, _emailOptions.Password);
        }

        try
        {
            client.Send(mailMessage);
        }
        catch (Exception)
        {
            throw;
        }
    }
    
    private async Task SendAsync(MailMessage mailMessage)
    {
        using var client = new SmtpClient(_emailOptions.Host, _emailOptions.Port);
        client.EnableSsl = _emailOptions.EnableSsl;
        client.UseDefaultCredentials = true;
        if (_emailOptions.RequiresAuthentication)
        {
            client.UseDefaultCredentials = false;
            client.Credentials = new NetworkCredential(
                _emailOptions.UserName, _emailOptions.Password);
        }

        try
        {
            await client.SendMailAsync(mailMessage);
        }
        catch (Exception)
        {
            throw;
        }
    }
}