using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace ControlRH.Core.Attributes;

public class AuthorizePermissaoAttribute : AuthorizeAttribute, IAuthorizationFilter
{
    private readonly string _permissao;

    public AuthorizePermissaoAttribute(string permissao)
    {
        _permissao = permissao;
    }

    public void OnAuthorization(AuthorizationFilterContext context)
    {
        var usuario = context.HttpContext.User;

        if (!usuario.Identity.IsAuthenticated)
        {
            context.Result = new UnauthorizedResult();
            return;
        }

        bool temPermissao = usuario.Claims.Any(c => c.Type == "permissao" && c.Value.Equals(_permissao, StringComparison.OrdinalIgnoreCase));

        if (!temPermissao)
        {
            context.Result = new ForbidResult();
        }
    }
}
