using ControlRH.Areas.Colaborador.Models.ViewModels;
using ControlRH.Areas.Colaborador.Models;
using ControlRH.Api.Dtos;
using ControlRH.Areas.Admin.Models.ViewModels;

namespace ControlRH.Areas.Colaborador.Contracts;

public interface IPontoEletronicoService
{
    Task<IEnumerable<PontoEletronico>> GetAllAsync(CancellationToken cancellationToken = default);
    Task<PontoEletronico?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default);
    Task<TabelaPontoEletronicoViewModel> ObterTabelaIndexAsync(DateTime? dePeriodo = null, DateTime? atePeriodo = null, int page = 1, int pageSize = 5, CancellationToken cancellationToken = default);
    Task<PontoEletronicoViewModel?> DetailsAsync(Guid id, CancellationToken cancellationToken = default);
    Task UpdateAsync(Guid id, PontoEletronicoViewModel viewModel, CancellationToken cancellationToken = default);
    Task DeleteAsync(Guid id, CancellationToken cancellationToken = default);
    Task<bool> MarcarPontoAsync(PontoEletronicoViewModel viewModel, CancellationToken cancellationToken = default);
    Task<bool> MarcarPontoApiAsync(RegistrarPontoRequest request, CancellationToken cancellationToken = default);
    Task<PontoEletronico?> UltimaMarcacaoAsync(string cpf, CancellationToken cancellationToken = default);
    Task<byte[]?> ObterEspelhoPontoAsync(CancellationToken cancellationToken = default);
    Task<DocumentoDownloadViewModel?> DownloadDocumentoAsync(Guid id, CancellationToken cancellationToken = default);
    Task<TabelaDocumentoViewModel> ObterTodosDocumentosAsync(int page = 1, int pageSize = 5, CancellationToken cancellationToken = default);

}
