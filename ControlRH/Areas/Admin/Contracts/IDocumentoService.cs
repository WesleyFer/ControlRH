using ControlRH.Areas.Admin.Models;
using ControlRH.Areas.Admin.Models.ViewModels;
using ControlRH.Models.ViewModels;

namespace ControlRH.Areas.Admin.Contracts;

public interface IDocumentoService
{
    Task<IEnumerable<Documento>> GetAllAsync(CancellationToken cancellationToken = default);
    Task<Documento?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default);
    Task<DynamicTableViewModel> ObterTabelaIndexAsync(string? search, int page = 1, int pageSize = 5, string? sort = null, string? dir = null, CancellationToken cancellationToken = default);
    Task<DocumentoViewModel?> DetailsAsync(Guid id, CancellationToken cancellationToken = default);
    Task InsertAsync(DocumentoViewModel viewModel, CancellationToken cancellationToken = default);
    Task UpdateAsync(Guid id, DocumentoViewModel viewModel, CancellationToken cancellationToken = default);
    Task DeleteAsync(Guid id, CancellationToken cancellationToken = default);
    Task<IEnumerable<CarteiraCliente>> CarteirasClientesAsync(CancellationToken cancellationToken = default);
    Task<IEnumerable<Models.Colaborador>> ColaboradoresPorCarteiraAsync(Guid carteiraClienteId, CancellationToken cancellationToken = default);
}
