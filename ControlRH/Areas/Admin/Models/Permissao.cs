using ControlRH.Core.Enums;
using ControlRH.Core.Models;

namespace ControlRH.Areas.Admin.Models;

public class Permissao : Entidade
{
    private readonly List<GrupoPermissao> _gruposPermissoes = new();

    protected Permissao() { }

    public string Chave { get; private set; }

    public string Valor { get; private set; }

    public PermissaoType Tipo { get; private set; }

    public IReadOnlyCollection<GrupoPermissao> GruposPermissoes => _gruposPermissoes.AsReadOnly();
}
