using ControlRH.Areas.Admin.Contracts;
using ControlRH.Areas.Admin.Models;
using ControlRH.Areas.Admin.Models.ViewModels;
using ControlRH.Core.Contracts;
using ControlRH.Core.Data;
using ControlRH.Core.Extensions;
using ControlRH.Models.ViewModels;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace ControlRH.Areas.Admin.Services;

public class GrupoService : IGrupoService
{
    private readonly IQueryContext _queryContext;
    private readonly DbContext _context;

    public GrupoService(
        IQueryContext queryContext,
        DbContext context)
    {
        _queryContext = queryContext;
        _context = context;
    }

    public async Task<IEnumerable<Grupo>> GetAllAsync(CancellationToken cancellationToken = default)
    {
        return await _queryContext.QueryGrupos
            .ToListAsync(cancellationToken);
    }

    public async Task<Grupo?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default)
    {
        return await _context.Set<Grupo>()
            .Include(g => g.GruposPermissoes)
            .Include(g => g.UsuariosGrupos)
            .FirstOrDefaultAsync(c => c.Id == id, cancellationToken);
    }

    public async Task<DynamicTableViewModel> ObterTabelaIndexAsync(string? search, int page = 1, int pageSize = 5, string? sort = null, string? dir = null, CancellationToken cancellationToken = default)
    {
        var query = _queryContext.QueryGrupos;
        if (!string.IsNullOrWhiteSpace(search))
        {
            query = query.Where(x => EF.Functions.Like(x.Nome, $"%{search}%"));
        }

        if (!string.IsNullOrEmpty(sort) && Columns.ContainsKey(sort))
        {
            var parameter = Expression.Parameter(typeof(Grupo), "c");
            var property = Expression.Property(parameter, sort);
            var lambda = Expression.Lambda(property, parameter);

            var methodName = string.Equals(dir, "asc", StringComparison.OrdinalIgnoreCase) ? "OrderBy" : "OrderByDescending";
            var method = typeof(Queryable).GetMethods()
                .First(m => m.Name == methodName && m.GetParameters().Length == 2);

            var genericMethod = method.MakeGenericMethod(typeof(Grupo), property.Type);

            query = (IQueryable<Grupo>)genericMethod.Invoke(null, new object[] { query, lambda });
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
            ControllerName = nameof(Grupo),
            TextoBotaoAdicionar = "Novo Grupo",
            Export = false
        };

        return viewModel;
    }

    public async Task<GrupoViewModel?> DetailsAsync(Guid id, CancellationToken cancellationToken = default)
    {
        var entidade = await GetByIdAsync(id, cancellationToken);
        if (entidade is null)
            return null;

        var viewModel = new GrupoViewModel();
        viewModel.ToViewModel(entidade);
        return viewModel;
    }

    public async Task InsertAsync(GrupoViewModel viewModel, CancellationToken cancellationToken = default)
    {
        var entidade = viewModel.ToModel();
        if (!entidade.IsValid)
        {
            entidade.AddNotification("", "Entidade inválida.");
            return;
        }

        await _context.Set<Grupo>().AddAsync(entidade, cancellationToken);
        var changes = await _context.SaveChangesAsync(cancellationToken);
        if (changes <= 0)
            return;
    }

    public async Task UpdateAsync(Guid id, GrupoViewModel viewModel, CancellationToken cancellationToken = default)
    {
        var entidade = await GetByIdAsync(id, cancellationToken);

        if (entidade is null)
            return;

        _context.Set<GrupoPermissao>().RemoveRange(entidade.GruposPermissoes);
        entidade.AtualizarNome(viewModel.Nome);

        foreach (var permissaoId in viewModel.PermissoesSelecionadas)
        {
            var grupoPermissao = new GrupoPermissao(entidade.Id, permissaoId);
            await _context.Set<GrupoPermissao>().AddAsync(grupoPermissao, cancellationToken); // Correto agora
        }

        _context.Set<Grupo>().Update(entidade);
        var changes = await _context.SaveChangesAsync(cancellationToken);

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

        _context.Set<Grupo>().Remove(entidade);
        var changes = await _context.SaveChangesAsync(cancellationToken);

        if (changes <= 0)
        {
            entidade.AddNotification("", "Erro ao deletar.");
            return;
        }
    }

    public async Task<IEnumerable<PermissaoViewModel>> ObterPermissoesDisponiveisAsync(CancellationToken cancellationToken = default)
    {
        var permissoes = await _queryContext.QueryPermissoes
          .Select(c => new
          {
              c.Id,
              c.Chave,
              c.Valor,
              c.Tipo
          })
          .ToListAsync(cancellationToken);

        return permissoes
            .Select(c => new PermissaoViewModel
            {
                Id = c.Id,
                Chave = c.Chave,
                Valor = c.Valor,
                Categoria = c.Tipo.ToDescription()
            })
            .OrderBy(p => p.Categoria)
            .ThenBy(p => p.Valor);
    }

    private Dictionary<string, string> Columns => new()
    {
        { "Nome", "NOME" },
    };
}