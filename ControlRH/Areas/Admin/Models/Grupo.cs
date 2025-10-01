using ControlRH.Core.Enums;
using ControlRH.Core.Models;

namespace ControlRH.Areas.Admin.Models;

public class Grupo : AggregateRoot
{
    private readonly List<UsuarioGrupo> _usuariosGrupos = new();
    private readonly List<GrupoPermissao> _gruposPermissoes = new();

    protected Grupo() { }

    public Grupo(string nome, GrupoType tipo)
    {
        Nome = nome;
        Tipo = tipo;
    }

    public string Nome { get; private set; }

    public GrupoType Tipo { get; private set; }

    public IReadOnlyCollection<UsuarioGrupo> UsuariosGrupos => _usuariosGrupos.AsReadOnly();

    public IReadOnlyCollection<GrupoPermissao> GruposPermissoes => _gruposPermissoes.AsReadOnly();

    public void LimparGrupoPermissoes()
    {
        _gruposPermissoes.Clear();
    }


    public void RemoverGrupoPermissao(GrupoPermissao grupoPermissao)
    {
        _gruposPermissoes.Remove(grupoPermissao);
    }

    public void AdicionarUsuarioGrupo(Guid usuarioId, Guid grupoId)
    {
        if (_usuariosGrupos.Any(c => c.UsuarioId == usuarioId && c.GrupoId == grupoId))
            return;

        var usuarioGrupo = new UsuarioGrupo(usuarioId, grupoId);

        _usuariosGrupos.Add(usuarioGrupo);
    }

    public void AdicionarGrupoPermissao(Guid permissaoId)
    {
        if (_gruposPermissoes.Any(c => c.PermissaoId == permissaoId))
            return;

        var grupoPermissao = new GrupoPermissao(this.Id, permissaoId);

        _gruposPermissoes.Add(grupoPermissao);
    }

    public void AtualizarNome(string nome)
    {
        if (!string.IsNullOrWhiteSpace(nome) && Nome != nome)
            Nome = nome;
    }
}
