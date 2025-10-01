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
[Route("Admin/Grupo")]
public class GrupoController : Controller
{

    private readonly IGrupoService _grupoService;

    public GrupoController(IGrupoService grupoService)
    {
        _grupoService = grupoService;
    }

    [HttpGet]
    public async Task<IActionResult> Index(string? search, int page = 1, int pageSize = 5, string? sort = null, string? dir = null, CancellationToken cancellationToken = default)
    {
        var viewModel = await _grupoService
            .ObterTabelaIndexAsync(search, page, pageSize, sort, dir);

        return View("Index", viewModel);
    }

    [HttpGet("Details/{id}")]
    public async Task<IActionResult> Details(Guid id, CancellationToken cancellationToken)
    {
        var viewModel = await _grupoService
           .DetailsAsync(id, cancellationToken);

        await PreencherSelectListsAsync(viewModel);

        return View("Details", viewModel);
    }

    [HttpGet("Create")]
    public async Task<IActionResult> Create()
    {
        var viewModel = new GrupoViewModel();

        await PreencherSelectListsAsync(viewModel);

        return View("Create", viewModel);
    }

    [HttpPost("Create")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create([FromForm] GrupoViewModel viewModel, CancellationToken cancellationToken = default)
    {
        if (!ModelState.IsValid)
        {
            ShowToast($"Validação falhou.", ToastType.Error);
            return View("Create", viewModel);
        }

        try
        {
            await _grupoService.InsertAsync(viewModel, cancellationToken);
            return RedirectToAction("Index", "Grupo", new { area = "Admin" });
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
        var viewModel = await _grupoService
           .DetailsAsync(id, cancellationToken);

        await PreencherSelectListsAsync(viewModel);

        return View("Edit", viewModel);
    }

    [HttpPost("Edit/{id}")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(Guid id, [FromForm] GrupoViewModel viewModel, CancellationToken cancellationToken = default)
    {
        if (!ModelState.IsValid)
        {
            ShowToast($"Validação falhou.", ToastType.Error);
            return View("Edit", viewModel);
        }

        try
        {
            await _grupoService.UpdateAsync(id, viewModel, cancellationToken);
            return RedirectToAction("Index", "Grupo", new { area = "Admin" });
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
            await _grupoService.DeleteAsync(id, cancellationToken);
            ShowToast("Removido com sucesso.", ToastType.Success);
            return RedirectToAction("Index", "Grupo", new { area = "Admin" });
        }
        catch
        {
            ShowToast("Erro ao remover.", ToastType.Error);
            return RedirectToAction("Index", "Grupo", new { area = "Admin" });
        }
    }


    private void ShowToast(string message, ToastType type = ToastType.Success)
    {
        TempData["ToastType"] = type.ToString().ToLower();
        TempData["ToastMessage"] = message;
    }

    private async Task PreencherSelectListsAsync(GrupoViewModel viewModel)
    {
        var permissoes = await _grupoService.ObterPermissoesDisponiveisAsync();
        viewModel.PermissoesDisponiveis = permissoes.ToList();
    }
}
