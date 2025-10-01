using ControlRH.Areas.Admin.Contracts;
using ControlRH.Areas.Admin.Models.ViewModels;
using ControlRH.Core.Attributes;
using ControlRH.Core.Enums;
using Microsoft.AspNetCore.Mvc;
using static ControlRH.Core.Constantes.Acoes;

namespace ControlRH.Areas.Admin.Controllers;

[Permissao(AdministradoresAcoes.ControleTotal)]
[Area("Admin")]
[Route("Admin/Documento")]
public class DocumentoController : Controller
{
    private readonly IDocumentoService _documentoService;
    public DocumentoController(IDocumentoService documentoService)
    {
        _documentoService = documentoService;
    }

    [HttpGet]
    public async Task<IActionResult> Index(string? search, int page = 1, int pageSize = 5, string? sort = null, string? dir = null, CancellationToken cancellationToken = default)
    {
        var viewModel = await _documentoService
            .ObterTabelaIndexAsync(search, page, pageSize, sort, dir);

        return View("Index", viewModel);
    }

    [HttpGet("details/{id}")]
    public async Task<IActionResult> Details(Guid id, CancellationToken cancellationToken)
    {
        var viewModel = await _documentoService
            .DetailsAsync(id, cancellationToken);

        if (viewModel is null)
        {
            ShowToast("Registro não encontrado", ToastType.Warning);
            return RedirectToAction("Index", "Documento", new { area = "Admin" });
        }

        await PreencherSelectListsAsync(viewModel);

        return View("Details", viewModel);
    }

    [HttpGet("create")]
    public async Task<IActionResult> Create()
    {
        var viewModel = new DocumentoViewModel();

        await PreencherSelectListsAsync(viewModel);

        return View("Create", viewModel);
    }

    [HttpPost("create")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create([FromForm] DocumentoViewModel viewModel, CancellationToken cancellationToken = default)
    {
        if (viewModel.Arquivo != null)
        {
            var extension = Path.GetExtension(viewModel.Arquivo.FileName).ToLowerInvariant();
            if (extension != ".pdf")
            {
                ModelState.AddModelError("Arquivo", "Somente arquivos PDF são permitidos.");
            }
        }

        if (!ModelState.IsValid)
        {
            await PreencherSelectListsAsync(viewModel);

            return View("Create", viewModel);
        }

        try
        {
            await _documentoService.InsertAsync(viewModel, cancellationToken);
            return RedirectToAction("Index", "Documento", new { area = "Admin" });
        }
        catch
        {
            ShowToast($"Erro interno.", ToastType.Error);
            return View("Create", viewModel);
        }
    }

    [HttpGet("edit/{id}")]
    public async Task<IActionResult> Edit(Guid id, CancellationToken cancellationToken)
    {
        var viewModel = await _documentoService
            .DetailsAsync(id, cancellationToken);

        if (viewModel is null)
        {
            ShowToast("Registro não encontrado", ToastType.Warning);
            return RedirectToAction("Index", "Documento", new { area = "Admin" });
        }

        await PreencherSelectListsAsync(viewModel);

        return View("Edit", viewModel);
    }

    [HttpPost("edit/{id}")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(Guid id, [FromForm] DocumentoViewModel viewModel, CancellationToken cancellationToken = default)
    {
        if (!ModelState.IsValid)
            return View("Edit", viewModel);

        try
        {
            await _documentoService.UpdateAsync(id, viewModel, cancellationToken);
            return RedirectToAction("Index", "Documento", new { area = "Admin" });
        }
        catch
        {
            ShowToast("Erro ao atualizar.", ToastType.Error);
            return View("Edit", viewModel);
        }
    }

    [HttpPost("delete")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Delete(Guid id, CancellationToken cancellationToken = default)
    {
        try
        {
            await _documentoService.DeleteAsync(id, cancellationToken);
            ShowToast("Removido com sucesso.", ToastType.Success);
            return RedirectToAction("Index", "Documento", new { area = "Admin" });
        }
        catch
        {
            ShowToast("Erro ao remover.", ToastType.Error);
            return RedirectToAction("Index", "Documento", new { area = "Admin" });
        }
    }

    [HttpGet("ColaboradoresPorCarteira")]
    public async Task<IActionResult> GetColaboradoresPorCarteira(Guid carteiraClienteId, CancellationToken cancellationToken = default)
    {
        if (carteiraClienteId == Guid.Empty)
            return Json(Enumerable.Empty<object>());

        var colaboradores = await _documentoService.ColaboradoresPorCarteiraAsync(carteiraClienteId, cancellationToken);

        // retorna só o necessário pro select
        var result = colaboradores.Select(c => new
        {
            id = c.Id,
            nome = c.Nome
        });

        return Json(result);
    }

    private void ShowToast(string message, ToastType type = ToastType.Success)
    {
        TempData["ToastType"] = type.ToString().ToLower();
        TempData["ToastMessage"] = message;
    }

    private async Task PreencherSelectListsAsync(DocumentoViewModel? viewModel)
    {
        if (viewModel == null) 
            return;

        var carteirasClientes = await _documentoService.CarteirasClientesAsync();
        viewModel?.CarteirasClientes(carteirasClientes);

        if (viewModel.CarteiraClienteId != Guid.Empty)
        {
            var colaboradores = await _documentoService.ColaboradoresPorCarteiraAsync(viewModel.CarteiraClienteId);
            viewModel.Colaboradores(colaboradores);
        }
        else
        {
            viewModel.Colaboradores(Enumerable.Empty<Models.Colaborador>());
        }
    }
}
