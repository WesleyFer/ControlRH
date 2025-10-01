using System.Linq.Expressions;
using ControlRH.Core.Contracts;
using ControlRH.Core.Models;
using Microsoft.EntityFrameworkCore;

namespace ControlRH.Core.Data
{
    public static class RepositoryExtensions
    {
        public static async Task<TEntity?> FirstOrDefaultAsync<TEntity>(
            this IRepository<TEntity> repository,
            Expression<Func<TEntity, bool>> predicate,
            CancellationToken cancellationToken = default)
            where TEntity : Entidade, IAggregateRoot
        {
            return await repository.Query(predicate).FirstOrDefaultAsync(cancellationToken);
        }

        public static async Task<TEntity?> FirstOrDefaultAsync<TEntity>(
            this IRepository<TEntity> repository,
            Expression<Func<TEntity, bool>> predicate,
            Func<IQueryable<TEntity>, IQueryable<TEntity>>? includes = null,
            CancellationToken cancellationToken = default)
            where TEntity : Entidade, IAggregateRoot
        {
            var query = repository.Query().Where(predicate);

            if (includes != null)
                query = includes(query);

            return await query.FirstOrDefaultAsync(cancellationToken);
        }

        public static async Task<bool> ContainsAsync<TEntity>(
            this IRepository<TEntity> repository,
            Expression<Func<TEntity, bool>> predicate,
            CancellationToken cancellationToken = default)
            where TEntity : Entidade, IAggregateRoot
        {
            return await repository.Query(predicate).AnyAsync(cancellationToken);
        }

        public static async Task<long> ContarAsync<TEntity>(
            this IRepository<TEntity> repository,
            Expression<Func<TEntity, bool>> predicate,
            CancellationToken cancellationToken = default)
            where TEntity : Entidade, IAggregateRoot
        {
            return await repository.Query(predicate).LongCountAsync(cancellationToken);
        }
    }
}