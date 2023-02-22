using System.Reflection;
using Alanyang.DotNetEmail.ApplicationCore.Entities.EmailAggregate;
using Microsoft.EntityFrameworkCore;

namespace Alanyang.DotNetEmail.Infrastructure.Data;

public class ApplicationContext : DbContext
{
    public ApplicationContext(DbContextOptions<ApplicationContext> options) : base(options)
    {
        
    }

    public DbSet<EmailRecord> EmailRecords => Set<EmailRecord>();
    
    protected override void OnModelCreating(ModelBuilder builder)
    {
        builder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());
    }
}