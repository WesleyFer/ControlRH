using ControlRH.Areas.Admin.Models;
using ControlRH.Areas.Admin.Models.ViewModels;
using ControlRH.Models.ViewModels;

namespace ControlRH.Areas.Admin.Contracts;

public interface ICargoService
{
    Task<IEnumerable<Cargo>> GetAllAsync(CancellationToken cancellationToken = default);
    Task<Cargo?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default);
    Task<DynamicTableViewModel> ObterTabelaIndexAsync(string? search, int page = 1, int pageSize = 5, string? sort = null, string? dir = null, CancellationToken cancellationToken = default);
    Task<CargoViewModel?> DetailsAsync(Guid id, CancellationToken cancellationToken = default);
    Task InsertAsync(CargoViewModel viewModel, CancellationToken cancellationToken = default);
    Task UpdateAsync(Guid id, CargoViewModel viewModel, CancellationToken cancellationToken = default);
    Task DeleteAsync(Guid id, CancellationToken cancellationToken = default);
}
