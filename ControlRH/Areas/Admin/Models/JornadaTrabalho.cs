using ControlRH.Core.Enums;
using ControlRH.Core.Models;

namespace ControlRH.Areas.Admin.Models;

public class JornadaTrabalho : AggregateRoot
{
    private readonly List<JornadaTrabalhoHorario> _jornadasTrabalhosHorarios = new();

    private readonly List<ColaboradorJornada> _colaboradoresJornadas = new();

    protected JornadaTrabalho() { }

    public JornadaTrabalho(string nome)
    {
        Nome = nome;
    }

    public string Nome { get; private set; }

    public IReadOnlyCollection<JornadaTrabalhoHorario> JornadasTrabalhosHorarios => _jornadasTrabalhosHorarios.AsReadOnly();

    public IReadOnlyCollection<ColaboradorJornada> ColaboradoresJornadas => _colaboradoresJornadas.AsReadOnly();

    public void AtualizarNome(string nome)
    {
        if (!string.IsNullOrWhiteSpace(nome) && Nome != nome)
            Nome = nome;
    }
    public void RemoverTodosHorarios()
    {
        _jornadasTrabalhosHorarios.Clear();
    }

    public void RemoverHorario(JornadaTrabalhoHorario horario)
    {
        _jornadasTrabalhosHorarios.Remove(horario);
    }

    public void AdicionarHorario(DiaSemanaType diaSemana, TimeSpan horaEntrada, TimeSpan horaSaida, TimeSpan duracaoIntervalo)
    {
        if (_jornadasTrabalhosHorarios.Any(h => h.DiaSemana == diaSemana))
            throw new InvalidOperationException($"Já existe um horário cadastrado para {diaSemana}.");

        var novoHorario = new JornadaTrabalhoHorario(this.Id, diaSemana, horaEntrada, horaSaida, duracaoIntervalo);
        _jornadasTrabalhosHorarios.Add(novoHorario);
    }

    public void AtualizarHorario(DiaSemanaType diaSemana, TimeSpan horaEntrada, TimeSpan horaSaida, TimeSpan duracaoIntervalo)
    {
        var horarioExistente = _jornadasTrabalhosHorarios.FirstOrDefault(h => h.DiaSemana == diaSemana);
        if (horarioExistente != null)
        {
            horarioExistente.AtualizarHoraEntrada(horaEntrada);
            horarioExistente.AtualizarHoraSaida(horaSaida);
            horarioExistente.AtualizarDuracaoIntervalo(duracaoIntervalo);
        }
    }
}
