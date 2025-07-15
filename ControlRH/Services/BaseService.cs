using ControlRH.Core.Contracts;
using ControlRH.Core.Models;

namespace ControlRH.Services;

public abstract class BaseService<TEntity> : IBaseService<TEntity> where TEntity : Entidade, IAggregateRoot
{
    private readonly IUnitOfWork _uow;

    public BaseService(IUnitOfWork uow)
    {
        _uow = uow;
    }

    public async Task InsertAsync(TEntity entidade, CancellationToken cancellationToken = default)
    {
        if (!entidade.IsValid)
        {
            entidade.AddNotification("", "Entidade inválida.");
            return;
        }

        var repositorio = _uow.Repository<TEntity>();

        await repositorio.InsertAsync(entidade, cancellationToken);

        var changes = await _uow.CommitAsync(cancellationToken);

        if (changes <= 0)
        {
            entidade.AddNotification("", "Erro ao salvar entidade.");
        }
    }

    public virtual async Task UpdateAsync(TEntity entidade, CancellationToken cancellationToken = default)
    {
        if (!entidade.IsValid)
        {
            entidade.AddNotification("", "Entidade inválida.");
            return;
        }

        var repositorio = _uow.Repository<TEntity>();

        await repositorio.UpdateAsync(entidade, cancellationToken);

        var changes = await _uow.CommitAsync(cancellationToken);

        if (changes <= 0)
        {
            entidade.AddNotification("", "Erro ao atualizar entidade.");
        }
    }

    public async Task DeleteAsync(TEntity entidade, CancellationToken cancellationToken = default)
    {

        var repositorio = _uow.Repository<TEntity>();

        await repositorio.DeleteAsync(entidade, cancellationToken);

        var changes = await _uow.CommitAsync(cancellationToken);

        if (changes <= 0)
        {
            entidade.AddNotification("", "Erro ao deletar entidade.");
        }
    }
}

public abstract class BaseLoteService<TEntity> : IBaseLoteService<TEntity> where TEntity : Entidade, IAggregateRoot
{
    private readonly IUnitOfWork _uow;

    public BaseLoteService(IUnitOfWork uow)
    {
        _uow = uow;
    }

    public async Task InsertAsync(IEnumerable<TEntity> entidades, CancellationToken cancellationToken = default)
    {
        var invalids = entidades.Where(e => !e.IsValid).ToList();
        if (invalids.Any())
        {
            return;
        }

        var repositorio = _uow.Repository<TEntity>();

        await repositorio.InsertAsync(entidades, cancellationToken);

        var changes = await _uow.CommitAsync(cancellationToken);

        if (changes <= 0)
        {
            entidades.FirstOrDefault().AddNotification("", "Erro ao salvar entidade.");
        }
    }

    public async Task UpdateAsync(IEnumerable<TEntity> entidades, CancellationToken cancellationToken = default)
    {
        //var invalids = entidades.Where(e => !e.IsValid).ToList();
        //if (invalids.Any())
        //{
        //    return;
        //}

        //var repositorio = _uow.Repository<TEntity>();

        //await repositorio.UpdateAsync(entidades, cancellationToken);

        //var changes = await _uow.CommitAsync(cancellationToken);

        //if (changes <= 0)
        //{
        //    entidades.FirstOrDefault().AddNotification("", "Erro ao salvar entidade.");
        //}

        throw new NotImplementedException();
    }

    public async Task DeleteAsync(IEnumerable<TEntity> entidades, CancellationToken cancellationToken = default)
    {
        var repositorio = _uow.Repository<TEntity>();

        await repositorio.DeleteAsync(entidades, cancellationToken);

        var changes = await _uow.CommitAsync(cancellationToken);

        if (changes <= 0)
        {
            entidades.FirstOrDefault().AddNotification("", "Erro ao deletar entidade.");
        }
    }

}