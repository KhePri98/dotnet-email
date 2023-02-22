using Ardalis.Specification.EntityFrameworkCore;
using Alanyang.DotNetEmail.ApplicationCore.Interfaces;

namespace Alanyang.DotNetEmail.Infrastructure.Data;

public class EfRepository<T> : RepositoryBase<T>, IReadRepository<T>, IRepository<T>
    where T : class, IAggregateRoot
{
    public EfRepository(ApplicationContext dbContext) : base(dbContext)
    {
        
    }
}