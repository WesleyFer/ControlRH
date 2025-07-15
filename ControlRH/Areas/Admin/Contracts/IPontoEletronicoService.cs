using ControlRH.Areas.Admin.Models;
using ControlRH.Areas.Admin.Models.ViewModels;
using ControlRH.Models.ViewModels;

namespace ControlRH.Areas.Admin.Contracts;

public interface IPontoEletronicoService
{
    Task<IEnumerable<PontoEletronico>> GetAllAsync(CancellationToken cancellationToken = default);
    Task<PontoEletronico?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default);
    Task<DynamicTableViewModel> ObterTabelaIndexAsync(string? search, int page = 1, int pageSize = 5, string? sort = null, string? dir = null, CancellationToken cancellationToken = default);
    Task<PontoEletronicoViewModel?> DetailsAsync(Guid id, CancellationToken cancellationToken = default);
    Task InsertAsync(PontoEletronicoViewModel viewModel, CancellationToken cancellationToken = default);
    Task UpdateAsync(Guid id, PontoEletronicoViewModel viewModel, CancellationToken cancellationToken = default);
    Task DeleteAsync(Guid id, CancellationToken cancellationToken = default);
}
