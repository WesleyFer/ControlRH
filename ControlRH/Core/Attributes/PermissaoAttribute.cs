using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using System.Security.Claims;
using static ControlRH.Core.Constantes.Acoes;

namespace ControlRH.Core.Attributes;

public class PermissaoAttribute : AuthorizeAttribute, IAuthorizationFilter
{
    private readonly string[] _permissoesNecessarias;

    public PermissaoAttribute(params string[] permissoesNecessarias)
    {
        _permissoesNecessarias = permissoesNecessarias;
    }

    public void OnAuthorization(AuthorizationFilterContext context)
    {
        var usuario = context.HttpContext.User;

        if (!usuario.Identity?.IsAuthenticated ?? true)
        {
            context.Result = new UnauthorizedResult(); // 401 Unauthorized
            return;
        }

        var rolesDoUsuario = usuario.FindAll(ClaimTypes.Role).Select(c => c.Value).ToList();

        if (rolesDoUsuario.Contains(AdministradoresAcoes.ControleTotal, StringComparer.OrdinalIgnoreCase))
        {
            // Se o usuário é um super administrador, ele TEM ACESSO IRRESTRITO.
            return;
        }

        if (!_permissoesNecessarias.Any(p => rolesDoUsuario.Contains(p, StringComparer.OrdinalIgnoreCase)))
        {
            context.Result = new ForbidResult(); // 403 Forbidden
            return;
        }

        //var ipUsuario = context.HttpContext.Connection.RemoteIpAddress?.ToString();
        //if (ipUsuario is not null && !ipUsuario.StartsWith("192.168.") && !ipUsuario.StartsWith("10.") && !ipUsuario.Equals("::1"))
        //{
        //    context.Result = new ForbidResult(); // 403 Forbidden
        //    return;
        //}
    }
}
