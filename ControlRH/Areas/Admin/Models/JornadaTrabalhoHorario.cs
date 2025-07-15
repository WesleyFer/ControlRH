using ControlRH.Core.Enums;
using ControlRH.Core.Models;

namespace ControlRH.Areas.Admin.Models;

public class JornadaTrabalhoHorario : Entidade
{
    protected JornadaTrabalhoHorario() { }

    public JornadaTrabalhoHorario(Guid jornadaTrabalhoId, DiaSemanaType diaSemana, TimeSpan horaEntrada, TimeSpan horaSaida, TimeSpan duracaoIntervalo)
    {
        JornadaTrabalhoId = jornadaTrabalhoId;
        DiaSemana = diaSemana;
        HoraEntrada = horaEntrada;
        HoraSaida = horaSaida;
        DuracaoIntervalo = duracaoIntervalo;
    }

    public Guid JornadaTrabalhoId { get; private set; }

    public DiaSemanaType DiaSemana { get; private set; }

    public TimeSpan HoraEntrada { get; private set; }

    public TimeSpan HoraSaida { get; private set; }

    public TimeSpan DuracaoIntervalo { get; private set; }

    public JornadaTrabalho JornadaTrabalho { get; private set; }

    public void AtualizarDiaSemana(DiaSemanaType diaSemana)
    {
        DiaSemana = diaSemana;
    }

    public void AtualizarHoraEntrada(TimeSpan horaEntrada)
    {
        HoraEntrada = horaEntrada;
    }

    public void AtualizarHoraSaida(TimeSpan horaSaida)
    {
        HoraSaida = horaSaida;
    }

    public void AtualizarDuracaoIntervalo(TimeSpan duracaoIntervalo)
    {
        DuracaoIntervalo = duracaoIntervalo;
    }
}
