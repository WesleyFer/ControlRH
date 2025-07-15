using ControlRH.Core.Contracts;
using ControlRH.Core.Models;

namespace ControlRH.Areas.Admin.Models;

public class PontoMarcacaoAjuste : Entidade, IAggregateRoot
{
    protected PontoMarcacaoAjuste() { }

    public PontoMarcacaoAjuste(Guid marcacaoPontoId, TimeOnly novoHorario, string justificativa)
    {
        MarcacaoPontoId = marcacaoPontoId;
        NovoHorario = novoHorario;
        Justificativa = justificativa;
    }

    public Guid MarcacaoPontoId { get; private set; }

    public PontoMarcacao MarcacaoPonto { get; private set; }

    public TimeOnly NovoHorario { get; private set; }

    public string Justificativa { get; private set; }

    public DateTime DataAjuste { get; private set; } = DateTime.Now;
}
