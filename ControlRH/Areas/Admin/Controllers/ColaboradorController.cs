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
[Route("Admin/Colaborador")]
public class ColaboradorController : Controller
{
    private readonly IColaboradorService _colaboradorService;

    public ColaboradorController(IColaboradorService colaboradorService)
    {
        _colaboradorService = colaboradorService;
    }

    [HttpGet]
    public async Task<IActionResult> Index(string? search, int page = 1, int pageSize = 5, string? sort = null, string? dir = null, CancellationToken cancellationToken = default)
    {
        var viewModel = await _colaboradorService
            .ObterTabelaIndexAsync(search, page, pageSize, sort, dir);

        return View("Index", viewModel);
    }

    [HttpGet("Details/{id}")]
    public async Task<IActionResult> Details(Guid id, CancellationToken cancellationToken)
    {
        var viewModel = await _colaboradorService
           .DetailsAsync(id, cancellationToken);

        await PreencherSelectListsAsync(viewModel);

        return View("Details", viewModel);
    }

    [HttpGet("Create")]
    public async Task<IActionResult> Create()
    {
        var viewModel = new ColaboradorViewModel();

        await PreencherSelectListsAsync(viewModel);

        return View("Create", viewModel);
    }

    [HttpPost("Create")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create([FromForm] ColaboradorViewModel viewModel, CancellationToken cancellationToken = default)
    {
        if (!ModelState.IsValid)
        {
            var erros = ModelState
              .Where(ms => ms.Value.Errors.Count > 0)
              .SelectMany(ms => ms.Value.Errors)
              .Select(e => e.ErrorMessage)
              .FirstOrDefault();

            ShowToast($"Validação falhou. {erros.ToString()}", ToastType.Error);

            await PreencherSelectListsAsync(viewModel);

            return View("Create", viewModel);
        }

        try
        {
            await _colaboradorService.InserirColaboradorComUsuarioAsync(viewModel, cancellationToken);
            return RedirectToAction("Index", "Colaborador", new { area = "Admin" });
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
        var viewModel = await _colaboradorService
           .DetailsAsync(id, cancellationToken);

        await PreencherSelectListsAsync(viewModel);

        return View("Edit", viewModel);
    }

    [HttpPost("Edit/{id}")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(Guid id, [FromForm] ColaboradorViewModel viewModel, CancellationToken cancellationToken = default)
    {
        if (!ModelState.IsValid)
        {
            ShowToast($"Validação falhou.", ToastType.Error);
            return View("Edit", viewModel);
        }

        try
        {
            await _colaboradorService.UpdateAsync(id, viewModel, cancellationToken);
            return RedirectToAction("Index", "Colaborador", new { area = "Admin" });
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
            await _colaboradorService.DeleteAsync(id, cancellationToken);
            ShowToast("Removido com sucesso.", ToastType.Success);
            return RedirectToAction("Index", "Colaborador", new { area = "Admin" });
        }
        catch
        {
            ShowToast("Erro ao remover.", ToastType.Error);
            return RedirectToAction("Index", "Colaborador", new { area = "Admin" });
        }
    }

    private void ShowToast(string message, ToastType type = ToastType.Success)
    {
        TempData["ToastType"] = type.ToString().ToLower();
        TempData["ToastMessage"] = message;
    }

    private async Task PreencherSelectListsAsync(ColaboradorViewModel? viewModel)
    {
        var carteirasClientes = await _colaboradorService.CarteirasClientesAsync();
        var jornadasTrabalhos = await _colaboradorService.JornadasTrabalhosAsync();
        var cargos = await _colaboradorService.CargosAsync();

        viewModel?.CarteirasClientes(carteirasClientes);
        viewModel?.JornadasTrabalhos(jornadasTrabalhos);
        viewModel?.Cargos(cargos);
    }
}
