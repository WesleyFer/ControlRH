using ControlRH.Core.Contracts;
using ControlRH.Core.Helpers;
using ControlRH.Core.Models;

namespace ControlRH.Areas.Admin.Models;

public class Usuario : AggregateRoot
{
    private readonly List<UsuarioGrupo> _usuariosGrupos = new();

    protected Usuario() { }

    public Usuario(string cpf, Guid colaboradorId)
    {
        Login = cpf;
        ColaboradorId = colaboradorId;
        SenhaHash = Utils.CryptoSenha("Mudar@123");
        DeveAlterarSenha = true;
    }

    public Usuario(Guid colaboradorId, string senha)
    {
        ColaboradorId = colaboradorId;
        SenhaHash = Utils.CryptoSenha(senha);
        DeveAlterarSenha = true;
    }

    public Usuario(string cpf, Guid colaboradorId, string senha)
    {
        Login = cpf;
        ColaboradorId = colaboradorId;
        SenhaHash = Utils.CryptoSenha(senha);
        DeveAlterarSenha = true;
    }

    public string Login { get; private set; }

    public string SenhaHash { get; private set; }

    public Guid ColaboradorId { get; private set; }

    public Colaborador Colaborador { get; private set; }

    public bool DeveAlterarSenha { get; private set; }

    public IReadOnlyCollection<UsuarioGrupo> UsuariosGrupos => _usuariosGrupos.AsReadOnly();

    public void LimparUsuarioGrupo()
    {
        _usuariosGrupos.Clear();
    }

    public void RemoverUsuarioGrupo(UsuarioGrupo usuarioGrupo)
    {
        _usuariosGrupos.Remove(usuarioGrupo);
    }

    public void AdicionarUsuarioGrupo(Guid grupoId)
    {
        if (_usuariosGrupos.Any(c => c.GrupoId == grupoId))
            return;

        var usuarioGrupo = new UsuarioGrupo(this.Id, grupoId);

        _usuariosGrupos.Add(usuarioGrupo);
    }

    public void AlterarSenha(string novaSenhaHash)
    {
        SenhaHash = Utils.CryptoSenha(novaSenhaHash);
        DeveAlterarSenha = false;
    }

    public void AlterarUsuarioGrupo(Guid grupoId)
    {
        if (_usuariosGrupos.Any(c => c.GrupoId == grupoId))
            return;

        var usuarioGrupo = new UsuarioGrupo(this.Id, grupoId);

        _usuariosGrupos.Add(usuarioGrupo);
    }
}
