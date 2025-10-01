using System.Linq.Expressions;

namespace ControlRH.Core.Contracts
{
    public interface IRepository<TEntity> : IDisposable where TEntity : class, IAggregateRoot
    {
        //Task<IEnumerable<TEntity>> FindAsync(Func<IQueryable<TEntity>, IQueryable<TEntity>>? queryBuilder = null, CancellationToken cancellationToken = default);

        IQueryable<TEntity> Query(Expression<Func<TEntity, bool>>? predicate = null);

        Task<IEnumerable<TEntity>> FindAsync(Expression<Func<TEntity, bool>> predicate = null, CancellationToken cancellationToken = default);

        Task InsertAsync(TEntity entidade, CancellationToken cancellationToken = default);

        Task InsertAsync(IEnumerable<TEntity> entidades, CancellationToken cancellationToken = default);

        Task UpdateAsync(TEntity entidade, CancellationToken cancellationToken = default);

        Task DeleteAsync(TEntity entidade, CancellationToken cancellationToken = default);

        Task DeleteAsync(IEnumerable<TEntity> entidades, CancellationToken cancellationToken = default);
    }
}
