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
[Route("Admin/CarteiraCliente")]
public class CarteiraClienteController : Controller
{
    private readonly ICarteiraClienteService _carteiraClienteService;
    public CarteiraClienteController(ICarteiraClienteService carteiraClienteService)
    {
        _carteiraClienteService = carteiraClienteService;
    }

    [HttpGet]
    public async Task<IActionResult> Index(string? search, int page = 1, int pageSize = 5, string? sort = null, string? dir = null, CancellationToken cancellationToken = default)
    {
        var viewModel = await _carteiraClienteService
            .ObterTabelaIndexAsync(search, page, pageSize, sort, dir);

        return View("Index", viewModel);
    }

    [HttpGet("details/{id}")]
    public async Task<IActionResult> Details(Guid id, CancellationToken cancellationToken)
    {
        var viewModel = await _carteiraClienteService
            .DetailsAsync(id, cancellationToken);

        if (viewModel is null)
        {
            ShowToast("Registro não encontrado", ToastType.Warning);
            return RedirectToAction("Index", "CarteiraCliente", new { area = "Admin" });
        }

        return View("Details", viewModel);
    }

    [HttpGet("create")]
    public IActionResult Create() => View("Create");

    [HttpPost("create")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create([FromForm] CarteiraClienteViewModel viewModel, CancellationToken cancellationToken = default)
    {
        if (!ModelState.IsValid)
            return View("Create", viewModel);

        try
        {
            await _carteiraClienteService.InsertAsync(viewModel, cancellationToken);
            return RedirectToAction("Index", "CarteiraCliente", new { area = "Admin" });
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
        var viewModel = await _carteiraClienteService
            .DetailsAsync(id, cancellationToken);

        if (viewModel is null)
        {
            ShowToast("Registro não encontrado", ToastType.Warning);
            return RedirectToAction("Index", "CarteiraCliente", new { area = "Admin" });
        }

        return View("Edit", viewModel);
    }

    [HttpPost("edit/{id}")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(Guid id, [FromForm] CarteiraClienteViewModel viewModel, CancellationToken cancellationToken = default)
    {
        if (!ModelState.IsValid)
            return View("Edit", viewModel);

        try
        {
            await _carteiraClienteService.UpdateAsync(id, viewModel, cancellationToken);
            return RedirectToAction("Index", "CarteiraCliente", new { area = "Admin" });
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
            await _carteiraClienteService.DeleteAsync(id, cancellationToken);
            ShowToast("Removido com sucesso.", ToastType.Success);
            return RedirectToAction("Index", "CarteiraCliente", new { area = "Admin" });
        }
        catch
        {
            ShowToast("Erro ao remover.", ToastType.Error);
            return RedirectToAction("Index", "CarteiraCliente", new { area = "Admin" });
        }
    }

    private void ShowToast(string message, ToastType type = ToastType.Success)
    {
        TempData["ToastType"] = type.ToString().ToLower();
        TempData["ToastMessage"] = message;
    }
}
