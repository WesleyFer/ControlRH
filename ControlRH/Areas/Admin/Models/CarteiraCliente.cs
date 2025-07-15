using ControlRH.Core.Contracts;
using ControlRH.Core.Models;

namespace ControlRH.Areas.Admin.Models;

public class CarteiraCliente : Entidade, IAggregateRoot
{
    private readonly List<Colaborador> _colaboradores = new();

    protected CarteiraCliente() { }

    public CarteiraCliente(string nome)
    {
        Nome = nome;
    }

    public string Nome { get; private set; }

    public IReadOnlyCollection<Colaborador> Colaboradores => _colaboradores.AsReadOnly();

    public void AtualizarNome(string nome)
    {
        if (!string.IsNullOrWhiteSpace(nome) && Nome != nome)
            Nome = nome;
    }
}
