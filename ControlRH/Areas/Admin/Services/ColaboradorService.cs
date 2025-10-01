using ControlRH.Areas.Admin.Contracts;
using ControlRH.Areas.Admin.Models;
using ControlRH.Areas.Admin.Models.ViewModels;
using ControlRH.Core.Contracts;
using ControlRH.Core.Helpers;
using ControlRH.Core.Models;
using ControlRH.Models.ViewModels;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace ControlRH.Areas.Admin.Services;

public class ColaboradorService : IColaboradorService
{
    private readonly IQueryContext _queryContext;
    private readonly IUnitOfWork _uow;

    public ColaboradorService(IQueryContext queryContext, IUnitOfWork uow)
    {
        _queryContext = queryContext;
        _uow = uow;
    }

    public async Task<IEnumerable<Models.Colaborador>> GetAllAsync(CancellationToken cancellationToken = default)
    {
        return await _queryContext.QueryColaboradores
            .ToListAsync(cancellationToken);
    }

    public async Task<IEnumerable<CarteiraCliente>> CarteirasClientesAsync(CancellationToken cancellationToken = default)
    {
        return await _queryContext.QueryCarteirasClientes
            .ToListAsync(cancellationToken);
    }

    public async Task<IEnumerable<JornadaTrabalho>> JornadasTrabalhosAsync(CancellationToken cancellationToken = default)
    {
        return await _queryContext.QueryJornadasTrabalhos
            .ToListAsync(cancellationToken);
    }

    public async Task<IEnumerable<Cargo>> CargosAsync(CancellationToken cancellationToken = default)
    {
        return await _queryContext.QueryCargos
            .ToListAsync(cancellationToken);
    }

    public async Task<Models.Colaborador?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default)
    {
        return await _queryContext.QueryColaboradores
            .FirstOrDefaultAsync(c => c.Id == id, cancellationToken);
    }

    public async Task<DynamicTableViewModel> ObterTabelaIndexAsync(string? search, int page = 1, int pageSize = 5, string? sort = null, string? dir = null, CancellationToken cancellationToken = default)
    {
        var query = _queryContext.QueryColaboradores;
        if (!string.IsNullOrWhiteSpace(search))
        {
            query = query.Where(x => EF.Functions.Like(x.Nome, $"%{search}%"));
        }

        if (!string.IsNullOrEmpty(sort) && Columns.ContainsKey(sort))
        {
            var parameter = Expression.Parameter(typeof(Models.Colaborador), "c");
            var property = Expression.Property(parameter, sort);
            var lambda = Expression.Lambda(property, parameter);

            var methodName = string.Equals(dir, "asc", StringComparison.OrdinalIgnoreCase) ? "OrderBy" : "OrderByDescending";
            var method = typeof(Queryable).GetMethods()
                .First(m => m.Name == methodName && m.GetParameters().Length == 2);

            var genericMethod = method.MakeGenericMethod(typeof(Models.Colaborador), property.Type);

            query = (IQueryable<Models.Colaborador>)genericMethod.Invoke(null, new object[] { query, lambda });
        }

        var totalItems = await query.CountAsync();
        var pageData = await query.Skip((page - 1) * pageSize).Take(pageSize).ToListAsync();

        var viewModel = new DynamicTableViewModel
        {
            Data = pageData
                   .Select(entity => new
                   {
                       Id = entity.Id,
                       Cpf = Utils.FormatarCpf(entity.Cpf),
                       Pis = Utils.FormatarPis(entity.Pis),
                       Matricula = Utils.FormatarMatricula(entity.Matricula),
                       Nome = entity.Nome,
                       Cargo = entity.Cargo
                   }).Cast<object>(),
            Columns = Columns,
            PageNumber = page,
            PageSize = pageSize,
            TotalItems = totalItems,
            Search = search,
            Sort = sort,
            Dir = dir,
            AreaName = "Admin",
            ControllerName = nameof(Colaborador),
            TextoBotaoAdicionar = "Novo Colaborador",
            Export = false
        };

        return viewModel;
    }

    public async Task<ColaboradorViewModel?> DetailsAsync(Guid id, CancellationToken cancellationToken = default)
    {
        var entidade = await GetByIdAsync(id, cancellationToken);
        if (entidade is null)
            return null;

        var viewModel = new ColaboradorViewModel();
        viewModel.ToViewModel(entidade);
        return viewModel;
    }

    public async Task InserirColaboradorComUsuarioAsync(ColaboradorViewModel viewModel, CancellationToken cancellationToken = default)
    {
        var entidade = viewModel.ToModel();
        if (!entidade.IsValid)
        {
            entidade.AddNotification("", "Entidade inválida.");
            return;
        }

        var repositorio = _uow.Repository<Models.Colaborador>();

        await repositorio.InsertAsync(entidade, cancellationToken);

        var usuarioExistente = await _queryContext.QueryUsuarios
            .AnyAsync(c => c.Login == entidade.Cpf, cancellationToken);

        if (!usuarioExistente)
            await CriarNovoUsuarioAsync(entidade.Id, entidade.Cpf, cancellationToken);

        var changes = await _uow.CommitAsync(cancellationToken);

        if (changes <= 0)
            return;
    }

    private async Task CriarNovoUsuarioAsync(Guid colaboradorId, string cpf, CancellationToken cancellationToken = default)
    {
        var usuarioRepo = _uow.Repository<Usuario>();
        var usuario = new Usuario(cpf, colaboradorId);

        await AssociarUsuarioGrupoPadraoAsync(usuario);

        await usuarioRepo.InsertAsync(usuario, cancellationToken);
    }

    private async Task AssociarUsuarioGrupoPadraoAsync(Usuario usuario)
    {
        var grupoPadrao = await _queryContext.QueryGrupos
            .FirstOrDefaultAsync(c => c.Nome == "Colaboradores");

        if (grupoPadrao is not null)
        {
            usuario.AdicionarUsuarioGrupo(grupoPadrao.Id);
        }
    }

    public async Task UpdateAsync(Guid id, ColaboradorViewModel viewModel, CancellationToken cancellationToken = default)
    {
        var entidade = await GetByIdAsync(id, cancellationToken);

        if (entidade is null)
            return;

        var repositorio = _uow.Repository<Models.Colaborador>();
        entidade.AtualizarNome(viewModel.Nome);

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

        var repositorio = _uow.Repository<Models.Colaborador>();
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
        { "Cpf", "CPF" },
        { "Pis", "PIS" },
        { "Matricula", "MATRICULA" },
        { "Nome", "NOME" },
        { "Cargo", "CARGO" },
    };
}
