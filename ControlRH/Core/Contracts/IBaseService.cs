using ControlRH.Core.Models;

namespace ControlRH.Core.Contracts;

public interface IBaseService<TEntity> where TEntity : Entidade, IAggregateRoot
{
    Task InsertAsync(TEntity entidade, CancellationToken cancellationToken = default);
    Task UpdateAsync(TEntity entidade, CancellationToken cancellationToken = default);
    Task DeleteAsync(TEntity entidade, CancellationToken cancellationToken = default);
}

public interface IBaseLoteService<TEntity> where TEntity : Entidade, IAggregateRoot
{
    Task InsertAsync(IEnumerable<TEntity> entidades, CancellationToken cancellationToken = default);
    Task UpdateAsync(IEnumerable<TEntity> entidades, CancellationToken cancellationToken = default);
    Task DeleteAsync(IEnumerable<TEntity> entidades, CancellationToken cancellationToken = default);
}