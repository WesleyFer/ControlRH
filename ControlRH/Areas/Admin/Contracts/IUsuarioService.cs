using ControlRH.Areas.Admin.Models;
using ControlRH.Areas.Admin.Models.ViewModels;
using ControlRH.Models.ViewModels;

namespace ControlRH.Areas.Admin.Contracts;

public interface IUsuarioService
{
    Task<IEnumerable<Usuario>> GetAllAsync(CancellationToken cancellationToken = default);
    Task<Usuario?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default);
    Task<DynamicTableViewModel> ObterTabelaIndexAsync(string? search, int page = 1, int pageSize = 5, string? sort = null, string? dir = null, CancellationToken cancellationToken = default);
    Task<UsuarioViewModel?> DetailsAsync(Guid id, CancellationToken cancellationToken = default);
    Task InsertAsync(UsuarioViewModel viewModel, CancellationToken cancellationToken = default);
    Task UpdateAsync(Guid id, UsuarioViewModel viewModel, CancellationToken cancellationToken = default);
    Task DeleteAsync(Guid id, CancellationToken cancellationToken = default);
    Task<IEnumerable<GrupoViewModel>> GruposAsync(CancellationToken cancellationToken = default);
    Task<IEnumerable<ColaboradorViewModel>> ColaboradoresAsync(CancellationToken cancellationToken = default);
}
