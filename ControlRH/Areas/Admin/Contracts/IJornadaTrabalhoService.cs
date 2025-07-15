using ControlRH.Areas.Admin.Models;
using ControlRH.Areas.Admin.Models.ViewModels;
using ControlRH.Models.ViewModels;
using System;

namespace ControlRH.Areas.Admin.Contracts;

public interface IJornadaTrabalhoService
{
    Task<IEnumerable<JornadaTrabalho>> GetAllAsync(CancellationToken cancellationToken = default);
    Task<JornadaTrabalho?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default);
    Task<DynamicTableViewModel> ObterTabelaIndexAsync(string? search, int page = 1, int pageSize = 5, string? sort = null, string? dir = null, CancellationToken cancellationToken = default);
    Task<JornadaTrabalhoViewModel?> DetailsAsync(Guid id, CancellationToken cancellationToken = default);
    Task InsertAsync(JornadaTrabalhoViewModel viewModel, CancellationToken cancellationToken = default);
    Task UpdateAsync(Guid id, JornadaTrabalhoViewModel viewModel, CancellationToken cancellationToken = default);
    Task DeleteAsync(Guid id, CancellationToken cancellationToken = default);
}
