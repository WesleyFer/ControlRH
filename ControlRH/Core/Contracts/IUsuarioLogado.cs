namespace ControlRH.Core.Contracts
{
    public interface IUsuarioLogado
    {
        bool Autenticado { get; }

        string Nome { get; }

        string Email { get; }

        IEnumerable<string> Regras { get; }
    }
}
