using ControlRH.Areas.Admin.Models;
using ControlRH.Areas.Admin.Models.ViewModels;
using ControlRH.Models.ViewModels;

namespace ControlRH.Areas.Admin.Contracts;

public interface IGrupoService
{
    Task<IEnumerable<Grupo>> GetAllAsync(CancellationToken cancellationToken = default);
    Task<Grupo?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default);
    Task<DynamicTableViewModel> ObterTabelaIndexAsync(string? search, int page = 1, int pageSize = 5, string? sort = null, string? dir = null, CancellationToken cancellationToken = default);
    Task<GrupoViewModel?> DetailsAsync(Guid id, CancellationToken cancellationToken = default);
    Task InsertAsync(GrupoViewModel viewModel, CancellationToken cancellationToken = default);
    Task UpdateAsync(Guid id, GrupoViewModel viewModel, CancellationToken cancellationToken = default);
    Task DeleteAsync(Guid id, CancellationToken cancellationToken = default);
    Task<IEnumerable<PermissaoViewModel>> ObterPermissoesDisponiveisAsync(CancellationToken cancellationToken = default);
}
