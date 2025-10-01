using ControlRH.Core.Models;

namespace ControlRH.Areas.Admin.Models;

public class ColaboradorJornada : Entidade
{
    protected ColaboradorJornada() { }

    public ColaboradorJornada(Guid colaboradorId, Guid jornadaTrabalhoId)
    {
        ColaboradorId = colaboradorId;
        JornadaTrabalhoId = jornadaTrabalhoId;
    }

    public Guid ColaboradorId { get; private set; }

    public Colaborador Colaborador { get; private set; }

    public Guid JornadaTrabalhoId { get; private set; }

    public JornadaTrabalho JornadaTrabalho { get; private set; }
}
