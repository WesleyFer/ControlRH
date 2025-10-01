using ControlRH.Areas.Admin.Models;
using ControlRH.Areas.Colaborador.Contracts;
using ControlRH.Areas.Colaborador.Models.ViewModels;
using ControlRH.Core.Attributes;
using ControlRH.Core.Contracts;
using ControlRH.Core.Enums;
using ControlRH.Core.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Drawing.Printing;
using System.Threading;
using static ControlRH.Core.Constantes.Acoes;

namespace ControlRH.Areas.Colaborador.Controllers;

[Authorize]
[Area("Colaborador")]
[Route("Colaborador/PontoEletronico")]
public class PontoEletronicoController : Controller
{
    private readonly IPontoEletronicoService _pontoEletronicoService;

    public PontoEletronicoController(IPontoEletronicoService pontoEletronicoService)
    {
        _pontoEletronicoService = pontoEletronicoService;
    }

    [Permissao(PontoEletronicoAcoes.ConsultarPonto)]
    [HttpGet]
    public async Task<IActionResult> Index(DateTime? dePeriodo = null, DateTime? atePeriodo = null, int page = 1, int pageSize = 5)
    {
        var viewModel = await _pontoEletronicoService
            .ObterTabelaIndexAsync(dePeriodo, atePeriodo, page, pageSize);

        return View("Index", viewModel);
    }

    [Permissao(PontoEletronicoAcoes.MarcarPonto)]
    [HttpGet("MarcarPonto")]
    public async Task<IActionResult> MarcarPontoAsync()
    {
        var viewModel = new PontoEletronicoViewModel();

        await PreencherSelectListsAsync(viewModel);

        return View("MarcarPonto", viewModel);
    }

    [Permissao(PontoEletronicoAcoes.MarcarPonto)]
    [HttpPost("MarcarPonto")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> MarcarPontoAsync([FromForm] PontoEletronicoViewModel viewModel, CancellationToken cancellationToken = default)
    {
        if (!ModelState.IsValid)
        {
            await PreencherSelectListsAsync(viewModel);
            return View("MarcarPonto", viewModel);
        }

        bool sucesso = await _pontoEletronicoService
            .MarcarPontoAsync(viewModel, cancellationToken);

        if (sucesso)
        {
            ShowToast($"Ponto marcado com sucesso!", ToastType.Success);
            return RedirectToAction(nameof(Index));
        }
        else
        {
            ShowToast($"Não foi possível marcar o ponto. Verifique suas credenciais ou tente novamente.", ToastType.Success);
            await PreencherSelectListsAsync(viewModel);
            return View("MarcarPonto", viewModel);
        }
    }

    [HttpGet("ExportarEspelhoPonto")]
    public async Task<IActionResult> ExportarEspelhoPontoAsync()
    {
        var excel = await _pontoEletronicoService.ObterEspelhoPontoAsync();

        return File(excel, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", $"espelho-ponto-{DateTime.Now.ToString("dd-mm-yyyy-HH-mm-ss")}.xlsx");
    }

    [Permissao(PontoEletronicoAcoes.DocumentoPonto)]
    [HttpGet("Documento")]
    public async Task<IActionResult> DocumentoPontoAsync(int page = 1, int pageSize = 5)
    {

        var viewModel = await _pontoEletronicoService
            .ObterTodosDocumentosAsync(page, pageSize);

        return View("Documento", viewModel);
    }

    [Permissao(PontoEletronicoAcoes.DocumentoPonto)]
    [HttpGet("DownloadDocumento/{id}")]
    public async Task<IActionResult> DownloadDocumentoPontoAsync(Guid id)
    {
        var documento = await _pontoEletronicoService.DownloadDocumentoAsync(id);

        if (documento == null || documento.Conteudo.Length == 0)
            return NotFound("Arquivo não encontrado.");

        var nomeArquivo = Path.HasExtension(documento.Nome)
                            ? documento.Nome
                            : documento.Nome + ".pdf";

        return File(documento.Conteudo, "application/pdf", nomeArquivo);
    }

    private async Task PreencherSelectListsAsync(PontoEletronicoViewModel? viewModel)
    {
        if (viewModel is null)
            throw new ArgumentNullException(nameof(viewModel));

        viewModel.TiposMarcacoes();
    }

    protected void ShowToast(string message, ToastType type = ToastType.Success)
    {
        TempData["ToastType"] = type.ToString().ToLower();

        TempData["ToastMessage"] = message;
    }
}
