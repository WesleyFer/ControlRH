namespace ControlRH.Core.Contracts
{
    public interface IUsuarioLogado
    {
        bool Autenticado { get; }

        string Nome { get; }

        string Cpf { get; }

        string Pis { get; }
        
        Guid ColaboradorId { get; }

        Guid CarteiraClienteId { get; }

        IEnumerable<string> Regras { get; }
    }
}
