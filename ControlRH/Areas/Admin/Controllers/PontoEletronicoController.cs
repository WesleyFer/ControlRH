using ControlRH.Areas.Admin.Contracts;
using ControlRH.Areas.Admin.Models.ViewModels;
using ControlRH.Core.Enums;
using Microsoft.AspNetCore.Mvc;

namespace ControlRH.Areas.Admin.Controllers;

[Area("Admin")]
[Route("Admin/PontoEletronico")]
public class PontoEletronicoController : Controller
{

    public PontoEletronicoController()
    {
    }

    public async Task<IActionResult> Index(string? search, int page = 1, int pageSize = 5, string? sort = null, string? dir = null)
    {
        return View("Index");
    }

    [HttpGet("Details/{id}")]
    public async Task<IActionResult> Details(Guid id, CancellationToken cancellationToken)
    {
        return View("Details", new PontoEletronicoViewModel());
    }

    [HttpGet("Create")]
    public IActionResult Create()
    {
        return View("Create", new PontoEletronicoViewModel());
    }

    [HttpGet("Edit/{id}")]
    public async Task<IActionResult> Edit(Guid id, CancellationToken cancellationToken)
    {
        return View("Edit", new PontoEletronicoViewModel());
    }

}

//[Area("Admin")]
//[Route("Admin/PontoEletronico")]
//public class PontoEletronicoController : Controller
//{
//    private readonly IPontoEletronicoService _pontoEletronicoService;

//    public PontoEletronicoController(IPontoEletronicoService pontoEletronicoService)
//    {
//        _pontoEletronicoService = pontoEletronicoService;
//    }

//    public async Task<IActionResult> Index(string? search, int page = 1, int pageSize = 5, string? sort = null, string? dir = null)
//    {
//        var viewModel = await _pontoEletronicoService
//            .ObterTabelaIndexAsync(search, page, pageSize, sort, dir);

//        return View("Index", viewModel);
//    }

//    [HttpGet("Details/{id}")]
//    public async Task<IActionResult> Details(Guid id, CancellationToken cancellationToken)
//    {
//        var viewModel = await _pontoEletronicoService
//            .DetailsAsync(id, cancellationToken);

//        if (viewModel is null)
//        {
//            ShowToast("Registro não encontrado", ToastType.Warning);
//            return RedirectToAction("Index", "PontoEletronico", new { area = "Admin" });
//        }

//        return View("Details", viewModel);
//    }

//    [HttpGet("Create")]
//    public IActionResult Create()
//    {
//        return View("Create", new PontoEletronicoViewModel());
//    }

//    [HttpPost("Create")]
//    [ValidateAntiForgeryToken]
//    public async Task<IActionResult> Create([FromForm] PontoEletronicoViewModel viewModel, CancellationToken cancellationToken = default)
//    {
//        if (!ModelState.IsValid)
//            return View("Create", viewModel);

//        try
//        {
//            await _pontoEletronicoService.InsertAsync(viewModel, cancellationToken);
//            return RedirectToAction("Index", "PontoEletronico", new { area = "Admin" });
//        }
//        catch
//        {
//            ShowToast($"Erro interno.", ToastType.Error);
//            return View("Create", viewModel);
//        }
//    }

//    [HttpGet("Edit/{id}")]
//    public async Task<IActionResult> Edit(Guid id, CancellationToken cancellationToken)
//    {
//        var viewModel = await _pontoEletronicoService
//             .DetailsAsync(id, cancellationToken);

//        if (viewModel is null)
//        {
//            ShowToast("Registro não encontrado", ToastType.Warning);
//            return RedirectToAction("Index", "PontoEletronico", new { area = "Admin" });
//        }

//        return View("Edit", viewModel);
//    }

//    [HttpPost("Edit/{id}")]
//    [ValidateAntiForgeryToken]
//    public async Task<IActionResult> Edit(Guid id, [FromForm] PontoEletronicoViewModel viewModel, CancellationToken cancellationToken = default)
//    {
//        if (!ModelState.IsValid)
//            return View("Edit", viewModel);

//        try
//        {
//            await _pontoEletronicoService.UpdateAsync(id, viewModel, cancellationToken);
//            return RedirectToAction("Index", "PontoEletronico", new { area = "Admin" });
//        }
//        catch
//        {
//            ShowToast("Erro ao atualizar.", ToastType.Error);
//            return View("Edit", viewModel);
//        }
//    }

//    [HttpPost("Delete")]
//    [ValidateAntiForgeryToken]
//    public async Task<IActionResult> Delete(Guid id, CancellationToken cancellationToken = default)
//    {
//        try
//        {
//            await _pontoEletronicoService.DeleteAsync(id, cancellationToken);
//            ShowToast("Removido com sucesso.", ToastType.Success);
//            return RedirectToAction("Index", "PontoEletronico", new { area = "Admin" });
//        }
//        catch
//        {
//            ShowToast("Erro ao remover.", ToastType.Error);
//            return RedirectToAction("Index", "PontoEletronico", new { area = "Admin" });
//        }
//    }

//    private void ShowToast(string message, ToastType type = ToastType.Success)
//    {
//        TempData["ToastType"] = type.ToString().ToLower();
//        TempData["ToastMessage"] = message;
//    }
//}

