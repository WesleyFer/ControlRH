using ControlRH.Core.Models;

namespace ControlRH.Areas.Admin.Models;

public class Cargo : AggregateRoot
{
    private readonly List<Colaborador> _colaboradores = new();

    public Cargo(string nome)
    {
        Nome = nome;
    }

    protected Cargo() { }

    public string Nome { get; private set; }

    public IReadOnlyCollection<Colaborador> Colaboradores => _colaboradores.AsReadOnly();

    public void AtualizarNome(string nome)
    {
        if (!string.IsNullOrEmpty(nome))
            Nome = nome;
    }
}
