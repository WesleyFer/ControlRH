using ControlRH.Core.Contracts;
using ControlRH.Core.Enums;
using System.ComponentModel.DataAnnotations;

namespace ControlRH.Areas.Admin.Models.ViewModels;

public class JornadaTrabalhoViewModel : IViewModel<JornadaTrabalho>
{
    public Guid? Id { get; set; }

    [Required(ErrorMessage = "Nome é obrigatório.")]
    public string Nome { get; set; }

    // Horário comum para todos os dias selecionados
    [Required(ErrorMessage = "Hora de entrada é obrigatória")]
    public TimeSpan HoraEntrada { get; set; }

    [Required(ErrorMessage = "Hora de saída é obrigatória")]
    public TimeSpan HoraSaida { get; set; }

    [Required(ErrorMessage = "Duração do intervalo é obrigatório")]
    public TimeSpan DuracaoIntervalo { get; set; }

    [Required(ErrorMessage = "Selecione pelo menos um dia da semana.")]
    public List<DiaSemanaType> DiasSelecionados { get; set; } = new();

    public List<JornadaTrabalhoHorarioViewModel> JornadasTrabalhosHorarios { get; set; } = new();

    public JornadaTrabalho ToModel()
    {
        var model = new JornadaTrabalho(Nome);

        foreach (DiaSemanaType dia in Enum.GetValues(typeof(DiaSemanaType)))
        {
            if (DiasSelecionados.Contains(dia))
            {
                model.AdicionarHorario(dia, HoraEntrada, HoraSaida, DuracaoIntervalo);
            }
            else
            {
                model.AdicionarHorario(dia, TimeSpan.Zero, TimeSpan.Zero, TimeSpan.Zero);
            }
        }

        return model;
    }

    public void ToViewModel(JornadaTrabalho entidade)
    {
        Id = entidade.Id;
        Nome = entidade.Nome;

        if (entidade.JornadasTrabalhosHorarios.Any())
        {
            var primeiro = entidade.JornadasTrabalhosHorarios.First();

            HoraEntrada = primeiro.HoraEntrada;
            HoraSaida = primeiro.HoraSaida;
            DuracaoIntervalo = primeiro.DuracaoIntervalo;
            DiasSelecionados = entidade.JornadasTrabalhosHorarios
                .Where(c => c.HoraEntrada != TimeSpan.Zero && c.HoraSaida != TimeSpan.Zero && c.DuracaoIntervalo != TimeSpan.Zero)
                .Select(c => c.DiaSemana)
                .ToList();

        }
    }
}

public class JornadaTrabalhoHorarioViewModel
{
    public Guid? Id { get; set; }

    public Guid JornadaTrabalhoId { get; set; }

    [Required(ErrorMessage = "Dia da semana é obrigatório.")]
    public DiaSemanaType DiaSemana { get; set; }

    [Required(ErrorMessage = "Hora da entrada é obrigatório.")]
    public TimeSpan HoraEntrada { get; set; }

    [Required(ErrorMessage = "Hora da saída é obrigatório.")]
    public TimeSpan HoraSaida { get; set; }

    [Required(ErrorMessage = "Duração do intervalo é obrigatório.")]
    public TimeSpan DuracaoIntervalo { get; set; }
}