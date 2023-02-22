using System.Net.Mail;
using Alanyang.DotNetEmail.ApplicationCore.Entities.EmailAggregate;
using Alanyang.DotNetEmail.ApplicationCore.Extensions;
using Alanyang.DotNetEmail.ApplicationCore.Interfaces;
using Alanyang.DotNetEmail.ApplicationCore.Specifications;

namespace Alanyang.DotNetEmail.ApplicationCore.Services;

public class EmailSendingQueue : IEmailSendingQueue
{
    private readonly IRepository<EmailRecord> _emailRecordRepository;
    private readonly IEmailSender _emailSender;

    public EmailSendingQueue(
        IRepository<EmailRecord> emailRecordRepository,
        IEmailSender emailSender)
    {
        _emailRecordRepository = emailRecordRepository;
        _emailSender = emailSender;
    }

    public async Task AddEmailAsync(
        string recipient,
        string subject,
        string body,
        IEnumerable<string>? cc,
        IEnumerable<string>? bcc,
        IEnumerable<Attachment>? attachments)
    {
        var record = new EmailRecord(recipient, subject, body);
        if (cc != null)
        {
            foreach (var ccAddress in cc)
            {
                record.AddCc(ccAddress);
            }
        }
        if (bcc != null)
        {
            foreach (var bccAddress in bcc)
            {
                record.AddBcc(bccAddress);
            }
        }
        if (attachments != null)
        {
            foreach (var attachment in attachments)
            {
                if (attachment.Name == null || string.IsNullOrWhiteSpace(attachment.Name))
                {
                    throw new ArgumentNullException("Attachment name cannot be null or empty.");
                }
                record.AddAttachment(attachment.Name, attachment.ContentStream.ReadAllBytes());
            }
        }
        await _emailRecordRepository.AddAsync(record);
    }

    public async Task DeleteAlreadySentAsync()
    {
        var recordSpec = new EmailRecordIsSentSpecification();
        var records = await _emailRecordRepository.ListAsync(recordSpec);
        await _emailRecordRepository.DeleteRangeAsync(records);
    }

    public async Task SendEmailAsync()
    {
        var recordSpec = new EmailRecordNotSentSpecification();
        var records = await _emailRecordRepository.ListAsync(recordSpec);

        for (int i = records.Count - 1; i >= 0; i--)
        {
            try
            {
                await _emailSender.SendEmailAsync(
                    recipient: records[i].Recipient,
                    subject: records[i].Subject,
                    body: records[i].Body,
                    cc: records[i].Cc,
                    bcc: records[i].Bcc,
                    attachments: records[i].GetAttachments()
                );
            }
            catch (Exception)
            {
                records.RemoveAt(i);
                continue;
            }

            records[i].MarkAsSent();
        }
        await _emailRecordRepository.UpdateRangeAsync(records);
    }
}