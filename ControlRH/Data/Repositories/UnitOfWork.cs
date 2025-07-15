using System.Collections.Concurrent;
using ControlRH.Core.Contracts;
using ControlRH.Core.Models;
using Microsoft.EntityFrameworkCore;

namespace ControlRH.Data.Repositories
{
    public sealed class UnitOfWork<TContext> : IUnitOfWork<TContext>, IDisposable
        where TContext : IQueryContext
    {
        private readonly DbContext _dbContext;
        private readonly ConcurrentDictionary<string, object> _repositories;
        private readonly IServiceProvider _serviceProvider;
        private bool _disposed = false;
        private readonly ILogger<UnitOfWork<TContext>> _logger;

        public UnitOfWork(TContext dbContext, IServiceProvider serviceProvider, ILogger<UnitOfWork<TContext>> logger)
        {
            _dbContext = dbContext as DbContext ?? throw new ArgumentNullException(nameof(dbContext));
            _repositories = new ConcurrentDictionary<string, object>();
            _serviceProvider = serviceProvider ?? throw new ArgumentNullException(nameof(serviceProvider));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        public async Task<int> CommitAsync(CancellationToken cancellationToken = default)
        {
            var executionStrategy = _dbContext.Database.CreateExecutionStrategy();

            return await executionStrategy.ExecuteAsync(async () =>
            {
                await using var transaction = await _dbContext.Database.BeginTransactionAsync(cancellationToken);

                try
                {
                    int result = await _dbContext.SaveChangesAsync(cancellationToken);
                    await transaction.CommitAsync(cancellationToken);
                    _logger.LogInformation("Transaction committed successfully.");
                    return result;
                }
                catch (Exception ex)
                {
                    await transaction.RollbackAsync(cancellationToken);
                    _logger.LogError(ex, "Error occurred during transaction. Rolling back.");
                    throw;
                }
            });
        }

        public async Task RollbackAsync()
        {
            _dbContext.ChangeTracker.Clear();
            await Task.CompletedTask;
        }

        public IRepository<TEntity> Repository<TEntity>() where TEntity : Entidade, IAggregateRoot
        {
            var entityTypeName = typeof(TEntity).FullName ?? typeof(TEntity).Name;

            if (_repositories.TryGetValue(entityTypeName, out var existingRepo))
            {
                return (IRepository<TEntity>)existingRepo;
            }

            var repository = _serviceProvider.GetService<IRepository<TEntity>>();

            if (repository == null)
            {
                repository = new Repository<TEntity>(_dbContext);
            }

            _repositories.TryAdd(entityTypeName, repository);
            return repository;
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        private void Dispose(bool disposing)
        {
            if (!_disposed)
            {
                if (disposing)
                {
                    _dbContext.Dispose();
                }

                _disposed = true;
            }
        }
    }
}