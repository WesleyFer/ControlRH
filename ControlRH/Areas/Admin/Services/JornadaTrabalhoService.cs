using ControlRH.Areas.Admin.Contracts;
using ControlRH.Areas.Admin.Models;
using ControlRH.Areas.Admin.Models.ViewModels;
using ControlRH.Core.Contracts;
using ControlRH.Core.Enums;
using ControlRH.Models.ViewModels;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Linq.Expressions;

namespace ControlRH.Areas.Admin.Services;

public class JornadaTrabalhoService : IJornadaTrabalhoService
{
    private readonly IQueryContext _queryContext;
    private readonly IUnitOfWork _uow;

    public JornadaTrabalhoService(IQueryContext queryContext, IUnitOfWork uow)
    {
        _queryContext = queryContext;
        _uow = uow;
    }

    public async Task<IEnumerable<JornadaTrabalho>> GetAllAsync(CancellationToken cancellationToken = default)
    {
        return await _queryContext.QueryJornadasTrabalhos
            .ToListAsync(cancellationToken);
    }

    public async Task<JornadaTrabalho?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default)
    {
        return await _queryContext.QueryJornadasTrabalhos
            .FirstOrDefaultAsync(c => c.Id == id, cancellationToken);
    }

    public async Task<DynamicTableViewModel> ObterTabelaIndexAsync(string? search, int page = 1, int pageSize = 5, string? sort = null, string? dir = null, CancellationToken cancellationToken = default)
    {
        var query = _queryContext.QueryJornadasTrabalhos;
        if (!string.IsNullOrWhiteSpace(search))
        {
            query = query.Where(x => EF.Functions.Like(x.Nome, $"%{search}%"));
        }

        if (!string.IsNullOrEmpty(sort) && Columns.ContainsKey(sort))
        {
            var parameter = Expression.Parameter(typeof(JornadaTrabalho), "c");
            var property = Expression.Property(parameter, sort);
            var lambda = Expression.Lambda(property, parameter);

            var methodName = string.Equals(dir, "asc", StringComparison.OrdinalIgnoreCase) ? "OrderBy" : "OrderByDescending";
            var method = typeof(Queryable).GetMethods()
                .First(m => m.Name == methodName && m.GetParameters().Length == 2);

            var genericMethod = method.MakeGenericMethod(typeof(JornadaTrabalho), property.Type);

            query = (IQueryable<JornadaTrabalho>)genericMethod.Invoke(null, new object[] { query, lambda });
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
            ControllerName = nameof(JornadaTrabalho),
            TextoBotaoAdicionar = "Nova Jornada Trabalho",
            Export = false
        };

        return viewModel;
    }

    public async Task<JornadaTrabalhoViewModel?> DetailsAsync(Guid id, CancellationToken cancellationToken = default)
    {
        var entidade = await GetByIdAsync(id, cancellationToken);
        if (entidade is null)
            return null;

        var viewModel = new JornadaTrabalhoViewModel();
        viewModel.ToViewModel(entidade);
        return viewModel;
    }

    public async Task InsertAsync(JornadaTrabalhoViewModel viewModel, CancellationToken cancellationToken = default)
    {
        var entidade = viewModel.ToModel();
        if (!entidade.IsValid)
        {
            entidade.AddNotification("", "Entidade inválida.");
            return;
        }

        var repositorio = _uow.Repository<JornadaTrabalho>();
        await repositorio.InsertAsync(entidade, cancellationToken);
        var changes = await _uow.CommitAsync(cancellationToken);

        if (changes <= 0)
        {
            entidade.AddNotification("", "Erro ao salvar jornada de trabalho.");
        }
    }

    public async Task UpdateAsync(Guid id, JornadaTrabalhoViewModel viewModel, CancellationToken cancellationToken = default)
    {
        var entidade = await GetByIdAsync(id, cancellationToken);

        if (entidade is null)
            return;

        var repositorio = _uow.Repository<JornadaTrabalho>();

        entidade.AtualizarNome(viewModel.Nome);

        foreach (var diaSemana in viewModel.DiasSelecionados)
        {
            entidade.AtualizarHorario(diaSemana, viewModel.HoraEntrada, viewModel.HoraSaida, viewModel.DuracaoIntervalo);
        }

        // Define 00:00 nos dias NÃO selecionados
        var todosOsDiasSemana = Enum.GetValues(typeof(DiaSemanaType)).Cast<DiaSemanaType>();
        var diasNaoSelecionados = todosOsDiasSemana.Except(viewModel.DiasSelecionados);

        foreach (var dia in diasNaoSelecionados)
        {
            entidade.AtualizarHorario(
                dia,
                TimeSpan.Zero,     // HoraEntrada
                TimeSpan.Zero,     // HoraSaida
                TimeSpan.Zero      // DuracaoIntervalo
            );
        }

        await repositorio.UpdateAsync(entidade, cancellationToken);

        var changes = await _uow.CommitAsync(cancellationToken);

        if (changes <= 0)
        {
            entidade.AddNotification("", "Erro ao atualizar jornada de trabalho.");
        }
    }

    public async Task DeleteAsync(Guid id, CancellationToken cancellationToken = default)
    {
        var entidade = await GetByIdAsync(id, cancellationToken);
        if (entidade is null)
            return;

        var repositorio = _uow.Repository<JornadaTrabalho>();
        await repositorio.DeleteAsync(entidade, cancellationToken);
        var changes = await _uow.CommitAsync(cancellationToken);

        if (changes <= 0)
        {
            entidade.AddNotification("", "Erro ao deletar jornada de trabalho.");
            return;
        }
    }

    private Dictionary<string, string> Columns => new()
    {
        { "Nome", "NOME" },
    };

}
