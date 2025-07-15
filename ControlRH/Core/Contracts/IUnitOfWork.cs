using ControlRH.Core.Models;

namespace ControlRH.Core.Contracts
{
    public interface IUnitOfWork
    {
        IRepository<TEntidade> Repository<TEntidade>() where TEntidade : Entidade, IAggregateRoot;

        Task<int> CommitAsync(CancellationToken cancellationToken = default);
    }

    public interface IUnitOfWork<TContexto> : IUnitOfWork where TContexto : IQueryContext
    {
    }
}
