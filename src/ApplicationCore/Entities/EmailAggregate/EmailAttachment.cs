namespace Alanyang.DotNetEmail.ApplicationCore.Entities.EmailAggregate;

public class EmailAttachment : BaseEntity
{
    public int EmailRecordId { get; private set; }
    public string Name { get; private set; }
    public byte[] Content { get; private set; }

    public EmailAttachment(
        string name,
        byte[] content)
    {
        Name = name;
        Content = content;
    }
}