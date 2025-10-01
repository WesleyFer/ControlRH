using ControlRH.Areas.Admin.Contracts;
using ControlRH.Areas.Admin.Models.ViewModels;
using ControlRH.Core.Attributes;
using ControlRH.Core.Enums;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using static ControlRH.Core.Constantes.Acoes;

namespace ControlRH.Areas.Admin.Controllers;

[Permissao(AdministradoresAcoes.ControleTotal)]
[Area("Admin")]
[Route("Admin/JornadaTrabalho")]
public class JornadaTrabalhoController : Controller
{
    private readonly IJornadaTrabalhoService _jornadaTrabalhoService;

    public JornadaTrabalhoController(IJornadaTrabalhoService jornadaTrabalhoService)
    {
        _jornadaTrabalhoService = jornadaTrabalhoService;
    }

    [HttpGet]
    public async Task<IActionResult> Index(string? search, int page = 1, int pageSize = 5, string? sort = null, string? dir = null)
    {
        var viewModel = await _jornadaTrabalhoService
            .ObterTabelaIndexAsync(search, page, pageSize, sort, dir);

        return View("Index", viewModel);
    }

    [HttpGet("Details/{id}")]
    public async Task<IActionResult> Details(Guid id, CancellationToken cancellationToken)
    {
        var viewModel = await _jornadaTrabalhoService
            .DetailsAsync(id, cancellationToken);

        if (viewModel is null)
        {
            ShowToast("Registro não encontrado", ToastType.Warning);
            return RedirectToAction("Index", "JornadaTrabalho", new { area = "Admin" });
        }

        return View("Details", viewModel);
    }

    [HttpGet("Create")]
    public IActionResult Create()
    {
        return View("Create", new JornadaTrabalhoViewModel());
    }

    [HttpPost("Create")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create([FromForm] JornadaTrabalhoViewModel viewModel, CancellationToken cancellationToken = default)
    {
        if (!ModelState.IsValid)
            return View("Create", viewModel);

        try
        {
            await _jornadaTrabalhoService.InsertAsync(viewModel, cancellationToken);
            return RedirectToAction("Index", "JornadaTrabalho", new { area = "Admin" });
        }
        catch
        {
            ShowToast($"Erro interno.", ToastType.Error);
            return View("Create", viewModel);
        }
    }

    [HttpGet("Edit/{id}")]
    public async Task<IActionResult> Edit(Guid id, CancellationToken cancellationToken)
    {
        var viewModel = await _jornadaTrabalhoService
             .DetailsAsync(id, cancellationToken);

        if (viewModel is null)
        {
            ShowToast("Registro não encontrado", ToastType.Warning);
            return RedirectToAction("Index", "JornadaTrabalho", new { area = "Admin" });
        }

        return View("Edit", viewModel);
    }

    [HttpPost("Edit/{id}")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(Guid id, [FromForm] JornadaTrabalhoViewModel viewModel, CancellationToken cancellationToken = default)
    {
        if (!ModelState.IsValid)
            return View("Edit", viewModel);

        try
        {
            await _jornadaTrabalhoService.UpdateAsync(id, viewModel, cancellationToken);
            return RedirectToAction("Index", "JornadaTrabalho", new { area = "Admin" });
        }
        catch
        {
            ShowToast("Erro ao atualizar.", ToastType.Error);
            return View("Edit", viewModel);
        }
    }

    [HttpPost("Delete")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Delete(Guid id, CancellationToken cancellationToken = default)
    {
        try
        {
            await _jornadaTrabalhoService.DeleteAsync(id, cancellationToken);
            ShowToast("Removido com sucesso.", ToastType.Success);
            return RedirectToAction("Index", "JornadaTrabalho", new { area = "Admin" });
        }
        catch
        {
            ShowToast("Erro ao remover.", ToastType.Error);
            return RedirectToAction("Index", "JornadaTrabalho", new { area = "Admin" });
        }
    }

    private void ShowToast(string message, ToastType type = ToastType.Success)
    {
        TempData["ToastType"] = type.ToString().ToLower();
        TempData["ToastMessage"] = message;
    }
}
