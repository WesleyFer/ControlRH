//using ControlRH.Core.Contracts;
//using ControlRH.Core.Enums;
//using ControlRH.Core.Models;
//using ControlRH.Models.ViewModels;
//using Microsoft.AspNetCore.Authorization;
//using Microsoft.AspNetCore.Mvc;

//namespace ControlRH.Controllers
//{
//    public abstract class PagedController<TModel, TViewModel> : Controller
//        where TModel : Entidade, IAggregateRoot
//        where TViewModel : class, IViewModel<TModel>, new()
//    {

//        protected readonly IBaseService<TModel> _service;

//        protected PagedController(IBaseService<TModel> service)
//        {
//            _service = service;
//        }

//        protected void ShowToast(string message, ToastType type = ToastType.Success)
//        {
//            TempData["ToastType"] = type.ToString().ToLower();
//            TempData["ToastMessage"] = message;
//        }

//        protected abstract bool Export { get; }

//        protected abstract string? TextoBotaoAdicionar { get; }

//        protected abstract string ControllerRoute { get; }

//        protected abstract Dictionary<string, string> Columns { get; }

//        protected virtual IQueryable<TModel> ApplySearch(IQueryable<TModel> query, string? search) => query;

//        protected virtual Func<IQueryable<TModel>, IQueryable<TModel>>? BuildQuery(Guid? id = null, Func<IQueryable<TModel>, IQueryable<TModel>>? query = null)
//        {
//            Func<IQueryable<TModel>, IQueryable<TModel>>? currentQuery = null;

//            if (id.HasValue)
//            {
//                currentQuery = q => q.Where(e => e.Id == id.Value);
//            }

//            if (query is not null)
//            {
//                if (currentQuery is not null)
//                {
//                    currentQuery = q => query(currentQuery(q));
//                }
//                else
//                {
//                    currentQuery = query;
//                }
//            }

//            return currentQuery;
//        }

//        [HttpGet]
//        public virtual async Task<IActionResult> Index(string? search, int page = 1, int pageSize = 5, string? sort = null, string? dir = null, CancellationToken cancellationToken = default)
//        {
//            var data = (await _service.FindAsync(BuildQuery(), cancellationToken)).AsQueryable();

//            data = ApplySearch(data, search);

//            if (!string.IsNullOrEmpty(sort) && Columns.ContainsKey(sort))
//            {
//                var propInfo = typeof(TModel).GetProperty(sort);
//                if (propInfo != null)
//                {
//                    data = string.Equals(dir, "asc", StringComparison.OrdinalIgnoreCase)
//                        ? data.OrderBy(c => propInfo.GetValue(c, null))
//                        : data.OrderByDescending(c => propInfo.GetValue(c, null));
//                }
//            }

//            var totalItems = data.Count();
//            var pageData = data.Skip((page - 1) * pageSize).Take(pageSize).ToList();

//            var viewModel = new DynamicTableViewModel
//            {
//                Data = pageData.Cast<object>(),
//                Columns = Columns,
//                PageNumber = page,
//                PageSize = pageSize,
//                TotalItems = totalItems,
//                Search = search,
//                Sort = sort,
//                Dir = dir,
//                ControllerName = ControllerRoute,
//                TextoBotaoAdicionar = TextoBotaoAdicionar,
//                Export = Export
//            };

//            return View("Index", viewModel);
//        }

//        [HttpGet("details/{id}")]
//        public virtual async Task<IActionResult> Details(Guid id, CancellationToken cancellationToken)
//        {
//            var model = (await _service.FindAsync(BuildQuery(id), cancellationToken)).FirstOrDefault();

//            if (model is null)
//            {
//                ShowToast($"{nameof(TModel)} não encontrado.", ToastType.Error);
//                return RedirectToAction("Index");
//            }

//            var viewModel = new TViewModel();

//            viewModel.ToViewModel(model);

//            return View("Details", viewModel);
//        }

//        [HttpGet("create")]
//        public virtual IActionResult Create() => View("Create");

//        [HttpPost("create")]
//        [ValidateAntiForgeryToken]
//        public virtual async Task<IActionResult> Create([FromForm] TViewModel viewModel, CancellationToken cancellationToken = default)
//        {
//            if (!ModelState.IsValid)
//            {
//                ShowToast($"Validação falhou.", ToastType.Error);
//                return RedirectToAction("Create");
//            }

//            try
//            {
//                var model = viewModel.ToModel();

//                await _service.InsertAsync(model, cancellationToken);

//                if ((model as dynamic).IsValid == false) // ou outra verificação
//                {
//                    ShowToast("Erro ao salvar.", ToastType.Error);
//                    return View("Create", model);
//                }

//                return RedirectToAction("Index");
//            }
//            catch
//            {
//                ShowToast($"Erro interno.", ToastType.Error);
//                return RedirectToAction("Index");
//            }
//        }

//        [HttpGet("edit/{id}")]
//        public virtual async Task<IActionResult> Edit(Guid id, CancellationToken cancellationToken)
//        {
//            var model = (await _service.FindAsync(BuildQuery(id), cancellationToken)).FirstOrDefault();

//            if (model is null)
//            {
//                ShowToast($"{nameof(TModel)} não encontrado.", ToastType.Error);
//                return RedirectToAction("Index");
//            }

//            var viewModel = new TViewModel();

//            viewModel.ToViewModel(model);

//            return View("Edit", viewModel);
//        }

//        [HttpPost("edit/{id}")]
//        [ValidateAntiForgeryToken]
//        public virtual async Task<IActionResult> Edit(Guid id, [FromForm] TViewModel viewModel, CancellationToken cancellationToken = default)
//        {
//            if (!ModelState.IsValid)
//            {
//                ShowToast($"Validação falhou.", ToastType.Error);
//                return RedirectToAction("Index");
//            }

//            try
//            {
//                var entity = (await _service.FindAsync(BuildQuery(id), cancellationToken)).FirstOrDefault();
//                if (entity is null)
//                {
//                    ShowToast("Registro não encontrado.", ToastType.Error);
//                    return RedirectToAction("Index");
//                }

//                var model = viewModel.ToModel(entity);

//                await _service.UpdateAsync(model, cancellationToken);

//                return RedirectToAction("Index");
//            }
//            catch
//            {
//                ShowToast("Erro ao atualizar.", ToastType.Error);
//                return RedirectToAction("Index");
//            }
//        }

//        [HttpPost("delete")]
//        [ValidateAntiForgeryToken]
//        public virtual async Task<IActionResult> Delete(Guid id, CancellationToken cancellationToken = default)
//        {
//            try
//            {
//                var model = (await _service.FindAsync(BuildQuery(id), cancellationToken)).FirstOrDefault();

//                if (model is null)
//                {
//                    ShowToast("Registro não encontrado.", ToastType.Error);
//                    return RedirectToAction("Index");
//                }

//                await _service.DeleteAsync(model);

//                ShowToast("Removido com sucesso.", ToastType.Success);

//                return RedirectToAction("Index");
//            }
//            catch
//            {
//                ShowToast("Erro ao remover.", ToastType.Error);

//                return RedirectToAction("Index");
//            }
//        }
//    }
//}
