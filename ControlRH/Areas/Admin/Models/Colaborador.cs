using ControlRH.Core.Models;

namespace ControlRH.Areas.Admin.Models;

public class Colaborador : AggregateRoot
{
    private readonly List<ColaboradorJornada> _colaboradoresJornadas = new();

    private readonly List<Documento> _documentos = new();

    protected Colaborador() { }

    public Colaborador(string cpf, string pis, string matricula, string nome, Guid cargoId, Guid carteiraClienteId)
    {
        Cpf = cpf;
        Pis = pis;
        Matricula = matricula;
        Nome = nome;
        CargoId = cargoId;
        CarteiraClienteId = carteiraClienteId;
        Ativo = true;
    }

    public string Cpf { get; private set; }
    public string Pis { get; private set; }
    public string Matricula { get; private set; }
    public string Nome { get; private set; }
    public Guid CargoId { get; private set; }
    public Cargo Cargo { get; private set; }
    public Guid CarteiraClienteId { get; private set; }
    public CarteiraCliente CarteiraCliente { get; private set; }
    public bool Ativo { get; private set; }
    public IReadOnlyCollection<ColaboradorJornada> ColaboradoresJornadas => _colaboradoresJornadas.AsReadOnly();
    public IReadOnlyCollection<Documento> Documentos => _documentos.AsReadOnly();
    public void AdicionarJornadaTrabalho(Guid jornadaTrabalhoId)
    {
        if (_colaboradoresJornadas.Any(cj => cj.JornadaTrabalhoId == jornadaTrabalhoId))
            return;

        var colaboradorJornada = new ColaboradorJornada(this.Id, jornadaTrabalhoId);

        _colaboradoresJornadas.Add(colaboradorJornada);
    }

    public void AtualizarCpf(string cpf)
    {
        if (!string.IsNullOrWhiteSpace(cpf) && Cpf != cpf)
            Cpf = cpf;
    }

    public void AtualizarPis(string pis)
    {
        if (!string.IsNullOrWhiteSpace(pis) && Pis != pis)
            Pis = pis;
    }

    public void AtualizarMatricula(string matricula)
    {
        if (!string.IsNullOrWhiteSpace(matricula) && Matricula != matricula)
            Matricula = matricula;
    }

    public void AtualizarNome(string nome)
    {
        if (!string.IsNullOrWhiteSpace(nome) && Nome != nome)
            Nome = nome;
    }

    public void AtualizarCargo(Guid cargoId)
    {
        if (cargoId != Guid.Empty && CargoId != cargoId)
            CargoId = cargoId;
    }

    public void AtualizarCarteiraCliente(Guid carteiraClienteId)
    {
        if (Guid.Empty != carteiraClienteId)
            CarteiraClienteId = carteiraClienteId;
    }

    public void AtivarColaborador()
    {
        Ativo = true;
    }

    public void InativarColaborador()
    {
        Ativo = false;
    }
}
