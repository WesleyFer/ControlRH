using ControlRH.Api.Dtos;
using ControlRH.Areas.Colaborador.Contracts;
using ControlRH.Areas.Colaborador.Models.ViewModels;
using ControlRH.Core.Contracts;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ControlRH.Api;

[Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
[Route("api")]
[ApiController]
public class ApiController : ControllerBase
{
    private readonly IAuthService _authService;
    private readonly IPontoEletronicoService _pontoEletronicoService;
    private readonly IUsuarioLogado _usuarioLogado;

    public ApiController(
        IAuthService authService,
        IPontoEletronicoService pontoEletronicoService,
        IUsuarioLogado usuarioLogado)
    {
        _authService = authService;
        _pontoEletronicoService = pontoEletronicoService;
        _usuarioLogado = usuarioLogado;
    }

    [AllowAnonymous]
    [HttpPost("Autenticar")]
    public async Task<IActionResult> AutenticarAsync([FromBody] LoginRequest request)
    {
        var token = await _authService
            .AutenticarTokenAsync(request.Login, request.Senha);

        if (string.IsNullOrEmpty(token))
            return StatusCode(400, "Login ou senha inválidos.");

        return Ok(new
        {
            sucesso = true,
            retorno = token
        });
    }

    [HttpPost("MarcarPonto")]
    public async Task<IActionResult> MarcarPontoAsync([FromBody] RegistrarPontoRequest request, CancellationToken cancellationToken)
    {

        bool sucesso = await _pontoEletronicoService
           .MarcarPontoApiAsync(request, cancellationToken);

        if (!sucesso)
            return StatusCode(500, "Erro ao registrar ponto.");

        return Ok(new
        {
            sucesso = true,
            retorno = "Ponto registrado com sucesso."
        });
    }
}
