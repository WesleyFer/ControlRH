using System.Security.Claims;
using ControlRH.Core.Contracts;

namespace ControlRH.Core.Models
{
    public class UsuarioLogado : IUsuarioLogado
    {

        private readonly IHttpContextAccessor _httpContextAccessor;

        public UsuarioLogado(IHttpContextAccessor httpContextAccessor)
        {
            _httpContextAccessor = httpContextAccessor;
        }

        private ClaimsPrincipal? User => _httpContextAccessor?.HttpContext?.User;

        public bool Autenticado => User?.Identity?.IsAuthenticated ?? false;

        public string Nome => User?.Identity?.Name ?? string.Empty;

        public string Email => User?.FindFirst("email")?.Value ?? string.Empty;

        public IEnumerable<string> Regras => User?.Claims
            .Where(c => c.Type == "roles")
            .Select(c => c.Value) ?? Enumerable.Empty<string>();
    }
}
