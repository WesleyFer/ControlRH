using ControlRH.Areas.Admin.Contracts;
using ControlRH.Areas.Admin.Models.ViewModels;
using ControlRH.Core.Attributes;
using ControlRH.Core.Enums;
using Microsoft.AspNetCore.Mvc;
using static ControlRH.Core.Constantes.Acoes;

namespace ControlRH.Areas.Admin.Controllers;

[Permissao(AdministradoresAcoes.ControleTotal)]
[Area("Admin")]
[Route("Admin/Usuario")]
public class UsuarioController : Controller
{

    private readonly IUsuarioService _usuarioService;

    public UsuarioController(IUsuarioService usuarioService)
    {
        _usuarioService = usuarioService;
    }

    [HttpGet]
    public async Task<IActionResult> Index(string? search, int page = 1, int pageSize = 5, string? sort = null, string? dir = null, CancellationToken cancellationToken = default)
    {
        var viewModel = await _usuarioService
            .ObterTabelaIndexAsync(search, page, pageSize, sort, dir);

        return View("Index", viewModel);
    }

    [HttpGet("Details/{id}")]
    public async Task<IActionResult> Details(Guid id, CancellationToken cancellationToken)
    {
        var viewModel = await _usuarioService
           .DetailsAsync(id, cancellationToken);

        await PreencherSelectListsAsync(viewModel);

        return View("Details", viewModel);
    }

    [HttpGet("Create")]
    public async Task<IActionResult> Create()
    {
        var viewModel = new UsuarioViewModel();

        await PreencherSelectListsAsync(viewModel);

        return View("Create", viewModel);
    }

    [HttpPost("Create")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create([FromForm] UsuarioViewModel viewModel, CancellationToken cancellationToken = default)
    {
        if (!ModelState.IsValid)
        {
            var erros = ModelState
              .Where(ms => ms.Value.Errors.Count > 0)
              .SelectMany(ms => ms.Value.Errors)
              .Select(e => e.ErrorMessage)
              .FirstOrDefault();

            ShowToast($"Validação falhou. {erros.ToString()}", ToastType.Error);
            return View("Create", viewModel);
        }

        try
        {
            await _usuarioService.InsertAsync(viewModel, cancellationToken);
            return RedirectToAction("Index", "Usuario", new { area = "Admin" });
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
        var viewModel = await _usuarioService
           .DetailsAsync(id, cancellationToken);

        await PreencherSelectListsAsync(viewModel);

        return View("Edit", viewModel);
    }

    [HttpPost("Edit/{id}")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(Guid id, [FromForm] UsuarioViewModel viewModel, CancellationToken cancellationToken = default)
    {
        if (!ModelState.IsValid)
        {
            var erros = ModelState
                .Where(ms => ms.Value.Errors.Count > 0)
                .SelectMany(ms => ms.Value.Errors)
                .Select(e => e.ErrorMessage)
                .FirstOrDefault();

            ShowToast($"Validação falhou. {erros.ToString()}", ToastType.Error);
            return View("Edit", viewModel);
        }

        try
        {
            await _usuarioService.UpdateAsync(id, viewModel, cancellationToken);
            return RedirectToAction("Index", "Usuario", new { area = "Admin" });
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
            await _usuarioService.DeleteAsync(id, cancellationToken);
            ShowToast("Removido com sucesso.", ToastType.Success);
            return RedirectToAction("Index", "Usuario", new { area = "Admin" });
        }
        catch
        {
            ShowToast("Erro ao remover.", ToastType.Error);
            return RedirectToAction("Index", "Usuario", new { area = "Admin" });
        }
    }

    private void ShowToast(string message, ToastType type = ToastType.Success)
    {
        TempData["ToastType"] = type.ToString().ToLower();
        TempData["ToastMessage"] = message;
    }

    private async Task PreencherSelectListsAsync(UsuarioViewModel? viewModel)
    {
        var colaboradores = await _usuarioService.ColaboradoresAsync();
        var grupos = await _usuarioService.GruposAsync();

        viewModel?.SetColaboradores(colaboradores);
        viewModel?.SetGrupos(grupos);
    }
}
