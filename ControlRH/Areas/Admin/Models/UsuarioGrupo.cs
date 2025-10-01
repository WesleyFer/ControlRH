using ControlRH.Core.Models;

namespace ControlRH.Areas.Admin.Models;

public class UsuarioGrupo : Entidade
{
    protected UsuarioGrupo() { }

    public UsuarioGrupo(Guid usuarioId, Guid grupoId)
    {
        UsuarioId = usuarioId;
        GrupoId = grupoId;
    }

    public Guid UsuarioId { get; private set; }

    public Usuario Usuario { get; private set; }

    public Guid GrupoId { get; private set; }

    public Grupo Grupo { get; private set; }
}
