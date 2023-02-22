using System.Net.Mail;
using Alanyang.DotNetEmail.ApplicationCore.Interfaces;

namespace Alanyang.DotNetEmail.ApplicationCore.Entities.EmailAggregate;

public class EmailRecord : BaseEntity, IAggregateRoot
{
    private readonly List<EmailAttachment> _attachments = new();

    public string Recipient { get; private set; }
    public ICollection<string> Cc { get; private set; } = new List<string>();
    public ICollection<string> Bcc { get; private set; } = new List<string>();
    public string Subject { get; private set; }
    public string Body { get; private set; }
    public IReadOnlyCollection<EmailAttachment> Attachments => _attachments.AsReadOnly();
    public bool IsSent { get; private set; }

    public EmailRecord(
        string recipient,
        string subject,
        string body
    )
    {
        Recipient = recipient;
        Subject = subject;
        Body = body;
    }
    
    public void MarkAsSent()
    {
        IsSent = true;
    }
    
    public void AddCc(string address)
    {
        Cc.Add(address);
    }
    
    public void AddBcc(string address)
    {
        Bcc.Add(address);
    }
    
    public void AddAttachment(string fileName, byte[] fileContent)
    {
        _attachments.Add(new EmailAttachment(fileName, fileContent));
    }
    
    public List<Attachment> GetAttachments()
    {
        var attachments = new List<Attachment>();
        
        foreach (var backingAttachment in _attachments)
        {
            var stream = new MemoryStream(backingAttachment.Content);
            var attachment = new Attachment(stream, backingAttachment.Name);

            attachments.Add(attachment);
        }

        return attachments;
    }
}