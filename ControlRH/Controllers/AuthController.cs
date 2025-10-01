using ControlRH.Core.Contracts;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using static ControlRH.Core.Constantes.Acoes;


namespace ControlRH.Controllers;

[AllowAnonymous]
public class AuthController : Controller
{
    private readonly IAuthService _authService;

    public AuthController(IAuthService authService)
    {
        _authService = authService;
    }

    [HttpGet]
    public IActionResult Index()
    {
        return View();
    }

    [HttpGet]
    public IActionResult Login() => View();

    [HttpPost]
    public async Task<IActionResult> Login(string login, string senha)
    {
        var sucesso = await _authService.AutenticarAsync(login, senha, HttpContext);

        if (!sucesso)
        {
            ModelState.AddModelError("", "Login ou senha inválidos.");
            return View();
        }

        // Redirecionar imediatamente para garantir que o cookie seja aplicado no próximo request
        return RedirectToAction("RedirecionarAposLogin");
    }

    [HttpGet]
    public IActionResult RedirecionarAposLogin()
    {
        var usuario = HttpContext.User;

        if (!usuario.Identity.IsAuthenticated)
        {
            return RedirectToAction("Login");
        }

        if (usuario.IsInRole(AdministradoresAcoes.ControleTotal))
        {
            return RedirectToAction("Index", "Home");
        }
        else if (usuario.IsInRole(PontoEletronicoAcoes.MarcarPonto))
        {
            return RedirectToAction("Index", "PontoEletronico", new { area = "Colaborador" });
        }

        return RedirectToAction("Login");
    }

    [HttpGet]
    public async Task<IActionResult> Logout()
    {
        await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
        return RedirectToAction("Login");
    }

    [HttpGet]
    public IActionResult AcessoNegado() => View();
}
