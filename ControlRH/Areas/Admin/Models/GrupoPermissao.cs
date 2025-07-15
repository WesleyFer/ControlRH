using ControlRH.Core.Models;

namespace ControlRH.Areas.Admin.Models;

public class GrupoPermissao : Entidade
{

    protected GrupoPermissao() { }

    public GrupoPermissao(Guid grupoId, Guid permissaoId)
    {
        GrupoId = grupoId;
        PermissaoId = permissaoId;
    }

    public Guid GrupoId { get; private set; }

    public Grupo Grupo { get; private set; }

    public Guid PermissaoId { get; private set; }

    public Permissao Permissao { get; private set; }
}
