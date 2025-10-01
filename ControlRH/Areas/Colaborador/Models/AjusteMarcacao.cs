using ControlRH.Core.Contracts;
using ControlRH.Core.Models;

namespace ControlRH.Areas.Colaborador.Models;

public class AjusteMarcacao : AggregateRoot
{
    protected AjusteMarcacao() { }

    public AjusteMarcacao(
        Guid pontoEletronicoId,
        DateTime dataHora,
        string justificativa,
        string nomeResponsavelAjuste,
        string cpfResponsavelAjuste)
    {
        PontoEletronicoId = pontoEletronicoId;
        DataHora = dataHora;
        Justificativa = justificativa;
        NomeResponsavelAjuste = nomeResponsavelAjuste;
        CpfResponsavelAjuste = cpfResponsavelAjuste;
    }

    public Guid PontoEletronicoId { get; private set; }

    public PontoEletronico PontoEletronico { get; private set; }

    public DateTime DataHora { get; private set; }

    public string Justificativa { get; private set; }

    public string NomeResponsavelAjuste { get; private set; }

    public string CpfResponsavelAjuste { get; private set; }

}