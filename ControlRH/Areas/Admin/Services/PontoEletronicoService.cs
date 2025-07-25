﻿using ControlRH.Areas.Admin.Contracts;
using ControlRH.Areas.Admin.Models;
using ControlRH.Areas.Admin.Models.ViewModels;
using ControlRH.Core.Contracts;
using ControlRH.Models.ViewModels;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace ControlRH.Areas.Admin.Services;

public class PontoEletronicoService : IPontoEletronicoService
{
    private readonly IQueryContext _queryContext;
    private readonly IUnitOfWork _uow;

    public PontoEletronicoService(IQueryContext queryContext, IUnitOfWork uow)
    {
        _queryContext = queryContext;
        _uow = uow;
    }

    public async Task<IEnumerable<PontoEletronico>> GetAllAsync(CancellationToken cancellationToken = default)
    {
        return await _queryContext.QueryPontosEletronicos
            .ToListAsync(cancellationToken);
    }

    public async Task<PontoEletronico?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default)
    {
        return await _queryContext.QueryPontosEletronicos
            .FirstOrDefaultAsync(c => c.Id == id, cancellationToken);
    }

    public async Task<DynamicTableViewModel> ObterTabelaIndexAsync(string? search, int page = 1, int pageSize = 5, string? sort = null, string? dir = null, CancellationToken cancellationToken = default)
    {
        var query = _queryContext.QueryPontosEletronicos;
        if (!string.IsNullOrWhiteSpace(search))
        {
            // query = query.Where(x => EF.Functions.Like(x.Nome, $"%{search}%"));
        }

        if (!string.IsNullOrEmpty(sort) && Columns.ContainsKey(sort))
        {
            var parameter = Expression.Parameter(typeof(PontoEletronico), "c");
            var property = Expression.Property(parameter, sort);
            var lambda = Expression.Lambda(property, parameter);

            var methodName = string.Equals(dir, "asc", StringComparison.OrdinalIgnoreCase) ? "OrderBy" : "OrderByDescending";
            var method = typeof(Queryable).GetMethods()
                .First(m => m.Name == methodName && m.GetParameters().Length == 2);

            var genericMethod = method.MakeGenericMethod(typeof(PontoEletronico), property.Type);

            query = (IQueryable<PontoEletronico>)genericMethod.Invoke(null, new object[] { query, lambda });
        }

        var totalItems = await query.CountAsync();
        var pageData = await query.Skip((page - 1) * pageSize).Take(pageSize).ToListAsync();

        var viewModel = new DynamicTableViewModel
        {
            Data = pageData.Cast<object>(),
            Columns = Columns,
            PageNumber = page,
            PageSize = pageSize,
            TotalItems = totalItems,
            Search = search,
            Sort = sort,
            Dir = dir,
            AreaName = "Admin",
            ControllerName = nameof(PontoEletronico),
            TextoBotaoAdicionar = "Nova Marcação",
            Export = false
        };

        return viewModel;
    }

    public async Task<PontoEletronicoViewModel?> DetailsAsync(Guid id, CancellationToken cancellationToken = default)
    {
        var entidade = await GetByIdAsync(id, cancellationToken);
        if (entidade is null)
            return null;

        var viewModel = new PontoEletronicoViewModel();
        viewModel.ToViewModel(entidade);
        return viewModel;
    }

    public async Task InsertAsync(PontoEletronicoViewModel viewModel, CancellationToken cancellationToken = default)
    {
        var entidade = viewModel.ToModel();
        if (!entidade.IsValid)
        {
            entidade.AddNotification("", "Entidade inválida.");
            return;
        }

        var repositorio = _uow.Repository<PontoEletronico>();
        await repositorio.InsertAsync(entidade, cancellationToken);
        var changes = await _uow.CommitAsync(cancellationToken);

        if (changes <= 0)
        {
            entidade.AddNotification("", "Erro ao salvar.");
        }
    }

    public async Task UpdateAsync(Guid id, PontoEletronicoViewModel viewModel, CancellationToken cancellationToken = default)
    {
        var entidade = await GetByIdAsync(id, cancellationToken);

        if (entidade is null)
            return;

        var repositorio = _uow.Repository<PontoEletronico>();

        await repositorio.UpdateAsync(entidade, cancellationToken);

        var changes = await _uow.CommitAsync(cancellationToken);

        if (changes <= 0)
        {
            entidade.AddNotification("", "Erro ao atualizar.");
        }
    }

    public async Task DeleteAsync(Guid id, CancellationToken cancellationToken = default)
    {
        var entidade = await GetByIdAsync(id, cancellationToken);
        if (entidade is null)
            return;

        var repositorio = _uow.Repository<PontoEletronico>();
        await repositorio.DeleteAsync(entidade, cancellationToken);
        var changes = await _uow.CommitAsync(cancellationToken);

        if (changes <= 0)
        {
            entidade.AddNotification("", "Erro ao deletar.");
            return;
        }
    }

    private Dictionary<string, string> Columns => new()
    {
        { "Nome", "NOME" },
    };

}
