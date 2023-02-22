using Ardalis.Specification;

namespace Alanyang.DotNetEmail.ApplicationCore.Interfaces;

public interface IReadRepository<T> : IReadRepositoryBase<T> where T : class, IAggregateRoot
{
    
}