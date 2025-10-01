using ControlRH.Areas.Admin.Models;
using ControlRH.Areas.Admin.Models.ViewModels;
using ControlRH.Models.ViewModels;

namespace ControlRH.Areas.Admin.Contracts;

public interface ICarteiraClienteService
{
    Task<IEnumerable<CarteiraCliente>> GetAllAsync(CancellationToken cancellationToken = default);
    Task<CarteiraCliente?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default);
    Task<DynamicTableViewModel> ObterTabelaIndexAsync(string? search, int page = 1, int pageSize = 5, string? sort = null, string? dir = null, CancellationToken cancellationToken = default);
    Task<CarteiraClienteViewModel?> DetailsAsync(Guid id, CancellationToken cancellationToken = default);
    Task InsertAsync(CarteiraClienteViewModel viewModel, CancellationToken cancellationToken = default);
    Task UpdateAsync(Guid id, CarteiraClienteViewModel viewModel, CancellationToken cancellationToken = default);
    Task DeleteAsync(Guid id, CancellationToken cancellationToken = default);
}
