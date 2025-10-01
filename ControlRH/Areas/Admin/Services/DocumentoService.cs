using ControlRH.Areas.Admin.Contracts;
using ControlRH.Areas.Admin.Models;
using ControlRH.Areas.Admin.Models.ViewModels;
using ControlRH.Core.Configurations;
using ControlRH.Core.Contracts;
using ControlRH.Models.ViewModels;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using System.Linq.Expressions;

namespace ControlRH.Areas.Admin.Services;

public class DocumentoService : IDocumentoService
{
    private readonly IQueryContext _queryContext;
    private readonly IUnitOfWork _uow;
    private readonly UploadSettings _uploadSettings;

    public DocumentoService(
        IQueryContext queryContext,
        IUnitOfWork uow,
        IOptions<UploadSettings> uploadOptions)
    {
        _queryContext = queryContext;
        _uow = uow;
        _uploadSettings = uploadOptions.Value;
    }

    public async Task<IEnumerable<Documento>> GetAllAsync(CancellationToken cancellationToken = default)
    {
        return await _queryContext.QueryDocumentos
            .ToListAsync(cancellationToken);
    }

    public async Task<Documento?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default)
    {
        return await _queryContext.QueryDocumentos
            .FirstOrDefaultAsync(c => c.Id == id, cancellationToken);
    }

    public async Task<DynamicTableViewModel> ObterTabelaIndexAsync(string? search, int page = 1, int pageSize = 5, string? sort = null, string? dir = null, CancellationToken cancellationToken = default)
    {
        var query = _queryContext.QueryDocumentos;
        if (!string.IsNullOrWhiteSpace(search))
        {
            query = query.Where(x => EF.Functions.Like(x.Nome, $"%{search}%"));
        }

        if (!string.IsNullOrEmpty(sort) && Columns.ContainsKey(sort))
        {
            var parameter = Expression.Parameter(typeof(Documento), "c");
            var property = Expression.Property(parameter, sort);
            var lambda = Expression.Lambda(property, parameter);

            var methodName = string.Equals(dir, "asc", StringComparison.OrdinalIgnoreCase) ? "OrderBy" : "OrderByDescending";
            var method = typeof(Queryable).GetMethods()
                .First(m => m.Name == methodName && m.GetParameters().Length == 2);

            var genericMethod = method.MakeGenericMethod(typeof(Documento), property.Type);

            query = (IQueryable<Documento>)genericMethod.Invoke(null, new object[] { query, lambda });
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
            ControllerName = nameof(Documento),
            TextoBotaoAdicionar = "Novo Documento",
            Export = false,
            DisabledDetails = true,
            DisabledEdit = true
        };

        return viewModel;
    }

    public async Task<DocumentoViewModel?> DetailsAsync(Guid id, CancellationToken cancellationToken = default)
    {
        var entidade = await GetByIdAsync(id, cancellationToken);
        if (entidade is null)
            return null;

        var viewModel = new DocumentoViewModel();
        viewModel.ToViewModel(entidade);
        return viewModel;
    }

    public async Task InsertAsync(DocumentoViewModel viewModel, CancellationToken cancellationToken = default)
    {
        if (viewModel.Arquivo == null || viewModel.Arquivo.Length == 0)
            throw new InvalidOperationException("Arquivo é obrigatório.");

        // Tratamento do arquivo físico
        var caminhoArquivo = await SalvarArquivoAsync(viewModel.Arquivo, viewModel.CarteiraClienteId, viewModel.ColaboradorId, cancellationToken);

        // Persistência de dados
        var entidade = viewModel.ToModel(caminhoArquivo);
        if (!entidade.IsValid)
        {
            entidade.AddNotification("", "Entidade inválida.");
            return;
        }

        var repositorio = _uow.Repository<Documento>();
        await repositorio.InsertAsync(entidade, cancellationToken);
        var changes = await _uow.CommitAsync(cancellationToken);

        if (changes <= 0)
            entidade.AddNotification("", "Erro ao salvar.");
    }

    public async Task UpdateAsync(Guid id, DocumentoViewModel viewModel, CancellationToken cancellationToken = default)
    {
        var entidade = await GetByIdAsync(id, cancellationToken);

        if (entidade is null)
            return;

        var repositorio = _uow.Repository<Documento>();


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

        // 2. Deleta o registro do banco
        await DeleteFromDatabaseAsync(entidade, cancellationToken);

        // 3. Deleta o arquivo ou pasta
        DeleteFileOrFolder(entidade.CaminhoArquivo); // ou outra propriedade que guarda o caminho
    }

    private async Task DeleteFromDatabaseAsync(Documento entidade, CancellationToken cancellationToken)
    {
        var repositorio = _uow.Repository<Documento>();
        await repositorio.DeleteAsync(entidade, cancellationToken);

        var changes = await _uow.CommitAsync(cancellationToken);

        if (changes <= 0)
        {
            entidade.AddNotification("", "Erro ao deletar do banco.");
        }

    }

    private void DeleteFileOrFolder(string filePath)
    {
        if (string.IsNullOrEmpty(filePath) || !System.IO.File.Exists(filePath))
            return;

        try
        {
            // Deleta o arquivo
            File.Delete(filePath);

            // Verifica se a pasta está vazia e deleta se for o último arquivo
            var folder = Path.GetDirectoryName(filePath);
            if (!string.IsNullOrEmpty(folder) && Directory.Exists(folder))
            {
                var remainingFiles = Directory.GetFiles(folder);
                if (remainingFiles.Length == 0)
                {
                    Directory.Delete(folder, recursive: true);
                }
            }
        }
        catch (Exception ex)
        {
            throw;
        }
    }

    public async Task<IEnumerable<Models.Colaborador>> ColaboradoresPorCarteiraAsync(Guid carteiraClienteId, CancellationToken cancellationToken = default)
    {
        return await _queryContext.QueryColaboradores
            .Where(c => c.CarteiraClienteId == carteiraClienteId)
            .ToListAsync(cancellationToken);
    }

    public async Task<IEnumerable<CarteiraCliente>> CarteirasClientesAsync(CancellationToken cancellationToken = default)
    {
        return await _queryContext.QueryCarteirasClientes
            .ToListAsync(cancellationToken);
    }

    private async Task<string> SalvarArquivoAsync(IFormFile arquivo, Guid carteiraClienteId, Guid? colaboradorId, CancellationToken cancellationToken)
    {
        // Caminho base do upload
        var basePath = _uploadSettings.BasePath ?? Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "uploads");

        // Cria diretório da carteira
        var carteiraPath = Path.Combine(basePath, carteiraClienteId.ToString());
        if (!Directory.Exists(carteiraPath))
            Directory.CreateDirectory(carteiraPath);

        // Cria subdiretório do colaborador, se informado
        string finalPath = carteiraPath;
        if (colaboradorId.HasValue)
        {
            finalPath = Path.Combine(carteiraPath, colaboradorId.Value.ToString());
            if (!Directory.Exists(finalPath))
                Directory.CreateDirectory(finalPath);
        }

        // Nome único do arquivo
        var fileName = $"{arquivo.FileName}";
        var filePath = Path.Combine(finalPath, fileName);

        // Salvar fisicamente
        await using (var stream = new FileStream(filePath, FileMode.Create))
        {
            await arquivo.CopyToAsync(stream, cancellationToken);
        }

        // Retorna caminho relativo (para banco e exibição em View)
        var caminhoRelativo = filePath.Replace(Directory.GetCurrentDirectory(), "")
                                      .Replace("\\", "/");

        //if (!caminhoRelativo.StartsWith("/"))
        //    caminhoRelativo = "/" + caminhoRelativo;

        return caminhoRelativo;
    }

    private Dictionary<string, string> Columns => new()
    {
        { "Nome", "NOME" },
        { "Descricao", "DESCRICAO" },
    };

}
