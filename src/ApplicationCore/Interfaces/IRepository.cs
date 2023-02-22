using Ardalis.Specification;

namespace Alanyang.DotNetEmail.ApplicationCore.Interfaces;

public interface IRepository<T> : IRepositoryBase<T> where T : class, IAggregateRoot
{
    
}