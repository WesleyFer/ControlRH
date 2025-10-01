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

        public bool Autenticado => User?.Identity?.IsAuthenticated ?? true;

        public string Nome => User?.Identity?.Name;

        public string Cpf => User?.FindFirst("cpf")?.Value;

        public string Pis => User?.FindFirst("pis")?.Value;

        public Guid ColaboradorId => new Guid(User?.FindFirst("colaboradorId")?.Value);

        public Guid CarteiraClienteId => new(User?.FindFirst("carteiraclienteId")?.Value);

        public IEnumerable<string> Regras => User?.Claims
            .Where(c => c.Type == "roles")
            .Select(c => c.Value) ?? Enumerable.Empty<string>();
    }
}
