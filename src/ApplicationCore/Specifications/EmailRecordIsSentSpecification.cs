using Ardalis.Specification;
using Alanyang.DotNetEmail.ApplicationCore.Entities.EmailAggregate;

namespace Alanyang.DotNetEmail.ApplicationCore.Specifications;

public class EmailRecordIsSentSpecification : Specification<EmailRecord>
{
    public EmailRecordIsSentSpecification()
    {
        Query
            .Where(r => r.IsSent == true);
    }
}