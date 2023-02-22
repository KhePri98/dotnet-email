using System.Text.Json;
using Alanyang.DotNetEmail.ApplicationCore.Entities.EmailAggregate;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace Alanyang.DotNetEmail.Infrastructure.Data.EntityConfiguration;

public class EmailRecordConfiguration : IEntityTypeConfiguration<EmailRecord>
{
    public void Configure(EntityTypeBuilder<EmailRecord> builder)
    {
        builder.Property(e => e.Recipient)
            .HasMaxLength(256);

#pragma warning disable 8603,8604
        var options = new JsonSerializerOptions();
        var converter = new ValueConverter<ICollection<string>, string>(
            v => JsonSerializer.Serialize(v, options),
            v => JsonSerializer.Deserialize<List<string>>(v, options));

        var comparer = new ValueComparer<ICollection<string>>(
            (c1, c2) => c1.SequenceEqual(c2),
            c => c.Aggregate(0, (a, v) => HashCode.Combine(a, v.GetHashCode())),
            c => (ICollection<string>)c.ToList());
#pragma warning restore 8603,8604

        builder.Property(e => e.Cc)
            .HasMaxLength(256)
            .HasConversion(converter)
            .Metadata.SetValueComparer(comparer);
        
        builder.Property(e => e.Bcc)
            .HasMaxLength(256)
            .HasConversion(converter)
            .Metadata.SetValueComparer(comparer);

        builder.Property(e => e.Subject)
            .HasMaxLength(256);

        var navigation = builder.Metadata.FindNavigation(nameof(EmailRecord.Attachments));
        navigation?.SetPropertyAccessMode(PropertyAccessMode.Field);
    }
}