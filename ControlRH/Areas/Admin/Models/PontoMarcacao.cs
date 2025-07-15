using ControlRH.Core.Contracts;
using ControlRH.Core.Enums;
using ControlRH.Core.Models;

namespace ControlRH.Areas.Admin.Models;

public class PontoMarcacao : Entidade, IAggregateRoot
{
    protected PontoMarcacao() { }

    public PontoMarcacao(MarcacaoPontoType tipo, TimeOnly horario)
    {
        Tipo = tipo;
        Horario = horario;
    }

    public MarcacaoPontoType Tipo { get; private set; }

    public TimeOnly Horario { get; private set; }

    public DateTime DataRegistro { get; private set; } = DateTime.Now;

    public string OrigemMarcacao { get; private set; }

    public string IPDispositivo { get; private set; }

    public string DispositivoId { get; private set; }

}
