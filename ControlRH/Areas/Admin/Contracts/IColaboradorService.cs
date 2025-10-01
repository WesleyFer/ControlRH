using ControlRH.Areas.Admin.Models;
using ControlRH.Areas.Admin.Models.ViewModels;
using ControlRH.Models.ViewModels;

namespace ControlRH.Areas.Admin.Contracts;

public interface IColaboradorService
{
    Task<IEnumerable<Models.Colaborador>> GetAllAsync(CancellationToken cancellationToken = default);
    Task<IEnumerable<CarteiraCliente>> CarteirasClientesAsync(CancellationToken cancellationToken = default);
    Task<IEnumerable<JornadaTrabalho>> JornadasTrabalhosAsync(CancellationToken cancellationToken = default);
    Task<IEnumerable<Cargo>> CargosAsync(CancellationToken cancellationToken = default);
    Task<Models.Colaborador?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default);
    Task<DynamicTableViewModel> ObterTabelaIndexAsync(string? search, int page = 1, int pageSize = 5, string? sort = null, string? dir = null, CancellationToken cancellationToken = default);
    Task<ColaboradorViewModel?> DetailsAsync(Guid id, CancellationToken cancellationToken = default);
    Task InserirColaboradorComUsuarioAsync(ColaboradorViewModel viewModel, CancellationToken cancellationToken = default);
    Task UpdateAsync(Guid id, ColaboradorViewModel viewModel, CancellationToken cancellationToken = default);
    Task DeleteAsync(Guid id, CancellationToken cancellationToken = default);
}
